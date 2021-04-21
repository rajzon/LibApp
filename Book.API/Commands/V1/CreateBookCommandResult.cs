using System.Collections.Generic;
using Book.API.Commands.V1.Dtos;
using Book.API.Domain;

namespace Book.API.Commands.V1
{
    public class CreateBookCommandResult : BaseCommandResult
    {
        public CommandBookDto Book { get; set; }

        public CreateBookCommandResult(bool succeeded, CommandBookDto book) 
            : base(succeeded)
        {
            Book = book;
        }

        public CreateBookCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default, CommandBookDto book = default) 
            : base(succeeded, errors)
        {
            Book = book;
        }
    }
}