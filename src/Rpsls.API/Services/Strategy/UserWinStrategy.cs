using Rpsls.API.Dto;
using Rpsls.API.Entities;
using Rpsls.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Rpsls.API.Services.Strategy
{
    public class UserWinStrategy : IGameResultStrategy
    {
        private readonly ApplicationDbContext _context;

        public UserWinStrategy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameRuleDto> GetResult(int userChoiceId, Choice computerChoice)
        {
            var gameRule = await _context.GameRules
                .FirstOrDefaultAsync(gr => gr.WinnerId == userChoiceId && gr.LoserId == computerChoice.Id);

            if (gameRule != null) // User wins
            {
                return new GameRuleDto 
                {
                    Player = userChoiceId,
                    Computer = computerChoice.Id,
                    Results = "You win!",
                    Description = gameRule.Description
                };
            }

            return null; // Not a user win
        }
    }
}
