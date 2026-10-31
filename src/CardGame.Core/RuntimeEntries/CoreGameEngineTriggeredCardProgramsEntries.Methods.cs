#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTriggeredCardProgramsEntries
{
    internal void AssertProgramEquipmentColorDiscardChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().AssertProgramEquipmentColorDiscardChoice(frame, effect);
    internal global::CardGame.Core.Card[] EquipmentColorDiscardCandidates(int ownerSeat) => _resolve().EquipmentColorDiscardCandidates(ownerSeat);
    internal bool IsProgramDiscardedDelayedTrickTarget(int ownerSeat, global::CardGame.Core.CharacterState target, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().IsProgramDiscardedDelayedTrickTarget(ownerSeat, target, context);
    internal global::CardGame.Core.SkillProgramStepOutcome PayProgramEquipmentColorDiscard(global::CardGame.Core.ProgramSkillFrame frame, string resultBind) => _resolve().PayProgramEquipmentColorDiscard(frame, resultBind);
    internal void ResolveProgramEquipmentColorDiscardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramEquipmentColorDiscardChoice(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome UseProgramDiscardedCardAsDelayedTrick(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.CardKind kind) => _resolve().UseProgramDiscardedCardAsDelayedTrick(frame, targetSeat, kind);
    internal global::CardGame.Core.SkillProgramStepOutcome UseTriggeredProgramVirtualSlash(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().UseTriggeredProgramVirtualSlash(frame, targetSeat);
}
