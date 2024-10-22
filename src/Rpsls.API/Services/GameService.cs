using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rpsls.API.Dto;
using Rpsls.API.Infrastructure.Data;
using Rpsls.API.Services.RandomGeneratorService;
using Rpsls.API.Services.Strategy;

namespace Rpsls.API.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GameService> _logger;
        private readonly IRandomGenerator _randomGenerator;

        public GameService(ApplicationDbContext context, IMapper mapper, ILogger<GameService> logger, IRandomGenerator randomGenerator)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _randomGenerator = randomGenerator;
        }

        public async Task<ChoiceDto> GetChoice()
        {
            _logger.LogInformation("Get random generated choice method executing.");

            var choices = await GetChoicesAsync();

            var computerChoice = choices[_randomGenerator.Next(0, choices.Count)];

            return computerChoice;
        }

        public async Task<List<ChoiceDto>> GetChoicesAsync()
        {
            _logger.LogInformation("Get choices from database.");

            var choices = await _context.Choices.ToListAsync();
            return _mapper.Map<List<ChoiceDto>>(choices);
        }

        public async Task<GameRuleDto> PlayGameAsync(int userChoiceId)
        {
            _logger.LogInformation("PlayGameAsync started with user choice ID: {UserChoiceId}", userChoiceId);

            var allChoices = await _context.Choices.ToListAsync();
            if (!allChoices.Any(x => x.ChoiceExists(userChoiceId)))
            {
                _logger.LogWarning("Invalid choice ID: {UserChoiceId}", userChoiceId);
                throw new ArgumentException("Invalid choice ID.");
            }

            var computerChoice = allChoices[_randomGenerator.Next(0, allChoices.Count)];
            _logger.LogInformation("Computer choice: {ComputerChoiceId} - {ComputerChoiceName}", computerChoice.Id, computerChoice.Name);

            var strategies = new List<IGameResultStrategy>
            {
                new UserWinStrategy(_context),
                new ComputerWinStrategy(_context),
                new DrawStrategy()
            };

            _logger.LogInformation("Executing game strategy...");
            var context = new GameResultContext(strategies);
            var result = await context.ExecuteStrategyAsync(userChoiceId, computerChoice);

            _logger.LogInformation("Game result: {resultDescription} - {Results}", result.Description, result.Results);

            return result;
        }
    }
}