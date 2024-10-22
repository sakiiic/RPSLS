using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Rpsls.API.Dto;
using Rpsls.API.Entities;
using Rpsls.API.Infrastructure.Data;
using Rpsls.API.Services;
using Rpsls.API.Services.RandomGeneratorService;

namespace Rpsls.Tests
{
    public class GameServiceTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GameService>> _mockLogger;
        private Mock<IRandomGenerator> _mockRandomGenerator;
        private GameService _gameService;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GameService>>();
            _mockRandomGenerator = new Mock<IRandomGenerator>();

            _gameService = new GameService(_mockContext.Object, _mockMapper.Object, _mockLogger.Object, _mockRandomGenerator.Object);

            SetupChoicesDbSet();
            SetupGameRulesDbSet();
        }

        [Test]
        public async Task PlayGameAsync_ShouldReturnDraw_WhenUserAndComputerChooseSame()
        {
            int userChoiceId = 1; // User choses Rock

            _mockRandomGenerator.Setup(r => r.Next(0, 5)).Returns(0); // Index for Rock

            var result = await _gameService.PlayGameAsync(userChoiceId);

            Assert.IsNotNull(result);
            Assert.That(result.Results, Is.EqualTo("It's a draw!"));
            Assert.That(result.Description, Is.EqualTo("You both chose Rock"));
        }

        [Test]
        public async Task PlayGameAsync_ShouldReturnUserWin_WhenUserBeatsComputer()
        {
            int userChoiceId = 3; // User choses Scissors

            _mockRandomGenerator.Setup(r => r.Next(0, 5)).Returns(1); // Index for Paper

            var result = await _gameService.PlayGameAsync(userChoiceId);

            Assert.IsNotNull(result);
            Assert.That(result.Results, Is.EqualTo("You win!"));
            Assert.That(result.Description, Is.EqualTo("Scissors cuts Paper"));
        }

        [Test]
        public async Task PlayGameAsync_ShouldReturnComputerWin_WhenComputerBeatsUser()
        {
            int userChoiceId = 5; // User choses Spock
            _mockRandomGenerator.Setup(r => r.Next(0, 5)).Returns(1); // Index for Paper

            var result1 = await _gameService.PlayGameAsync(userChoiceId);

            Assert.IsNotNull(result1);
            Assert.That(result1.Results, Is.EqualTo("You lose!"));
            Assert.That(result1.Description, Is.EqualTo("Paper disproves Spock"));
        }

        [Test]
        public async Task PlayGameAsync_ShouldThrowArgumentException_WhenInvalidUserChoice()
        {
            int invalidUserChoiceId = 6;

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _gameService.PlayGameAsync(invalidUserChoiceId));
            Assert.That(ex.Message, Is.EqualTo("Invalid choice ID."));
        }

        [Test]
        public async Task PlayGameAsync_ShouldThrowArgumentException_WhenNegativeUserChoice()
        {
            int negativeUserChoiceId = -1;

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _gameService.PlayGameAsync(negativeUserChoiceId));
            Assert.That(ex.Message, Is.EqualTo("Invalid choice ID."));
        }

        [Test]
        public async Task GetChoicesAsync_ShouldReturnListOfChoices()
        {
            var choiceEntities = new List<Choice>
            {
                new Choice { Id = 1, Name = "Rock" },
                new Choice { Id = 2, Name = "Paper" },
                new Choice { Id = 3, Name = "Scissors" },
                new Choice { Id = 4, Name = "Lizard" },
                new Choice { Id = 5, Name = "Spock" }
            };

            _mockContext.Setup(c => c.Choices).Returns(choiceEntities.AsQueryable().BuildMockDbSet().Object);

            var choiceDtos = choiceEntities.Select(c => new ChoiceDto { Id = c.Id, Name = c.Name }).ToList();
            _mockMapper.Setup(m => m.Map<List<ChoiceDto>>(It.IsAny<List<Choice>>())).Returns(choiceDtos);

            var result = await _gameService.GetChoicesAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result[0].Name, Is.EqualTo("Rock"));
            Assert.That(result[4].Name, Is.EqualTo("Spock"));
        }

        [Test]
        public async Task GetChoice_ShouldReturnRandomChoice()
        {
            var choices = new List<Choice>
            {
                new Choice { Id = 1, Name = "Rock" },
                new Choice { Id = 2, Name = "Paper" },
                new Choice { Id = 3, Name = "Scissors" }
            };

            // Setup Choices
            _mockContext.Setup(c => c.Choices).Returns(choices.AsQueryable().BuildMockDbSet().Object);

            // Map Setup
            var choiceDtos = choices.Select(c => new ChoiceDto { Id = c.Id, Name = c.Name }).ToList();
            _mockMapper.Setup(m => m.Map<List<ChoiceDto>>(It.IsAny<List<Choice>>())).Returns(choiceDtos);

            // Setup Random Generator to return index 1 (which is Paper)
            _mockRandomGenerator.Setup(r => r.Next(0, choices.Count)).Returns(1);

            var result = await _gameService.GetChoice();

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Paper"));
        }

        private void SetupChoicesDbSet()
        {
            var choices = new List<Choice>
            {
                new Choice { Id = 1, Name = "Rock" },
                new Choice { Id = 2, Name = "Paper" },
                new Choice { Id = 3, Name = "Scissors" },
                new Choice { Id = 4, Name = "Lizard" },
                new Choice { Id = 5, Name = "Spock" }
            };

            _mockContext.Setup(c => c.Choices).Returns(choices.AsQueryable().BuildMockDbSet().Object);
        }

        private void SetupGameRulesDbSet()
        {
            var gameRules = new List<GameRule>
            {
                new GameRule { Id = 1, WinnerId = 1, LoserId = 3, Description = "Rock crushes Scissors" },
                new GameRule { Id = 2, WinnerId = 3, LoserId = 2, Description = "Scissors cuts Paper" },
                new GameRule { Id = 3, WinnerId = 2, LoserId = 1, Description = "Paper covers Rock" },
                new GameRule { Id = 4, WinnerId = 2, LoserId = 5, Description = "Paper disproves Spock" },
                new GameRule { Id = 5, WinnerId = 3, LoserId = 2, Description = "Scissors cuts Paper" },
                new GameRule { Id = 6, WinnerId = 3, LoserId = 4, Description = "Scissors decapitates Lizard" },
                new GameRule { Id = 7, WinnerId = 4, LoserId = 2, Description = "Lizard eats Paper" },
                new GameRule { Id = 8, WinnerId = 4, LoserId = 5, Description = "Lizard poisons Spock" },
                new GameRule { Id = 9, WinnerId = 5, LoserId = 3, Description = "Spock smashes Scissors" },
                new GameRule { Id = 10, WinnerId = 5, LoserId = 1, Description = "Spock vaporizes Rock" }
            };

            _mockContext.Setup(m => m.GameRules).Returns(gameRules.AsQueryable().BuildMockDbSet().Object);
        }
    }
}