using FluentAssertions;
using questionmark.Infrastructure.Services;
using questionmark.Infrastructure.Settings;
using Xunit;

namespace questionmark.Tests.Unit.Services;

public class EncryptionServiceTests
{
    private readonly EncryptionService encryptionService;
    public EncryptionServiceTests()
    {
        var settings = new EncryptionSettings
        {
            AESKey_user = "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f"
        };
        encryptionService = new EncryptionService(settings);
    }
    [Theory]
    [InlineData(1u)]
    [InlineData(42u)]
    [InlineData(1000u)]
    [InlineData(uint.MaxValue)]
    public void EncryptUserID_WhenEncryptedAndDecrypted_ShouldReturnOriginalId(uint originalId)
    {
        var encrypted = encryptionService.EncryptUserID(originalId);
        var decrypted = encryptionService.DecryptUserID(encrypted.cipher, encrypted.nonce, encrypted.tag);
        decrypted.Should().Be(originalId);
    }
    [Fact]
    public void EncryptUserID_ShouldReturnNonEmptyCipher()
    {
        var result = encryptionService.EncryptUserID(1u);
        result.cipher.Should().NotBeEmpty();
    }
    [Fact]
    public void EncryptUserID_ShouldReturnNonceOfLength12()
    {
        var result = encryptionService.EncryptUserID(1u);
        result.nonce.Should().HaveCount(12);
    }
    [Fact]
    public void EncryptUserID_ShouldReturnTagOfLength16()
    {
        var result = encryptionService.EncryptUserID(1u);
        result.tag.Should().HaveCount(16);
    }
    [Fact]
    public void EncryptUserID_MultipleEncryptions_ShouldUseDifferentNonces()
    {
        var result1 = encryptionService.EncryptUserID(99u);
        var result2 = encryptionService.EncryptUserID(99u);
        result1.nonce.Should().NotEqual(result2.nonce);
    }
    [Fact]
    public void EncryptUserID_MultipleEncryptions_ShouldProduceDifferentCiphers()
    {
        var firstResult = encryptionService.EncryptUserID(99u);
        var secondResult = encryptionService.EncryptUserID(99u);
        firstResult.cipher.Should().NotEqual(secondResult.cipher);
    }
    [Fact]
    public void DecryptUserID_WithInvalidTag_ShouldThrowException()
    {
        var encrypted = encryptionService.EncryptUserID(1u);
        var invalidTag = new byte[16];
        Array.Fill(invalidTag, (byte)0xFF);
        Action act = () => encryptionService.DecryptUserID(encrypted.cipher, encrypted.nonce, invalidTag);
        act.Should().Throw<Exception>();
    }
    [Fact]
    public void DecryptUserID_WithInvalidNonce_ShouldThrowException()
    {
        var encrypted = encryptionService.EncryptUserID(1u);
        var invalidNonce = new byte[12];
        Array.Fill(invalidNonce, (byte)0xFF);
        Action act = () => encryptionService.DecryptUserID(encrypted.cipher, invalidNonce, encrypted.tag);
        act.Should().Throw<Exception>();
    }
}
