#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHellActivitiesEntries
{
    internal bool CanPlanHellDiscard(global::CardGame.Core.CardLocation destination) => _resolve().CanPlanHellDiscard(destination);
    internal global::CardGame.Core.ModeNpcArrivalSourceFacts? CaptureNativeArrivalProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CaptureNativeArrivalProgramSource(frame);
    internal bool HasHellRemenImmunity(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => _resolve().HasHellRemenImmunity(player, kind);
    internal bool IsExactHellAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().IsExactHellAppearanceWindow(window);
    internal bool IsExactHellArrivalProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactHellArrivalProgram(frame);
    internal bool IsExactHellPreparationTargetContext(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().IsExactHellPreparationTargetContext(ownerSeat, context);
    internal bool IsHellNpc(global::CardGame.Core.CharacterState player) => _resolve().IsHellNpc(player);
    internal void RecordHellInterceptedMovement(global::CardGame.Core.HellShiyouTransferPlan plan, global::CardGame.Core.CardMovementRecord move, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact) => _resolve().RecordHellInterceptedMovement(plan, move, beforeFact);
}
