using BaraTests.Utils;

namespace BaraTests.UserTests
{
    public class UserTests : BaseTestFixture
    {
        [Fact]
        public async Task UpdateUserVerificationStatus_WithValidData_ShouldReturnSuccessfulResponse()
        {
            var verificationIdNumber = "12345678901";
            var dateOfBirth = "1990-01-01";
            var firstName = "John";
            var lastName = "Doe";
            var type = "BVN";

            var result = await userService.UpdateUserVerificationStatus(
                verificationIdNumber,
                dateOfBirth,
                true,
                firstName,
                lastName,
                type);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUserVerificationStatus_WithNonExistentUser_ShouldReturnFailure()
        {
            var verificationIdNumber = "99999999999";
            var dateOfBirth = "1990-01-01";
            var firstName = "John";
            var lastName = "Doe";
            var type = "BVN";

            var result = await userService.UpdateUserVerificationStatus(
                verificationIdNumber,
                dateOfBirth,
                true,
                firstName,
                lastName,
                type);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("User not found", result.Message);
        }

        [Fact]
        public async Task UpdateUserVerificationStatus_WithUnsuccessfulVerification_ShouldReturnFailure()
        {
            var verificationIdNumber = "12345678901";
            var dateOfBirth = "1990-01-01";
            var firstName = "John";
            var lastName = "Doe";
            var type = "BVN";

            var result = await userService.UpdateUserVerificationStatus(
                verificationIdNumber,
                dateOfBirth,
                false,
                firstName,
                lastName,
                type);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Validation failed", result.Message);
        }
    }
}
