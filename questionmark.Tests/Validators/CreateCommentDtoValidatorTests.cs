using FluentValidation.TestHelper;
using NSubstitute;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Posts.Validators;
using Xunit;

namespace questionmark.Tests.Unit.Validators;

public class CreateCommentDtoValidatorTests
{
    private readonly IPost _postRepo = Substitute.For<IPost>();
    private readonly CreateCommentDtoValidator _sut;
    private const uint ExistingPostId = 1u;
    private const string ValidText = "This is valid commment for sure.";
    public CreateCommentDtoValidatorTests()
    {
        _sut = new CreateCommentDtoValidator(_postRepo);
        _postRepo.DoesPostExists(ExistingPostId).Returns(true);
        _postRepo.DoesPostExists(Arg.Is<uint>(id => id != ExistingPostId)).Returns(false);
    }
    [Fact]
    public async Task Validate_ValidComment_ShouldHaveNoErrors()
    {
        var dto = new CreateCommentDto { postId = ExistingPostId, text = ValidText };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public async Task Validate_EmptyText_ShouldReturnCommentIsRequiredError()
    {
        var dto = new CreateCommentDto { postId = ExistingPostId, text = string.Empty };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.text)
              .WithErrorMessage("Comment is required.");
    }
    [Theory]
    [InlineData(1)]
    [InlineData(14)]
    public async Task Validate_TextTooShort_ShouldReturnMinimumLengthError(int length)
    {
        var dto = new CreateCommentDto { postId = ExistingPostId, text = new string('x', length) };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.text)
              .WithErrorMessage("Comment must be at least 15 characters long.");
    }
    [Theory]
    [InlineData(513)]
    [InlineData(1000)]
    public async Task Validate_TextTooLong_ShouldReturnMaximumLengthError(int length)
    {
        var dto = new CreateCommentDto { postId = ExistingPostId, text = new string('x', length) };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.text)
              .WithErrorMessage("Comment can't be longer than 512 characters.");
    }
    [Fact]
    public async Task Validate_NonExistingPostId_ShouldReturnPostDoesNotExistError()
    {
        const uint nonExistingId = 9999u;
        var dto = new CreateCommentDto { postId = nonExistingId, text = ValidText };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.postId)
              .WithErrorMessage("Post with this ID does not exists.");
    }
    [Fact]
    public async Task Validate_ExistingPostId_ShouldHaveNoErrors()
    {
        var dto = new CreateCommentDto { postId = ExistingPostId, text = ValidText };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.postId);
    }
    [Fact]
    public async Task Validate_PostIdZero_ShouldReturnPostIdIsRequiredError()
    {
        var dto = new CreateCommentDto { postId = 0u, text = ValidText };
        var result = await _sut.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.postId)
              .WithErrorMessage("Post ID is required.");
    }
}