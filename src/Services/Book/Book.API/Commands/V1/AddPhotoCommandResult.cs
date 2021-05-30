using System.Collections.Generic;
using Book.API.Commands.V1.Dtos;

namespace Book.API.Commands.V1
{
    public class AddPhotoCommandResult : BaseCommandResult
    {
        public CommandPhotoDto Photo { get; set; }

        public AddPhotoCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default, CommandPhotoDto result = default)
            :base(succeeded, errors)
        {
            Photo = result;
        }
        
        public AddPhotoCommandResult(bool succeeded, CommandPhotoDto result = default)
            :base(succeeded)
        {
            Photo = result;
        }
        
        
    }
}