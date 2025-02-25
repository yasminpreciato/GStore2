using System.Configuration;
using GStore2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GStore2.Data;

    public class AppDbContext : IdentityDbContext<Usuario>
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
       {
       }
       
      public DbSet<Categoria> Categorias { get; set; }

      public DbSet<Produto> Produtos { get; set; }

      public DbSet<ProdutoFoto> produtoFotos { get; set; }

      public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        #region Definição dos Nomes do Entity
        builder.Entity<Usuario>().ToTable("usuario");
        builder.Entity<IdentityRole>().ToTable("perfil");
        builder.Entity<IdentityUserRole<string>>().ToTable("usuario_perfil");
        builder.Entity<IdentityUserClaim<string>>().ToTable("usuario_regra");
        builder.Entity<IdentityUserToken<string>>().ToTable("usuario_token");
        builder.Entity<IdentityUserLogin<string>>().ToTable("usuario_Login");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("perfil_regra");
       #endregion
      
      AppDbSeed seed = new(builder);
    }
}
