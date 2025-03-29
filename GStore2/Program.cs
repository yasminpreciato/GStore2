using Google.Protobuf.WellKnownTypes;
using GStore2.Data;
using GStore2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Serviço da conexão com o banco de dados
string conexao = builder.Configuration.GetConnectionString("GStore2Conn");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySQL(conexao)
);

// Serviço de getão do Usuario - Identity
builder.Services.AddIdentity<Usuario, IdentityRole>(
    Options => Options.SignIn.RequireConfirmedEmail = false 
  
).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


var app = builder.Build();

// Garantir que o banco exista ao o projeto
using (var scope = app.Services.CreateScope())
{
    var DbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();   
    await DbContext.Database.EnsureCreatedAsync(); 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
