#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHoldCardsEntries
{
    internal SkillProgramStepOutcome HoldProgramTargetCards(ProgramSkillFrame frame, int chooserSeat, int holderSeat, IReadOnlyList<CardZoneKind> zones, string resultBind, int minimumCards) => _resolve().HoldProgramTargetCards(frame, chooserSeat, holderSeat, zones, resultBind, minimumCards);
    internal void PublishProgramHoldCardSelection(ProgramSkillFrame frame) => _resolve().PublishProgramHoldCardSelection(frame);
    internal void ResolveProgramHoldCardSelection(ProgramSkillFrame frame, PromptChoice selected) => _resolve().ResolveProgramHoldCardSelection(frame, selected);
    internal void ReturnProgramPojunHoldsAtTurnEnd() => _resolve().ReturnProgramPojunHoldsAtTurnEnd();
}
