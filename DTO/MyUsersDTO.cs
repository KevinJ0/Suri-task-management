using Microsoft.AspNetCore.Identity;
using Suri.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Suri.DTO
{
    public class MyUsersDTO : IdentityUser
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "No coincide las contraseñas")]
        public string PasswordConfirm { get; set; }
        public virtual ICollection<Actividades> Actividades { get; set; }
    }
}
