using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VDMS.Models
{
    public class RoleViewModel : Microsoft.AspNet.Identity.EntityFramework.IdentityRole
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public bool Disabled { get; set; }
    }
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool Disabled { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }

}