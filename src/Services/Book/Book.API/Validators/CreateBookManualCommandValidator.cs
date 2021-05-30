using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Book.API.Commands.V1;
using Book.API.Domain;
using Book.API.Domain.Common;
using Book.API.Extensions;
using FluentValidation;

namespace Book.API.Validators
{
    public class CreateBookManualCommandValidator : AbstractValidator<CreateBookManualCommand>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IPublisherRepository _publisherRepository;

        public CreateBookManualCommandValidator(IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository, 
            ILanguageRepository languageRepository, 
            IPublisherRepository publisherRepository)
        {
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _languageRepository = languageRepository;
            _publisherRepository = publisherRepository;
            
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
            
            const int authorIdMinValue = 1;
            RuleFor(b => b.AuthorsIds)
                .NotEmpty()
                .ForEach(a =>
                    a.GreaterThanOrEqualTo(authorIdMinValue)
                        .WithMessage($"Author id must be greater then {authorIdMinValue - 1}"))
                .AllValuesMustExistsInDb(nameof(Author.Id), _authorRepository.GetAllAsync());

            RuleFor(b => b.LanguageId)
                .GreaterThanOrEqualTo(1)
                .ValueMustExistsInDb(id => _languageRepository.FindByIdAsync(id ?? 0));
                
            
            RuleFor(b => b.PublisherId)
                .GreaterThanOrEqualTo(1)
                .ValueMustExistsInDb(id => _publisherRepository.FindByIdAsync(id ?? 0));
            
            const int categoryIdMinValue = 1;
            RuleFor(b => b.CategoriesIds)
                .ForEach(c =>
                    c.GreaterThanOrEqualTo(categoryIdMinValue)
                        .WithMessage($"Category id must be greater than {categoryIdMinValue - 1}"))
                .AllValuesMustExistsInDb(nameof(Category.Id), _categoryRepository.GetAllAsync());
        }
    }
}