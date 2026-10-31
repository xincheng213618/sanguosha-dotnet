#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnedCardsEntries
{
    internal void AssertProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertProgramOwnedCardSelection(frame, paused);
    internal global::CardGame.Core.SkillProgramEffect GetOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GetOwnedSelectionEffect(frame);
    internal int GetProgramOwnerLostHp(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GetProgramOwnerLostHp(frame);
    internal void PublishProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramOwnedCardSelection(frame);
    internal void ResolveProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramOwnedCardSelection(frame, effect, selected);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectProgramOwnedCards(global::CardGame.Core.ProgramSkillFrame frame, int cardOwnerSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? expression, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> suits) => _resolve().SelectProgramOwnedCards(frame, cardOwnerSeat, amount, expression, zones, resultBind, minimumCards, maximumCards, cardKinds, suits);
}
