#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHoldCardsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome HoldProgramTargetCards(global::CardGame.Core.ProgramSkillFrame frame, int chooserSeat, int holderSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, string resultBind, int minimumCards) => _resolve().HoldProgramTargetCards(frame, chooserSeat, holderSeat, zones, resultBind, minimumCards);
    internal void PublishProgramHoldCardSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramHoldCardSelection(frame);
    internal void ResolveProgramHoldCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramHoldCardSelection(frame, selected);
    internal void ReturnProgramPojunHoldsAtTurnEnd() => _resolve().ReturnProgramPojunHoldsAtTurnEnd();
}
