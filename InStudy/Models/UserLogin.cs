/*!
*\file User Login file
* 
*\brief UserLogin class to process Login page

*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InStudy.Models
{
    public class UserLogin
    {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        public String EmailID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}