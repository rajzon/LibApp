using System;
using System.Text.RegularExpressions;
using Book.API.Commands.V1;
using Book.API.Domain;
using FluentValidation;

namespace Book.API.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(b => b.Title)
                .MinimumLength(3)
                .MaximumLength(100);
            
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
            
            RuleFor(b => b.AuthorsNames)
                .NotNull();
            RuleFor(b => b.AuthorsNames)
                .ForEach(a =>
                    a.NotEmpty()
                        .MinimumLength(7)
                        .MaximumLength(61)
                        .Matches("^[^0-9]+$")
                        .WithMessage("Author name cannot contain any digits")
                );

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