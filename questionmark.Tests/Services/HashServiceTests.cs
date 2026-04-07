using FluentAssertions;
using questionmark.Infrastructure.Services;
using questionmark.Infrastructure.Settings;
using Xunit;

namespace questionmark.Tests.Unit.Services;

public class HashServiceTests
{
    private readonly HashService hashService;
    public HashServiceTests()
    {
        var settings = new HashSettings
        {
            authHashKey = "ywXMPY2obN1EVyPFYUq6g396xsqgWVbo",
            cipherHashKey = "tpacMDvZ9Irx0wYkU1DqgKRWvo8wg6vj"
        };
        hashService = new HashService(settings);
    }
    [Fact]
    public async Task HashAuthAsync_SameInput_ShouldReturnSameHash()
    {
        var auth = new string('a', 64);
        var hash1 = await hashService.HashAuthAsync(auth);
        var hash2 = await hashService.HashAuthAsync(auth);
        hash1.Should().Be(hash2);
    }
    [Fact]
    public async Task HashAuthAsync_DifferentInputs_ShouldReturnDifferentHashes()
    {
        var auth1 = new string('a', 64);
        var auth2 = new string('b', 64);
        var hash1 = await hashService.HashAuthAsync(auth1);
        var hash2 = await hashService.HashAuthAsync(auth2);
        hash1.Should().NotBe(hash2);
    }
    [Fact]
    public async Task HashAuthAsync_ShouldReturn128CharacterHexString()
    {
        var auth = new string('f', 64);
        var hash = await hashService.HashAuthAsync(auth);
        hash.Should().HaveLength(128);
        hash.Should().MatchRegex("^[0-9A-F]+$");
    }
    [Fact]
    public async Task HashCipherAsync_SameInput_ShouldReturnSameHash()
    {
        var cipher = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var hash1 = await hashService.HashCipherAsync(cipher);
        var hash2 = await hashService.HashCipherAsync(cipher);
        hash1.Should().Equal(hash2);
    }
    [Fact]
    public async Task HashCipherAsync_DifferentInputs_ShouldReturnDifferentHashes()
    {
        var cipher1 = new byte[] { 1, 2, 3, 4 };
        var cipher2 = new byte[] { 5, 6, 7, 8 };
        var hash1 = await hashService.HashCipherAsync(cipher1);
        var hash2 = await hashService.HashCipherAsync(cipher2);
        hash1.Should().NotEqual(hash2);
    }
    [Fact]
    public async Task HashCipherAsync_ShouldReturn32Bytes()
    {
        var cipher = new byte[] { 0xAB, 0xCD, 0xEF };
        var hash = await hashService.HashCipherAsync(cipher);
        hash.Should().HaveCount(32);
    }
}