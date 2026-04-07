using FluentValidation.TestHelper;
using NSubstitute;
using questionmark.Application.Users.Contracts;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Users.DTO;
using questionmark.Application.Users.Validators;
using Xunit;

namespace questionmark.Tests.Unit.Validators;

public class UserLoginDtoValidatorTests
{
    private readonly IUser _userRepo = Substitute.For<IUser>();
    private readonly IHashService _hashService = Substitute.For<IHashService>();
    private readonly UserLoginDtoValidator userLoginValidator;
    private const string ValidAuth = "aabbccddeeff00112233445566778899aabbccddeeff00112233445566778899";
    public UserLoginDtoValidatorTests()
    {
        userLoginValidator = new UserLoginDtoValidator(_userRepo, _hashService);
        _hashService.HashAuthAsync(Arg.Any<string>()).Returns("hashed");
        _userRepo.IsLoginCorrect(Arg.Any<string>()).Returns(true);
    }
    [Fact]
    public async Task Validate_ValidAuthRegisteredUser_ShouldHaveNoErrors()
    {
        var dto = new UserLoginDto { auth = ValidAuth };
        var result = await userLoginValidator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.auth);
    }
    [Fact]
    public async Task Validate_EmptyAuth_ShouldReturnAuthIsRequiredError()
    {
        var dto = new UserLoginDto { auth = string.Empty };
        var result = await userLoginValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.auth)
              .WithErrorMessage("Auth is required.");
    }
    [Theory]
    [InlineData(63)]
    [InlineData(65)]
    public async Task Validate_InvalidAuthLength_ShouldReturnAuthIsNotInCorrectFormatError(int length)
    {
        var dto = new UserLoginDto { auth = new string('a', length) };
        var result = await userLoginValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.auth)
              .WithErrorMessage("Auth is not in correct format.");
    }
    [Fact]
    public async Task Validate_NonHexAuth_ShouldReturnAuthIsNotInCorrectFormatError()
    {
        var dto = new UserLoginDto { auth = new string('z', 64) };
        var result = await userLoginValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.auth)
              .WithErrorMessage("Auth is not in correct format.");
    }
    [Fact]
    public async Task Validate_UnregisteredUser_ShouldReturnUserIsNotRegisteredError()
    {
        _userRepo.IsLoginCorrect(Arg.Any<string>()).Returns(false);
        var dto = new UserLoginDto { auth = ValidAuth };
        var result = await userLoginValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x)
              .WithErrorMessage("User is not registered.");
    }
    [Fact]
    public async Task Validate_CorrectLogin_ShouldHaveNoErrors()
    {
        _userRepo.IsLoginCorrect(Arg.Any<string>()).Returns(true);
        var dto = new UserLoginDto { auth = ValidAuth };
        var result = await userLoginValidator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}