#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnedDeathBenefitObserversRuntime
{
    internal bool OwnedDeathBenefitObserverPrefix(ProgramSkillFrame root)
    {
        if (!_coreOwnedDeathBenefitReturnsOperations.ExactOwnedDeathBenefitReturn(root) || !_coreFixedRecipientBenefitsOperations.ValidFixedRecipientReceipt(root))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        if (index == _resolutionStackOperations._resolutionStack.Count - 1)
            return true;
        if (!_corePairBenefitObserversOperations.PairBenefitFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]))
            return false;
        for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            // Each helper below proves the complete exact rescue suffix. Do not
            // subsequently reinterpret bound/virtual Alcohol as native Peach.
            if (_resolutionStackOperations._resolutionStack[child - 1] is DyingFrame rescued && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child - 1, rescued) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child - 1, rescued) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child - 1, rescued) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child - 1, rescued)))
                return true;
            if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) && !OwnedDeathBenefitDeadDyingEdge(child))
                return false;
        }

        return true;
    }

    internal ProgramSkillFrame? OwnedDeathBenefitObserverRoot() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.OwnedDeathBenefitReturn is not null && OwnedDeathBenefitObserverPrefix(f));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 680)]
    internal bool IsOwnedDeathBenefitProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && OwnedDeathBenefitObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    internal bool OwnedDeathBenefitDeadDyingEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index] is not DyingFrame dying || !_coreOwnedDeathBenefitReturnsOperations.IsOriginalDyingSuspendedByOwnedDeathBenefit(dying))
            return false;
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        return dying.Continuation switch
        {
            DyingContinuationKind.ProgramSkill => parent is ProgramSkillFrame program && program.Id == dying.ParentFrameId,
            DyingContinuationKind.Damage => parent is DamageFrame damage && damage.Id == dying.ParentFrameId && damage.TargetSeat == dying.VictimSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == damage.ParentFrameId)is { AttackAttempt: { } attack, AttackReturn: not null } && attack.TargetSeat == dying.VictimSeat && dying.KillerSeat == (attack.SourceLess ? null : attack.SourceSeat),
            DyingContinuationKind.AttackHpLoss => parent is ProgramSkillFrame { AttackAttempt: { } attack, AttackReturn: not null } program && program.Id == dying.ParentFrameId && attack.TargetSeat == dying.VictimSeat && dying.KillerSeat is null,
            _ => false
        };
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 550)]
    internal bool TryAdvanceOwnedDeathBenefitSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || OwnedDeathBenefitObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: false, incompleteAttackError: "Owned death-benefit damage must finish its actual damage/Dying children before its program tail.");
    }
}
