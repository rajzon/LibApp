using Book.API.Commands.V1;
using FluentValidation;

namespace Book.API.Validators
{
    public class AddPhotoCommandValidator : AbstractValidator<AddPhotoCommand>
    {
        public AddPhotoCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(p => p.File)
                .NotEmpty();
        }
    }
}