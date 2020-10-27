using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.NETCore3._1.Models
{
    //[Table("Usuário")] Se eu quiser alterar o nome 
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Este campo é obrigatório")]
        [MaxLength(20,ErrorMessage ="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3,ErrorMessage ="Este campo deve conter entre 3 e 60 caracteres")]
        public string Password { get; set; }

        //[Column("Perfil")] Caso eu queira colocar outro tipo de nome na coluna do banco de dados
        public string Role { get; set; }//tipo de perfil
    }
}
