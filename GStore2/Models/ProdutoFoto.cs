using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GStore2.Models;

    [Table("produto_foto")]
    public class ProdutoFoto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Por favor, informe o Produto")]
        public int ProdutoId{ get; set; }
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        
        [Display(Name ="Arquivo")]
        [Required(ErrorMessage ="Por favor, informe o Arquivo")]
        [StringLength(300)]
        public string ArquivoFoto { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(100, ErrorMessage = "A descrição deve possuir no máximo 100 caracteres")]
        public string Descricao { get; set; }
    }
