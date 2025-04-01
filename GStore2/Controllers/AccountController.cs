using System.Net.Mail;
using System.Security.Claims;
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

    public AccountController(
        ILogger<AccountController> logger,
        SignInManager<Usuario> signInManager,
        UserManager<Usuario> userManager,
        IWebHostEnvironment host
    )
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _host = host;
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
                var user = awalt _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                    userName = user.userName;
            }  

            var result = awalt _signInManager.PasswordSignInAsync(
                userName, login.Senha, login.Lembrar, lockoutDnFailure: true
            );

            if (result.Succeeded) {
                _logger.LogInformation($"Usuário {login.Email} acessou o sistema");
                return LocalRedirect(login.UrlRetorno);
            }

            if (result.IsLockedOut){
                _logger.LoginWarning($"Usuário {login.Email} está bloqueado");
                ModelState.AddModelError("", "Sua conta está bloqueada, aguardar alguns minutos e tente novamente!!");
            }
            else
            if (result.IsNotAllowed) {
                _logger.LoginWarning($"Usuário {login.Email} não confirmou sua conta");
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
        awalt _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
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