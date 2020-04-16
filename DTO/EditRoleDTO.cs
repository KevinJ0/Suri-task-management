using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suri.DTO
{
    public class EditRoleDTO 
    {

       public  EditRoleDTO() {
          Users = new List<string>();
        }
        public string Id { get; set; }
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
