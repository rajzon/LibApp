using Book.API.Commands.V1;
using Book.API.Domain;
using Book.API.Extensions;
using FluentValidation;

namespace Book.API.Validators
{
    public class AddPhotoCommandValidator : AbstractValidator<AddPhotoCommand>
    {
        private readonly IBookRepository _bookRepository;

        
        public AddPhotoCommandValidator(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            
            RuleFor(p => p.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .ValueMustExistsInDb(id => _bookRepository.FindByIdAsync(id));

            RuleFor(p => p.File)
                .NotEmpty();
        }
    }
}