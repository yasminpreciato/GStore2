using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GStore2.Models;

    [Table("produto")]
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, informe a Categoria")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "Por favor, informe o Nome")]
        [StringLength(60, ErrorMessage = "O nome deve possuir no máximo 60 caracteres")]
        public string Nome { get; set; }

        [Display(Name ="Descrição", Prompt = "Descrição")]
        [StringLength(1000, ErrorMessage = "A descrição deve possuir no máximo 1000 caracteres")]
        public string Descricao { get; set; }

        [Display(Name = "Quantidade em Estoque")]
        [Required(ErrorMessage ="Por favor, informe a quantidade em estoque")]
        [Range(0, int.MaxValue)]
        public int QtdeEstoque { get; set; }

        [Display(Name ="Valor de Custo")]
        [Range(0, double.MaxValue)]
        [Required(ErrorMessage = "Por favor, informe o valor de custo")]
        [Column(TypeName ="decimal(10,2)")] //99.999.999,99
        public decimal ValorCusto{ get; set; }

        [Display(Name ="Valor de Venda")]
        [Range(0, double.MaxValue)]
        [Required(ErrorMessage = "Por favor, informe o valor de venda")]
        [Column(TypeName ="decimal(10,2)")] 
        public decimal  ValorVenda { get; set; }
  
    }
