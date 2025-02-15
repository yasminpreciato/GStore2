using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GStore2.Models;

    [Table("categoria")]
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome")]
        [StringLength(30, ErrorMessage = "O nome deve possuir no maximo 30 caracteres")]
        public string Nome { get; set; }
        
        public string Foto { get; set; }
    }
