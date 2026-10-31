#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedHandChoiceObserversRuntime
{
    internal bool IsNamedHandChoiceMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement)
    {
        if (movement.SubjectSeat != f.OwnerSeat || movement.BeforeCount != 0 || movement.CoverageResultBind is not null)
            return false;
        if (effect?.Op is SkillProgramEffectOp.RecordNamedPrivateAmbush or SkillProgramEffectOp.NullifyMatchingHandUse && f.NamedAmbush is { Stage: NamedAmbushStage.MovementChildren })
        {
            _coreNamedAmbushOperations.RequireNamedAmbushReceipt(f);
            return true;
        }

        if (effect?.Op == SkillProgramEffectOp.ResolveDamageSourceHandChoice && f.DamageSourceHandChoice is { Stage: DamageSourceHandChoiceStage.MovementChildren })
        {
            _coreDamageSourceHandChoiceOperations.RequireDamageSourceHandChoice(f);
            return true;
        }

        return false;
    }

    internal bool NamedHandChoiceFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.NamedAmbush is null && root.DamageSourceHandChoice is null)
            return false;
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(root);
        if (root.PendingMovementContinuation is { } movement && IsNamedHandChoiceMovement(root, effect, movement))
        {
            if (child is CardsMovedTriggerWindowFrame moved)
            {
                if (moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId != root.Id || moved.ResumeProgramFrameId is not null || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0)
                    return false;
                if (root.NamedAmbush is { } ambush)
                    return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > ambush.MovementBefore && m.Sequence <= ambush.MovementAfter && m.CardId == ambush.Deposit!.CardId && m.Reason.Value == (ambush.Removing ? "skill-program.named-ambush.remove" : "skill-program.named-ambush.record"));
                var r = root.DamageSourceHandChoice!;
                return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value == (r.Draw ? "skill-program.damage-source-hand.draw" : "skill-program.damage-source-hand.discard"));
            }

            return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        }

        if (root.DamageSourceHandChoice is not { Stage: DamageSourceHandChoiceStage.DamageIssued } || root.AttackAttempt is not { } attack || root.AttackReturn is null)
            return false;
        _coreDamageSourceHandChoiceOperations.RequireDamageSourceHandChoice(root);
        if (child is BeforeDamageProgramWindowFrame before)
            return before.ParentFrameId == root.Id && before.Continuation == BeforeDamageProgramContinuation.Attack && before.SourceSeat == attack.SourceSeat && (before.RedirectedTargetSeat ?? before.TargetSeat) == attack.TargetSeat && before.Amount > 0 && before.Nature == attack.Nature;
        if (child is DyingFrame { Continuation: DyingContinuationKind.AttackHpLoss } replaced)
            return _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverAttackHpLossDyingMatches(root, replaced);
        return child is DamageFrame damage && damage.ParentFrameId == root.Id && damage.SourceSeat == attack.SourceSeat && damage.TargetSeat == attack.TargetSeat && damage.Amount == attack.DamageAmount && damage.Nature == attack.Nature;
    }

    internal ProgramSkillFrame? NamedHandChoiceObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreNamedHandChoiceObserversOperations.NamedHandChoiceFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 80)]
    internal bool TryAdvanceNamedHandChoiceSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && NamedHandChoiceObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
