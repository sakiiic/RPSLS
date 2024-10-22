using Rpsls.API.Dto;
using Rpsls.API.Entities;

namespace Rpsls.API.Services.Strategy
{
    public class GameResultContext
    {
        private readonly List<IGameResultStrategy> _strategies;

        public GameResultContext(List<IGameResultStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<GameRuleDto> ExecuteStrategyAsync(int userChoiceId, Choice computerChoice)
        {
            foreach (var strategy in _strategies)
            {
                var result = await strategy.GetResult(userChoiceId, computerChoice);
                if (result != null)
                {
                    return result;
                }
            }

            return new GameRuleDto
            {
                Player = userChoiceId,
                Computer = computerChoice.Id,
                Results = "No valid game result."
            };
        }
    }
}
