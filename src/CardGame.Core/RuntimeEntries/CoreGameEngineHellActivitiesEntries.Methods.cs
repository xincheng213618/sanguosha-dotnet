#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHellActivitiesEntries
{
    internal bool CanPlanHellDiscard(CardLocation destination) => _resolve().CanPlanHellDiscard(destination);
    internal ModeNpcArrivalSourceFacts? CaptureNativeArrivalProgramSource(ProgramSkillFrame frame) => _resolve().CaptureNativeArrivalProgramSource(frame);
    internal bool HasHellRemenImmunity(CharacterState player, CardKind kind) => _resolve().HasHellRemenImmunity(player, kind);
    internal bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame window) => _resolve().IsExactHellAppearanceWindow(window);
    internal bool IsExactHellArrivalProgram(ProgramSkillFrame frame) => _resolve().IsExactHellArrivalProgram(frame);
    internal bool IsExactHellPreparationTargetContext(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().IsExactHellPreparationTargetContext(ownerSeat, context);
    internal bool IsHellNpc(CharacterState player) => _resolve().IsHellNpc(player);
    internal void RecordHellInterceptedMovement(HellShiyouTransferPlan plan, CardMovementRecord move, Action<CardMovementRecord>? beforeFact) => _resolve().RecordHellInterceptedMovement(plan, move, beforeFact);
}
