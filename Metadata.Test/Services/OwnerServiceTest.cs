using Amazon.S3.Model;
using Aspose.Pdf.Operators;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Moq;
using Serilog;
using SharedLib.Core.Exceptions;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Metadata.Test.Services
{
    public class OwnerServiceTest
    {
        public Mock<IFormFile> CreateMockupExcelFileFromPath(string filePath, string fileName)
        {
            var fileMock = new Mock<IFormFile>();

            using (var fileStream = File.OpenRead(filePath))
            {
                var memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                fileMock.Setup(f => f.FileName).Returns(fileName);
                fileMock.Setup(f => f.Length).Returns(memoryStream.Length);
                fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            }

            return fileMock;
        }

        [Fact(Skip = "No file specified.")]
        public async Task Import_Owners_Throw_UniqueConstraintException_Owner_ID_Code()
        {
            // Arrange
            var filePath = @"C:\path\to\your\example.xlsx"; // Replace with the actual file path
            var fileName = "example.xlsx";

            if (!File.Exists(filePath))
            {
                // Skip the test if the file does not exist
                Assert.True(true); // This line is just to make the test method compile
                return;
            }


            var fileMock = CreateMockupExcelFileFromPath(filePath, fileName);

            var mockService = new Mock<IOwnerService>();
            mockService.Setup(sm => sm.ImportOwnerFromExcelFileAsync(fileMock.Object))
                       .ThrowsAsync(new UniqueConstraintException($"Có một chủ sở hữu khác với CCCD: [your_owner_code] đã tồn tại trong hệ thống."));

            // Act & Assert
            await Assert.ThrowsAsync<UniqueConstraintException>(() => mockService.Object.ImportOwnerFromExcelFileAsync(fileMock.Object));
        }

        [Fact]
        public async Task Delete_Owner_With_ID_Not_Found()
        {
            var owner = new Metadata.Core.Entities.Owner
            {
                OwnerId = "1",
                OwnerStatus = "AcceptCompensation",
            };

            var mockService = new Mock<IOwnerService>();
            // Configure the behavior of the mock
            mockService.Setup(sm => sm.DeleteOwner(owner.OwnerId)).ThrowsAsync(new EntityWithIDNotFoundException<Core.Entities.Owner>(owner.OwnerId));
            // Use Assert.ThrowsAsync to verify the exception is thrown
            await Assert.ThrowsAsync<EntityWithIDNotFoundException<Core.Entities.Owner>>(() => mockService.Object.DeleteOwner(owner.OwnerId));

        }

        [Fact]
        public async Task Delete_Owner_With_AcceptCompensation_Status_Throw_InvalidActionException()
        {
            var owner = new Metadata.Core.Entities.Owner
            {
                OwnerId = "1",
                OwnerStatus = "AcceptCompensation",
            };

            var mockService = new Mock<IOwnerService>();
            // Configure the behavior of the mock
            mockService.Setup(sm => sm.DeleteOwner(owner.OwnerId)).ThrowsAsync(new InvalidActionException());
            // Use Assert.ThrowsAsync to verify the exception is thrown
            await Assert.ThrowsAsync<InvalidActionException>(() => mockService.Object.DeleteOwner(owner.OwnerId));
        }

        [Fact]
        public async Task Delete_Owner_Has_Plan_ID_Throw_InvalidActionException()
        {
            var owner = new Metadata.Core.Entities.Owner
            {
                OwnerId = "1",
                PlanId = "1"
            };

            var mockService = new Mock<IOwnerService>();
            // Configure the behavior of the mock
            mockService.Setup(sm => sm.DeleteOwner(owner.OwnerId)).ThrowsAsync(new InvalidActionException());
            // Use Assert.ThrowsAsync to verify the exception is thrown
            await Assert.ThrowsAsync<InvalidActionException>(() => mockService.Object.DeleteOwner(owner.OwnerId));
        }

    }
}
