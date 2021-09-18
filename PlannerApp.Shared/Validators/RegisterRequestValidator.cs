using FluentValidation;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Validators
{
   public  class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(p => p.Email)
                  .NotEmpty().WithMessage("email id required")
                  .EmailAddress().WithMessage("invaslid email id");

            RuleFor(p => p.FirstName)
                   .NotEmpty().WithMessage("firstName required")
                   .MaximumLength(15).WithMessage("firstname must be less than 25 characters");

            RuleFor(p=>p.LastName)
                .NotEmpty().WithMessage("lastName required")
                .MaximumLength(15).WithMessage("firstname must be less than 25 characters");
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("password required")
                .MinimumLength(5).WithMessage("password must at least 5 characters");
            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("confirm password required")
                .Equal(p => p.Password).WithMessage("confirm password doesn't match password");

        }
    }
}
