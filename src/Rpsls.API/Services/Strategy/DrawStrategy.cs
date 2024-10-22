using Rpsls.API.Dto;
using Rpsls.API.Entities;

namespace Rpsls.API.Services.Strategy
{
    public class DrawStrategy : IGameResultStrategy
    {
        public DrawStrategy() 
        {}

        public Task<GameRuleDto> GetResult(int userChoiceId, Choice computerChoice)
        {
            if (computerChoice.Id == userChoiceId) // It's a draw
            { 
                return Task.FromResult(new GameRuleDto 
                {
                    Player = userChoiceId,
                    Computer = computerChoice.Id,
                    Results = "It's a draw!",
                    Description = "You both chose " + computerChoice.Name
                });
            }

            //Not a draw
            return null;
        }
    }
}
