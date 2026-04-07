using FluentAssertions;
using questionmark.Infrastructure.Services;
using questionmark.Infrastructure.Settings;
using System.Security.Claims;
using Xunit;

namespace questionmark.Tests.Unit.Services;

public class JwtServiceTests
{
    private readonly JwtService jwtService;
    private const string AccessKey = "yKFph9z7FNhWCVfn574kEh1KgpToBGsAzdqURkSOAfQ";
    private const string RefreshKey = "4s2oU3UWhTWrcdtu6Cfi5NGuQF5zF0GMyKeS4FamHqX";
    public JwtServiceTests()
    {
        jwtService = new JwtService(new JwtSettings
        {
            AccessTokenKey = AccessKey,
            RefreshTokenKey = RefreshKey,
            Issuer = "questionmark",
            Audience = "questionmark",
            AccessTokenTTL = 10000,
            RefreshTokenTTL = 7
        });
    }
    private List<Claim> TestClaims(string userId = "AABBCCDD") =>
        new() { new Claim(ClaimTypes.NameIdentifier, userId) };
    [Fact]
    public async Task GetEncryptedUserIdFromAccessToken_ValidToken_ShouldReturnUserId()
    {
        const string userId = "DEADBEEF";
        var token = jwtService.GenerateAccessToken(TestClaims(userId));
        var result = await jwtService.GetEncryptedUserIdFromAccessToken(token);
        result.Should().Be(userId);
    }
    [Fact]
    public async Task GetEncryptedUserIdFromAccessToken_InvalidToken_ShouldReturnEmptyString()
    {
        var result = await jwtService.GetEncryptedUserIdFromAccessToken("not.jwt");
        result.Should().BeEmpty();
    }
    [Fact]
    public async Task GetEncryptedUserIdFromAccessToken_ExpiredToken_ShouldReturnEmptyString()
    {
        var shortLivedJwtService = new JwtService(new JwtSettings
        {
            AccessTokenKey = AccessKey,
            RefreshTokenKey = RefreshKey,
            Issuer = "questionmark",
            Audience = "questionmark",
            AccessTokenTTL = 1,
            RefreshTokenTTL = 7
        });
        var token = shortLivedJwtService.GenerateAccessToken(TestClaims());
        await Task.Delay(50);
        var result = await jwtService.GetEncryptedUserIdFromAccessToken(token);
        result.Should().BeEmpty();
    }
    [Fact]
    public async Task GetEncryptedUserIdFromAccessToken_RefreshToken_ShouldReturnEmptyString()
    {
        var refreshToken = jwtService.GenerateRefreshToken(TestClaims());
        var result = await jwtService.GetEncryptedUserIdFromAccessToken(refreshToken);
        result.Should().BeEmpty();
    }
    [Fact]
    public async Task GetEncryptedUserIdFromRefreshToken_ValidToken_ShouldReturnUserId()
    {
        const string userId = "CAFEBABE";
        var token = jwtService.GenerateRefreshToken(TestClaims(userId));
        var result = await jwtService.GetEncryptedUserIdFromRefreshToken(token);
        result.Should().Be(userId);
    }
    [Fact]
    public async Task GetEncryptedUserIdFromRefreshToken_InvalidToken_ShouldReturnEmptyString()
    {
        var result = await jwtService.GetEncryptedUserIdFromRefreshToken("not.jwt");
        result.Should().BeEmpty();
    }
    [Fact]
    public async Task GetEncryptedUserIdFromRefreshToken_AccessToken_ShouldReturnEmptyString()
    {
        var accessToken = jwtService.GenerateAccessToken(TestClaims());
        var result = await jwtService.GetEncryptedUserIdFromRefreshToken(accessToken);
        result.Should().BeEmpty();
    }
    [Fact]
    public async Task ValidateRefreshToken_ValidToken_ShouldReturnTrue()
    {
        var token = jwtService.GenerateRefreshToken(TestClaims());
        var result = await jwtService.ValidateRefreshToken(token);
        result.Should().BeTrue();
    }
    [Fact]
    public async Task ValidateRefreshToken_InvalidToken_ShouldReturnFalse()
    {
        var result = await jwtService.ValidateRefreshToken("not.jwt");
        result.Should().BeFalse();
    }
    [Fact]
    public async Task ValidateRefreshToken_AccessToken_ShouldReturnFalse()
    {
        var accessToken = jwtService.GenerateAccessToken(TestClaims());
        var result = await jwtService.ValidateRefreshToken(accessToken);
        result.Should().BeFalse();
    }
    [Fact]
    public void GetRefreshTokenTTL_ShouldReturnConfiguredValue()
    {
        var ttl = jwtService.getRefreshTokenTTL();
        ttl.Should().Be(7);
    }
}