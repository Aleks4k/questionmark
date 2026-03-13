using FluentValidation;
using questionmark.Application.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.Validators
{
    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostDtoValidator()
        {
            RuleFor(x => x.text).NotEmpty().WithMessage("Post is required.").MinimumLength(15).WithMessage("Post must be at least 15 characters long.").MaximumLength(512).WithMessage("Post can't be longer than 512 characters.");
        }
    }
}
