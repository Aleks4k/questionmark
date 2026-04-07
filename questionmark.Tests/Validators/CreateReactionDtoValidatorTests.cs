using FluentValidation.TestHelper;
using NSubstitute;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Posts.Validators;
using Xunit;

namespace questionmark.Tests.Unit.Validators;

public class CreateReactionDtoValidatorTests
{
    private readonly IPost _postRepo = Substitute.For<IPost>();
    private readonly CreateReactionDtoValidator reactionValidator;
    private const uint ExistingPostId = 1u;
    public CreateReactionDtoValidatorTests()
    {
        reactionValidator = new CreateReactionDtoValidator(_postRepo);
        _postRepo.DoesPostExists(ExistingPostId).Returns(true);
        _postRepo.DoesPostExists(Arg.Is<uint>(id => id != ExistingPostId)).Returns(false);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Validate_ValidReaction_ShouldHaveNoErrors(int reaction)
    {
        var dto = new CreateReactionDto { postId = ExistingPostId, currentReaction = reaction };
        var result = await reactionValidator.TestValidateAsync(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(3)]
    [InlineData(100)]
    public async Task Validate_ReactionOutOfRange_ShouldReturnReactionIsNotInGoodFormatError(int reaction)
    {
        var dto = new CreateReactionDto { postId = ExistingPostId, currentReaction = reaction };
        var result = await reactionValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.currentReaction)
              .WithErrorMessage("Reaction is not in good format.");
    }
    [Fact]
    public async Task Validate_NonExistingPostId_ShouldReturnPostDoesNotExistError()
    {
        const uint nonExistingId = 9999u;
        var dto = new CreateReactionDto { postId = nonExistingId, currentReaction = 1 };
        var result = await reactionValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.postId)
              .WithErrorMessage("Post with this ID does not exists.");
    }
    [Fact]
    public async Task Validate_ExistingPostId_ShouldHaveNoErrors()
    {
        var dto = new CreateReactionDto { postId = ExistingPostId, currentReaction = 1 };
        var result = await reactionValidator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.postId);
    }
    [Fact]
    public async Task Validate_PostIdZero_ShouldReturnPostIdIsRequiredError()
    {
        var dto = new CreateReactionDto { postId = 0u, currentReaction = 1 };
        var result = await reactionValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.postId)
              .WithErrorMessage("Post ID is required.");
    }
    [Fact]
    public async Task Validate_NonExistingPostIdAndReactionOutOfRange_ShouldReturnBothErrors()
    {
        var dto = new CreateReactionDto { postId = 9999u, currentReaction = 5 };
        var result = await reactionValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.currentReaction);
        result.ShouldHaveValidationErrorFor(x => x.postId);
    }
}