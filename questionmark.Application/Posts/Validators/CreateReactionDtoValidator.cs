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
    public class CreateReactionDtoValidator : AbstractValidator<CreateReactionDto>
    {
        private readonly IPost _postRepository;
        public CreateReactionDtoValidator(IPost postRepository) {
            _postRepository = postRepository;
            RuleFor(x => x.currentReaction).InclusiveBetween(0, 2).WithMessage("Reaction is not in good format.");
            RuleFor(x => x.postId).NotEmpty().WithMessage("Post ID is required.").MustAsync((x, cancellation) => doesPostExists(x)).WithMessage("Post with this ID does not exists.");
        }
        private async Task<bool> doesPostExists(uint postId)
        {
            var task = await _postRepository.DoesPostExists(postId);
            return task;
        }
    }
}
