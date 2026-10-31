#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnedCardsEntries
{
    internal SkillProgramEffect GetOwnedSelectionEffect(ProgramSkillFrame frame) => _resolve().GetOwnedSelectionEffect(frame);
    internal int GetProgramOwnerLostHp(ProgramSkillFrame frame) => _resolve().GetProgramOwnerLostHp(frame);
    internal void PublishProgramOwnedCardSelection(ProgramSkillFrame frame) => _resolve().PublishProgramOwnedCardSelection(frame);
    internal void ResolveProgramOwnedCardSelection(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _resolve().ResolveProgramOwnedCardSelection(frame, effect, selected);
    internal SkillProgramStepOutcome SelectProgramOwnedCards(ProgramSkillFrame frame, int cardOwnerSeat, int amount, SkillProgramNumberExpression? expression, IReadOnlyList<CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, IReadOnlyList<CardKind> cardKinds, IReadOnlyList<Suit> suits) => _resolve().SelectProgramOwnedCards(frame, cardOwnerSeat, amount, expression, zones, resultBind, minimumCards, maximumCards, cardKinds, suits);
}
