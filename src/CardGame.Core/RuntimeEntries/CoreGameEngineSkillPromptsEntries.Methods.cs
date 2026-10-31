#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillPromptsEntries
{
    internal int CountCardsUsedByCurrentPlayerThisTurn(int ownerSeat) => _resolve().CountCardsUsedByCurrentPlayerThisTurn(ownerSeat);
    internal int CountCardsUsedOrRespondedByPlayerThisTurn(int ownerSeat) => _resolve().CountCardsUsedOrRespondedByPlayerThisTurn(ownerSeat);
    internal bool IsAiPindianPending() => _resolve().IsAiPindianPending();
    internal CommandResult SubmitPindianPromptAnswer(PromptChoice selected) => _resolve().SubmitPindianPromptAnswer(selected);
}
