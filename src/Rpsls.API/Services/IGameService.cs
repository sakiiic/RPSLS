using Rpsls.API.Dto;

namespace Rpsls.API.Services
{
    public interface IGameService
    {
        Task<List<ChoiceDto>> GetChoicesAsync();
        Task<ChoiceDto> GetChoice();
        Task<GameRuleDto> PlayGameAsync(int userChoiceId);
    }
}
