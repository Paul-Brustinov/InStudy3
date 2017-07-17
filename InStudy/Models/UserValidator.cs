using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using InStudy.Models;

namespace RegistrationAndLogin.Models
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Length(4, 20);
            RuleFor(x => x.ConfirmPassword).NotNull().Equal(x=>x.Password);
            RuleFor(x => x.EmailID).NotNull().EmailAddress();
        }

    }
}