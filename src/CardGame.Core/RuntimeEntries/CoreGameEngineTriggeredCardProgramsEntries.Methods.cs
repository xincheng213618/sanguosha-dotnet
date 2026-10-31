#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTriggeredCardProgramsEntries
{
    internal void AssertProgramEquipmentColorDiscardChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().AssertProgramEquipmentColorDiscardChoice(frame, effect);
    internal Card[] EquipmentColorDiscardCandidates(int ownerSeat) => _resolve().EquipmentColorDiscardCandidates(ownerSeat);
    internal bool IsProgramDiscardedDelayedTrickTarget(int ownerSeat, CharacterState target, ProgramSkillWindowContext? context) => _resolve().IsProgramDiscardedDelayedTrickTarget(ownerSeat, target, context);
    internal SkillProgramStepOutcome PayProgramEquipmentColorDiscard(ProgramSkillFrame frame, string resultBind) => _resolve().PayProgramEquipmentColorDiscard(frame, resultBind);
    internal void ResolveProgramEquipmentColorDiscardChoice(PromptChoice choice) => _resolve().ResolveProgramEquipmentColorDiscardChoice(choice);
    internal SkillProgramStepOutcome UseProgramDiscardedCardAsDelayedTrick(ProgramSkillFrame frame, int targetSeat, CardKind kind) => _resolve().UseProgramDiscardedCardAsDelayedTrick(frame, targetSeat, kind);
    internal SkillProgramStepOutcome UseTriggeredProgramVirtualSlash(ProgramSkillFrame frame, int targetSeat) => _resolve().UseTriggeredProgramVirtualSlash(frame, targetSeat);
}
