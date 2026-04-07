using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using questionmark.Application.Users.Contracts;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Users.DTO;
using questionmark.Application.Users.Validators;
using Xunit;

namespace questionmark.Tests.Unit.Validators;

public class UserCreateDtoValidatorTests
{
    private readonly IUser _userRepo = Substitute.For<IUser>();
    private readonly IHashService _hashService = Substitute.For<IHashService>();
    private readonly UserCreateDtoValidator userCreateValidator;
    private const string ValidAuth = "aabbccddeeff00112233445566778899aabbccddeeff00112233445566778899";
    public UserCreateDtoValidatorTests()
    {
        userCreateValidator = new UserCreateDtoValidator(_userRepo, _hashService);
        _hashService.HashAuthAsync(Arg.Any<string>()).Returns("hashed");
        _userRepo.IsUserRegistered(Arg.Any<string>()).Returns(false);
    }
    [Fact]
    public async Task Validate_ValidAuth_ShouldHaveNoErrors()
    {
        var dto = new UserCreateDto { auth = ValidAuth };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.auth);
    }
    [Fact]
    public async Task Validate_EmptyAuth_ShouldReturnAuthIsRequiredError()
    {
        var dto = new UserCreateDto { auth = string.Empty };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.auth)
              .WithErrorMessage("Auth is required.");
    }
    [Theory]
    [InlineData(63)]
    [InlineData(65)]
    [InlineData(1)]
    [InlineData(128)]
    public async Task Validate_InvalidAuthLength_ShouldReturnAuthIsNotInCorrectFormatError(int length)
    {
        var dto = new UserCreateDto { auth = new string('a', length) };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.auth)
              .WithErrorMessage("Auth is not in correct format.");
    }
    [Theory]
    [InlineData("gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg")]
    [InlineData("aabbccddeeff001122334455667788990011223344556677889900112233445Z")]
    [InlineData("aabbccddeeff00112233445566778899aabbccddeeff001122334455667788 9")]
    public async Task Validate_NonHexAuth_ShouldReturnAuthIsNotInCorrectFormatError(string auth)
    {
        var dto = new UserCreateDto { auth = auth };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.auth)
              .WithErrorMessage("Auth is not in correct format.");
    }
    [Theory]
    [InlineData("AABBCCDDEEFF00112233445566778899AABBCCDDEEFF00112233445566778899")]
    [InlineData("aabbccddeeff00112233445566778899aabbccddeeff00112233445566778899")]
    [InlineData("0000000000000000000000000000000000000000000000000000000000000000")]
    public async Task Validate_ValidHexFormat_ShouldHaveNoErrors(string auth)
    {
        var dto = new UserCreateDto { auth = auth };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.auth);
    }
    [Fact]
    public async Task Validate_UserAlreadyRegistered_ShouldReturnUserIsAlreadyRegisteredError()
    {
        _userRepo.IsUserRegistered(Arg.Any<string>()).Returns(true);
        var dto = new UserCreateDto { auth = ValidAuth };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x)
              .WithErrorMessage("User is already registered.");
    }
    [Fact]
    public async Task Validate_UserNotRegistered_ShouldHaveNoErrors()
    {
        _userRepo.IsUserRegistered(Arg.Any<string>()).Returns(false);
        var dto = new UserCreateDto { auth = ValidAuth };
        var result = await userCreateValidator.TestValidateAsync(dto);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}