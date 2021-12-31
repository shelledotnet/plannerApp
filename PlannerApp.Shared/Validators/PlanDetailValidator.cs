using FluentValidation;
using PlannerApp.Shared.Models;

namespace PlannerApp.Shared.Validators
{
    public class PlanDetailValidator : AbstractValidator<PlanDetail>
    {
        public PlanDetailValidator()
        {
            RuleFor(p => p.Title)
                  .NotEmpty().WithMessage("title is required")  //title must not empty else withmessage
                  .MaximumLength(80).WithMessage("title must not be more than 80 characters");//title must not be more than 80 else withmesaage

            RuleFor(p => p.Description)
                   .MaximumLength(500).WithMessage("description must not be more than 500 characters");//description max lenght is 500 else withmessage
          
        }
    }
    
}
