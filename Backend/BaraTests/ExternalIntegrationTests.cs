using BaraTests.Utils;
using Services.YouVerifyIntegration;

namespace BaraTests
{
    public class ExternalIntegrationTests : BaseTestFixture
    {
        #region You Verify

        [Fact]
        public async Task TestYouVerifyKYCUseCase()
        {
            await TestYouVerifyKYC("BVN", "11111111111");
            await TestYouVerifyKYC("NIN", "00000000000");
        }

        internal async Task TestYouVerifyKYC(string type, string id)
        {
            var kycDto = new YouVerifyKycDto
            {
                Id = id,
                Type = type,
                IsSubjectConsent = true
            };

            var response = await youVerify.VerifyDocumentAsync(kycDto);
            Assert.NotNull(response);
            Assert.Equal("success", response.Message);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
        }

        #endregion
    }
}
