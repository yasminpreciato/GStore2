using System.Net.Mail;
using System.Security.Claims;
using GStore2.Data;
using GStore2.Helpers;
using GStore2.Models;
using GStore2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GStore2.Controllers;
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    private readonly IWebHostEnvironment _host;
    private readonly AppDbContext _db;

    public AccountController(
        ILogger<AccountController> logger,
        SignInManager<Usuario> signInManager,
        UserManager<Usuario> userManager,
        IWebHostEnvironment host,
        AppDbContext db
    )
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _host = host;
        _db = db;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        LoginVM login = new()
        {
            UrlRetorno = returnUrl ?? Url.Content("~/")
        };
        return View(login);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM login)
    {
        if (ModelState.IsValid)
        {
            string userName = login.Email;
            if (IsValidEmail(login.Email))
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                    userName = user.UserName;
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName, login.Senha, login.Lembrar, lockoutOnFailure: true
            );

            if (result.Succeeded) {
                _logger.LogInformation($"Usuário {login.Email} acessou o sistema");
                return LocalRedirect(login.UrlRetorno);
            }
            if (result.IsLockedOut) {
                _logger.LogWarning($"Usuário {login.Email} está bloqueado");
                ModelState.AddModelError("", "Sua conta está bloqueada, aguardar alguns minutos e tente novamente!!");
            }
            else
            if (result.IsNotAllowed) {
                _logger.LogWarning($"Usuário {login.Email} não confirmou sua conta");
                ModelState.AddModelError(string.Empty, "Sua conta não está confirmada, verefique seu email!!");
            }
            else
                ModelState.AddModelError(string.Empty, "Usuário e/ou Senha Invalidos!!!");
        }
        return View(login);
    }

       [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation($"Usuário {ClaimTypes.Email} fez logoff");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
     [HttpGet]
    public IActionResult Registro()
    {
        RegistroVM register = new();
        return View(register);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroVM registro)
    {
        if (ModelState.IsValid)
        {
            var usuario = Activator.CreateInstance<Usuario>();
            usuario.Nome = registro.Nome;
            usuario.DataNascimento = registro.DataNascimento;
            usuario.UserName = registro.Email;
            usuario.NormalizedUserName = registro.Email.ToUpper();
            usuario.Email = registro.Email;
            usuario.NormalizedEmail = registro.Email.ToUpper();
            usuario.EmailConfirmed = true;
            var result = await _userManager.CreateAsync(usuario, registro.Senha);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Novo usuário registro com o email {registro.Email}");

                await _userManager.AddToRoleAsync(usuario, "Cliente");

                if (registro.Foto != null)
                {
                    string nomeArquivo = usuario.Id + Path.GetExtension(registro.Foto.FileName);
                    string caminho = Path.Combine(_host.WebRootPath, "img\\usuarios");
                    string novoArquivo = Path.Combine(caminho, nomeArquivo);
                    using (var stream = new FileStream(novoArquivo, FileMode.Create))
                    {
                        registro.Foto.CopyTo(stream);
                    }
                    usuario.Foto = @"\img\usuarios\" + nomeArquivo;
                    await _db.SaveChangesAsync();
                }
                TempData["Success"] = "Conta Criada com Sucesso!";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, TranslateIdentityErrors.TranslateErrorMessage(error.Code));
        }
        return View(registro);
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
    
    public bool IsValidEmail(string email)
    {
        try
        {
            MailAddress m = new(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}
