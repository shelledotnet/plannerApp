using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Models
{
    
    public class LoginRequest
    {
        [Required(ErrorMessage ="email id required")]
        [EmailAddress(ErrorMessage ="invalid email id")]
        public string Email { get; set; }


        [Required(ErrorMessage ="password required")]
        [StringLength(20,MinimumLength=6,ErrorMessage ="invalid character lenght for password")]
        public string Password { get; set; }

    }


}
