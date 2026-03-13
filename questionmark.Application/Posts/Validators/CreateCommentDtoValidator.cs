using FluentValidation;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.Validators
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        private readonly IPost _postRepository;
        public CreateCommentDtoValidator(IPost postRepository)
        {
            _postRepository = postRepository;
            RuleFor(x => x.text).NotEmpty().WithMessage("Comment is required.").MinimumLength(15).WithMessage("Comment must be at least 15 characters long.").MaximumLength(512).WithMessage("Comment can't be longer than 512 characters.");
            RuleFor(x => x.postId).NotEmpty().WithMessage("Post ID is required.").MustAsync((x, cancellation) => doesPostExists(x)).WithMessage("Post with this ID does not exists.");
        }
        private async Task<bool> doesPostExists(uint postId)
        {
            var task = await _postRepository.DoesPostExists(postId);
            return task;
        }
    }
}
