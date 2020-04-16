using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Suri.DTO
{
    public class LoginDTO
    {

        [Required(AllowEmptyStrings = false, ErrorMessage= "Digite su usuario")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Digite su contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
