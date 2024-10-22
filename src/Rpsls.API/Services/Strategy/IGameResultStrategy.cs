using Rpsls.API.Dto;
using Rpsls.API.Entities;

namespace Rpsls.API.Services.Strategy
{
    public interface IGameResultStrategy
    {
        Task<GameRuleDto> GetResult(int userChoiceId, Choice computerChoice);
    }
}
