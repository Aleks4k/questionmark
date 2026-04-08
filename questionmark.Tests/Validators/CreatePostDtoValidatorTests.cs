using FluentValidation.TestHelper;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Posts.Validators;
using Xunit;

namespace questionmark.Tests.Unit.Validators;

public class CreatePostDtoValidatorTests
{
    private readonly CreatePostDtoValidator createPostValidator = new();
    [Theory]
    [InlineData(14)]
    [InlineData(256)]
    [InlineData(512)]
    public async Task Validate_ValidText_ShouldHaveNoErrors(int length)
    {
        var dto = new CreatePostDto { text = new string('x', length) };
        var result = await createPostValidator.TestValidateAsync(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public async Task Validate_EmptyText_ShouldReturnPostIsRequiredError()
    {
        var dto = new CreatePostDto { text = string.Empty };
        var result = await createPostValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.text)
              .WithErrorMessage("Post is required.");
    }
    [Theory]
    [InlineData(1)]
    [InlineData(14)]
    public async Task Validate_TextTooShort_ShouldReturnMinimumLengthError(int length)
    {
        var dto = new CreatePostDto { text = new string('x', length) };
        var result = await createPostValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.text)
              .WithErrorMessage("Post must be at least 15 characters long.");
    }
    [Theory]
    [InlineData(513)]
    [InlineData(1000)]
    public async Task Validate_TextTooLong_ShouldReturnMaximumLengthError(int length)
    {
        var dto = new CreatePostDto { text = new string('x', length) };
        var result = await createPostValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.text)
              .WithErrorMessage("Post can't be longer than 512 characters.");
    }
}