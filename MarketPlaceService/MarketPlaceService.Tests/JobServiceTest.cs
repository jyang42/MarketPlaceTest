using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlaceService.DataAccess.Models;
using MarketPlaceService.DataAccess;
using MarketPlaceService.Services.Models;
using MarketPlaceService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceService.Tests
{
    public class JobServiceTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly JobService _jobService;

        public JobServiceTests()
        {
            // Configure DbContext with In-Memory Database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "JobTestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated(); // Ensure database schema is created

            // Mock UserManager
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object);

            // Initialize JobService with the mocked UserManager and real DbContext
            _jobService = new JobService(_dbContext, _userManagerMock.Object);
        }

        [Fact]
        public async Task CreateJob_ReturnsJobDTO_WhenSuccessful()
        {
            // Arrange
            var poster = new User
            {
                Id = "1",
                FirstName = "John",
                LastName = "Doe",
                Email = "test@email.com"
            };

            // Mock the behavior of UserManager to return the user
            _userManagerMock.Setup(um => um.Users).Returns(new List<User> { poster }.AsQueryable());

            var createJobDTO = new CreateJobDTO
            {
                PosterId = "1",
                Description = "Test Job Description",
                Requirements = "Test Requirements",
                ExpirationDate = DateTime.Now.AddDays(10)
            };

            // Act
            var result = await _jobService.CreateJob(createJobDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Job Description", result.Description);
            Assert.Equal("John Doe", result.PosterName);
        }


        [Fact]
        public async Task CreateJob_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var createJobDTO = new CreateJobDTO
            {
                PosterId = "999", // Non-existent user
                Description = "Test Job Description",
                Requirements = "Test Requirements",
                ExpirationDate = DateTime.Now.AddDays(10)
            };

            _userManagerMock.Setup(um => um.Users).Returns(Enumerable.Empty<User>().AsQueryable());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _jobService.CreateJob(createJobDTO));
            Assert.Equal("User cannot be null", exception.Message);
        }
}