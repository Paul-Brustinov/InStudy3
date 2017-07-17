using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RegistrationAndLogin.Models;

namespace InStudy.Models
{
    [FluentValidation.Attributes.Validator(typeof(UserValidator))]
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name = "First Name")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        //[StringLength(20, ErrorMessage = "Maximum string legth is 20")]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Second name required")]
        //[StringLength(20, ErrorMessage = "Maximum string legth is 20")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateBirth { get; set; }

        [Display(Name = "Password")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "Password should contain 6 or more character")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Confirm pasword and password not match")]
        public string ConfirmPassword { get; set; }

    }
}