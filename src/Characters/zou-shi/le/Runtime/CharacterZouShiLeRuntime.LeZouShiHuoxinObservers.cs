#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZouShiLeRuntime
{
    internal bool IsHuoxinMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        frame.Huoxin is { Stage: HuoxinStage.ObtainChildren } && effect?.Op == SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse;
    internal bool HuoxinFirstChild(ProgramSkillFrame frame, ResolutionFrame child) =>
        frame.Huoxin is { Stage: HuoxinStage.ObtainChildren } && child.RuntimeOrigin?.ParentId == frame.Id;
    internal bool HuoxinObserverSuffix(int index) => _coreObserverSubtreesOperations.HasOwnedObserverSuffix(index);
    internal ProgramSkillFrame? HuoxinObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot(HuoxinFirstChild, HuoxinObserverSuffix);

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 450)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 440)]
    internal bool IsHuoxinRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.Huoxin is null || recovery.ParentFrameId == frame.Id) return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(value => value.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(value => value.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && HuoxinObserverSuffix(parent);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28951)]
    internal bool AdvanceHuoxinChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null &&
        HuoxinObserverRoot() is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
