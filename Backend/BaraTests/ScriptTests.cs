using BaraTests.Utils;
using Microsoft.AspNetCore.Http;
using ScriptModule.DTOs;
using ScriptModule.Enums;

namespace BaraTests
{
    public class ScriptTests : BaseTestFixture
    {
        [Fact]
        public async Task AddScript_WithValidData_ShouldReturnSuccessfulResponse()
        {
            var writerId = Guid.NewGuid();
            var scriptDetail = CreateValidScriptDetailDTO();

            var result = await scriptService.AddScript(scriptDetail, writerId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddScript_WithNonExistentWriter_ShouldReturnFailedResponse()
        {
            var nonExistentWriterId = Guid.NewGuid();
            var scriptDetail = CreateValidScriptDetailDTO();

            var result = await scriptService.AddScript(scriptDetail, nonExistentWriterId);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("does not exist", result.Message);
        }

        [Fact]
        public async Task GetScripts_ShouldReturnPaginatedResults()
        {
            var result = await scriptService.GetScripts(1, 10);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetScriptById_WithNonExistentId_ShouldReturnNotFound()
        {
            var nonExistentId = Guid.NewGuid();

            var result = await scriptService.GetScriptById(nonExistentId, null);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetScriptsByWriterId_WithNonExistentWriter_ShouldReturnEmptyResult()
        {
            var nonExistentWriterId = Guid.NewGuid();

            var result = await scriptService.GetScriptsByWriterId(nonExistentWriterId, 1, 10);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task DeleteScript_WithNonExistentScript_ShouldReturnFailedResponse()
        {
            var nonExistentScriptId = Guid.NewGuid();
            var writerId = Guid.NewGuid();

            var result = await scriptService.DeleteScript(nonExistentScriptId, writerId);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid script Id", result.Message);
        }

        [Fact]
        public async Task DownloadScript_WithNonExistentScript_ShouldReturnNotFound()
        {
            var nonExistentScriptId = Guid.NewGuid();

            var result = await scriptService.DownloadScript(nonExistentScriptId);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
        }

        private PostScriptDetailDTO CreateValidScriptDetailDTO()
        {
            // Create a simple test file
            var fileContent = "Test script content"u8.ToArray();
            var stream = new MemoryStream(fileContent);
            var formFile = new FormFile(stream, 0, fileContent.Length, "Script", "test-script.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            return new PostScriptDetailDTO
            {
                Title = "Test Script",
                Genre = "Drama",
                Logline = "A compelling story about testing",
                Synopsis = "This is a detailed synopsis of the test script.",
                Price = 500.00m,
                IsScriptRegistered = true,
                RegistrationBody = "WGA",
                File = formFile,
                Image = "test-image.jpg",
                CopyrightNumber = "CR123456",
                OwnershipRights = IPDealType.WriterRetainsRights,
                ProofUrl = "https://example.com/proof.pdf"
            };
        }
    }
}
