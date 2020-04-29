using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Suri.DTO
{
    public class EditUserDTO
    {

        public EditUserDTO()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        [Required]
        public string Id { get; set; }
        [Required]
        [DisplayName("Nombre")]
        public string UserName { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }

    }
}
