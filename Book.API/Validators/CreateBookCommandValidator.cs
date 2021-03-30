using System;
using System.Text.RegularExpressions;
using Book.API.Commands.V1;
using FluentValidation;

namespace Book.API.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(b => b.Title)
                .MinimumLength(3)
                .MaximumLength(50);
            
            RuleFor(b => b.Description)
                .MinimumLength(3)
                .MaximumLength(5000);
            
            RuleFor(b => b.Isbn10)
                .Length(10)
                .Matches("^[0-9]+$").WithMessage("ISBN10 Field must contain only digits");
            RuleFor(b => b.Isbn13)
                .Length(13)
                .Matches("^[0-9]+$").WithMessage("ISBN13 Field must contain only digits");
            
            RuleFor(b => b.PageCount)
                .GreaterThanOrEqualTo((ushort) 1);
            
            RuleFor(b => b.LanguageName)
                .MinimumLength(2)
                .MaximumLength(20)
                .Matches("^[a-zA-Z]+$").WithMessage("Language name can contain only letters");

            RuleFor(b => b.Author)
                .NotNull();
            
            RuleFor(b => b.Author.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .Matches("^[^0-9]+$").WithMessage("Author Firstname cannot contain any digits")
                .When(b => b.Author is not null);
            
            RuleFor(b => b.Author.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .Matches("^[^0-9]+$").WithMessage("Author Lastname cannot contain any digits")
                .When(b => b.Author is not null);

            RuleFor(b => b.PublisherName)
                .MinimumLength(2)
                .MaximumLength(40);
            
            RuleFor(b => b.CategoriesNames)
                .ForEach(c => 
                    c.MinimumLength(3)
                        .MaximumLength(30));


        }
    }
}