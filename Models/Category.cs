using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.NETCore3._1.Models

{   //[Table("Categoria")] Caso eu queira colocar outro nome no banco de dados 
    public class Category
    {
        [Key]
        //[Column("id")] Se eu quiser colocar outro nome na coluna 
        public int id { get; set; }

        [Required(ErrorMessage ="Este campo é obrigatório")]
        [MaxLength(60,ErrorMessage ="Este campo deve conter entre 3 a 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
        //[DataType("nvarchar")] posso alterar o tipo do dado se eu quiser
        public string Title { get; set; }


    }
}
