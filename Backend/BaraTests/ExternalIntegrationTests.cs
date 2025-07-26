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
            await TestYouVerifyKYC("BVN", "11111111111", "omonuwa");
            await TestYouVerifyKYC("NIN", "00000000000", "omonuwa");
        }

        internal async Task TestYouVerifyKYC(string type, string id, string lastName)
        {
            var kycDto = new YouVerify_KYC_DTO
            (
                id,
                type,
                lastName,
                true
            );

            var response = await youVerify.VerifyDocumentAsync(kycDto);
            Assert.NotNull(response);
            Assert.Equal("success", response.Message);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
        }

        #endregion
    }
}
