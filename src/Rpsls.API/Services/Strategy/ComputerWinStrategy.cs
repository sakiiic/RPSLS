using Rpsls.API.Dto;
using Rpsls.API.Entities;
using Rpsls.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Rpsls.API.Services.Strategy
{
    public class ComputerWinStrategy : IGameResultStrategy
    {
        private readonly ApplicationDbContext _context;

        public ComputerWinStrategy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameRuleDto> GetResult(int userChoiceId, Choice computerChoice)
        {
            var gameRule = await _context.GameRules
                .FirstOrDefaultAsync(gr => gr.WinnerId == computerChoice.Id && gr.LoserId == userChoiceId);

            if (gameRule != null) // Computer wins
            {
                return new GameRuleDto 
                {
                    Player = userChoiceId,
                    Computer = computerChoice.Id,
                    Results = "You lose!",
                    Description = gameRule.Description
                };
            }

            return null; // Not a computer win
        }
    }
}
