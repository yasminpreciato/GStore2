using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace GStore2.Models;

    [Table("usuario")]
    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "Por favor, informe o nome")]
        [StringLength(60, ErrorMessage ="o nome deve possuir no maximo 60 caracteres")]
        public string Nome{ get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [StringLength(300)]
        public  string Foto {get; set;}
    }
