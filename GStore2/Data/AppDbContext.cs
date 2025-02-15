using GStore2.Models;
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

      public DbSet<ProdutoFoto> produtoFoto { get; set; }

      public DbSet<Usuario> Usuarios { get; set; }


    }
