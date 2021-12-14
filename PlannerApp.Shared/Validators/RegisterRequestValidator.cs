using FluentValidation;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Validators
{

    #region Install_FluentValidation here on the calling App Install Blazor.FluentValidation
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(p => p.Email)
                  .NotEmpty().WithMessage("email id required")  //email must not empty else withmessage
                  .EmailAddress().WithMessage("invalid email id");//email must be valid emailAddress else withmessage

            RuleFor(p => p.FirstName)
                   .NotEmpty().WithMessage("firstName required")//firstname must not be empty else withmessage
                   .MaximumLength(25).WithMessage("firstname must be less than 25 characters");//firstname max lenght is 15 else withmessage

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("lastName required")
                .MaximumLength(25).WithMessage("firstname must be less than 25 characters");
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("password required")
                .MinimumLength(5).WithMessage("password must at least 5 characters");
            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("confirm password required")//confirmpassword must not be empty else withmessage
                .Equal(p => p.Password).WithMessage("confirm password doesn't match password");//confirmpassword must be equal to password else withmessage

        }
    } 
    #endregion
}
