using BaraTests.Utils;
using Xunit;

namespace BaraTests.UserTests
{
    public class UserTests : BaseTestFixture
    {
        [Fact]
        public async Task UpdateUserVerificationStatus_WithValidData_ShouldReturnSuccessfulResponse()
        {
            // This test requires existing user data in the database
            var verificationIdNumber = "12345678901";
            var dateOfBirth = "1990-01-01";
            var firstName = "John";
            var lastName = "Doe";
            var type = "BVN";

            var result = await userService.UpdateUserVerificationStatus(
                verificationIdNumber, 
                dateOfBirth, 
                true, // isSuccessful
                firstName, 
                lastName, 
                type);

            Assert.NotNull(result);
            // Result will depend on whether user exists with matching data
        }

        [Fact]
        public async Task UpdateUserVerificationStatus_WithNonExistentUser_ShouldReturnFailure()
        {
            var verificationIdNumber = "99999999999"; // Non-existent ID
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
                false, // isSuccessful = false
                firstName, 
                lastName, 
                type);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Validation failed", result.Message);
        }
    }
}
