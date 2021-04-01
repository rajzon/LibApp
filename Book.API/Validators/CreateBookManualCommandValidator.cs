﻿using System.Data;
using System.Linq;
using Book.API.Commands.V1;
using FluentValidation;

namespace Book.API.Validators
{
    public class CreateBookManualCommandValidator : AbstractValidator<CreateBookManualCommand>
    {
        public CreateBookManualCommandValidator()
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

            RuleFor(b => b.AuthorId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            
            RuleFor(b => b.LanguageId)
                .GreaterThanOrEqualTo(1);
            
            RuleFor(b => b.PublisherId)
                .GreaterThanOrEqualTo(1);
            
            
            //TODO consider place for storing rules
            const int categoryIdMinValue = 1;
            RuleFor(b => b.CategoriesIds)
                .ForEach(c =>
                    c.GreaterThanOrEqualTo(categoryIdMinValue)
                        .WithMessage($"Category id must be greater than {categoryIdMinValue - 1}"));

        }
    }
}