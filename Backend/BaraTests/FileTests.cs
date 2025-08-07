using BaraTests.Utils;
using Microsoft.AspNetCore.Http;
using UserModule.DTOs.DocumentDTOs;
using UserModule.Enums;

namespace BaraTests
{
    public class FileTests : BaseTestFixture
    {
        [Fact]
        public async Task ProcessDocumentForUpload_WithValidPdfDocument_ShouldReturnSuccessfulResponse()
        {
            var userDirectoryName = "TestUser_123";
            var documentDetail = CreateValidDocumentDetailDTO();

            var result = await fileService.ProcessDocumentForUpload(userDirectoryName, documentDetail);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);
            Assert.Contains("uploaded successfully", result.Message);
        }

        [Fact]
        public async Task ProcessDocumentForUpload_WithInvalidFileExtension_ShouldReturnInvalidFileTypeError()
        {
            var userDirectoryName = "TestUser_123";
            var documentDetail = CreateInvalidDocumentDetailDTO();

            var result = await fileService.ProcessDocumentForUpload(userDirectoryName, documentDetail);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(415, result.StatusCode);
            Assert.Contains("not supported", result.Message);
        }

        [Fact]
        public async Task ProcessDocumentForUpload_WithBVNDocument_ShouldCreateDocumentWithCorrectType()
        {
            var userDirectoryName = "TestUser_BVN";
            var documentDetail = CreateDocumentDetailDTO(DocumentType.BVN, "12345678901");

            var result = await fileService.ProcessDocumentForUpload(userDirectoryName, documentDetail);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async Task ProcessDocumentForUpload_WithNINDocument_ShouldCreateDocumentWithCorrectType()
        {
            var userDirectoryName = "TestUser_NIN";
            var documentDetail = CreateDocumentDetailDTO(DocumentType.NIN, "12345678901");

            var result = await fileService.ProcessDocumentForUpload(userDirectoryName, documentDetail);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async Task ProcessDocumentForUpload_WithInternationalPassportDocument_ShouldCreateDocumentWithCorrectType()
        {
            var userDirectoryName = "TestUser_Passport";
            var documentDetail = CreateDocumentDetailDTO(DocumentType.International_Passport, "A12345678");

            var result = await fileService.ProcessDocumentForUpload(userDirectoryName, documentDetail);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async Task ProcessDocumentForUpload_WithDriversLicenseDocument_ShouldCreateDocumentWithCorrectType()
        {
            var userDirectoryName = "TestUser_License";
            var documentDetail = CreateDocumentDetailDTO(DocumentType.Drivers_License, "DL123456789");

            var result = await fileService.ProcessDocumentForUpload(userDirectoryName, documentDetail);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        private PostDocumentDetailDTO CreateValidDocumentDetailDTO()
        {
            return CreateDocumentDetailDTO(DocumentType.BVN, "12345678901");
        }

        private PostDocumentDetailDTO CreateDocumentDetailDTO(DocumentType type, string verificationNumber)
        {
            var fileContent = "Test document content"u8.ToArray();
            var stream = new MemoryStream(fileContent);
            var formFile = new FormFile(stream, 0, fileContent.Length, "Document", "test-document.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            return new PostDocumentDetailDTO
            {
                Document = formFile,
                Type = type,
                VerificationNumber = verificationNumber
            };
        }

        private PostDocumentDetailDTO CreateInvalidDocumentDetailDTO()
        {
            var fileContent = "Test document content"u8.ToArray();
            var stream = new MemoryStream(fileContent);
            var formFile = new FormFile(stream, 0, fileContent.Length, "Document", "document.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            return new PostDocumentDetailDTO
            {
                Document = formFile,
                Type = DocumentType.BVN,
                VerificationNumber = "12345678901"
            };
        }
    }
}
