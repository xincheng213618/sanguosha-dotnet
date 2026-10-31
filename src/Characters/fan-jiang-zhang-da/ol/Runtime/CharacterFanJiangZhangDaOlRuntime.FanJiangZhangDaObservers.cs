#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanJiangZhangDaOlRuntime
{

    internal bool FanJiangZhangDaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Juesheng is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == JueshengStage.NativeUse && use.JueshengDuel == r.Origin && IsJueshengDuelUse(use.Id);
        return r.Stage == JueshengStage.GrantChildren && child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed && changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? FanJiangZhangDaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterFanJiangZhangDaOlOperations.FanJiangZhangDaFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 810)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 920)]
    internal bool HasFanJiangZhangDaDamageObserver(long id) => FanJiangZhangDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 950)]
    internal bool HasFanJiangZhangDaBeforeDamageObserver(long id) => FanJiangZhangDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27350)]
    internal bool AdvanceFanJiangZhangDaSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FanJiangZhangDaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
