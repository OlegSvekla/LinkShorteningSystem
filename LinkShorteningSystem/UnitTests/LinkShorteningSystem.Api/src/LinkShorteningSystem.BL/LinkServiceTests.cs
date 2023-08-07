using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.LinkShorteningSystem.Api.src.LinkShorteningSystem.BL
{
    [TestClass]
    public class LinkServiceTests
    {
        private Mock<IRepository<Link>> _linkRepositoryMock;
        private Mock<ILogger<LinkService>> _loggerMock;
        private LinkService _linkService;

        [TestInitialize]
        public void Setup()
        {
            _linkRepositoryMock = new Mock<IRepository<Link>>();
            _loggerMock = new Mock<ILogger<LinkService>>();
            _linkService = new LinkService(_linkRepositoryMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task ShortenLinkAsync_ValidInput_ShouldGenerateShortenedLinkAndSave()
        {
            // Arrange
            var baseClientLink = "http://example.com";
            var originalLink = "http://original-link.com";
            var generatedShortenedLink = $"{baseClientLink}/abcdefg";

            _linkRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Link>()))
                .Callback<Link>(link =>
                {
                    link.ShortenedLink = generatedShortenedLink;
                });

            // Act
            var result = await _linkService.ShortenLinkAsync(baseClientLink, originalLink);

            // Assert
            Assert.AreEqual(generatedShortenedLink, result);
            _linkRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Link>()), Times.Once);
        }

        [TestMethod]
        public async Task GetShortenedLinkAsync_ExistingLinkId_ShouldRetrieveShortenedLink()
        {
            // Arrange
            var linkId = 1;
            var existingLink = new Link { Id = linkId, ShortenedLink = "short-link" };
            _linkRepositoryMock.Setup(r => r.GetByIdAsync(linkId))
                .ReturnsAsync(existingLink);

            // Act
            var result = await _linkService.GetShortenedLinkAsync(linkId);

            // Assert
            Assert.AreEqual(existingLink.ShortenedLink, result);
        }

        [TestMethod]
        public async Task GetOriginalLinkAsync_ExistingShortenedLink_ShouldRetrieveOriginalLink()
        {
            // Arrange
            var shortenedLink = "short-link";
            var existingLink = new Link { ShortenedLink = shortenedLink, OriginalLink = "original-link" };
            _linkRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Link, bool>>>()))
                .ReturnsAsync(existingLink);

            // Act
            var result = await _linkService.GetOriginalLinkAsync(shortenedLink);

            // Assert
            Assert.AreEqual(existingLink.OriginalLink, result);
        }

        // Add more test methods for other scenarios
    }
}
