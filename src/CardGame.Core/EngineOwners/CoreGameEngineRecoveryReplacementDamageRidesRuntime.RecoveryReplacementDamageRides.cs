#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecoveryReplacementDamageRidesRuntime
{
    internal bool RecoveryReplacementFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => ride is RecoveryReplacementFrame recovery && recovery.ParentFrameId == beneath.Id && recovery.Return.ResumeFrameId == beneath.Id && recovery.Return.Continuation switch
    {
        PostEventContinuation.Program or PostEventContinuation.AwaitedProgramMovement => beneath is ProgramSkillFrame,
        PostEventContinuation.FriendDuelResponse => beneath is FriendDuelResponseFrame,
        PostEventContinuation.GuanduCardEffect => beneath is CardUseFrame guandu && _coreGuanduCardsOperations.IsGuanduCardEffectRecoveryReturn(guandu, recovery),
        PostEventContinuation.WumiaoFlood => beneath is CardUseFrame flood && _characterGuanYuWumiaoOperations.IsWumiaoFloodRecoveryReturn(flood, recovery),
        PostEventContinuation.DragonBoatCardEffect => beneath is CardUseFrame dragon && _coreDragonBoatCardsOperations.IsDragonBoatCardEffectRecoveryReturn(dragon, recovery),
        PostEventContinuation.CardUse or PostEventContinuation.GroupRecovery or PostEventContinuation.VirtualBasicCardUse or PostEventContinuation.RecoveryProducer => beneath is CardUseFrame,
        PostEventContinuation.DenglouCardUse => beneath is CardUseFrame { DenglouNative: not null, DenglouCostDrained: false },
        PostEventContinuation.GuoSiCardUse => beneath is CardUseFrame { GuoSiNative: not null, GuoSiCostDrained: false },
        PostEventContinuation.RecoveryPaidCardUse => beneath is CardUseFrame { RecoveryPaidContinuation: not null },
        PostEventContinuation.FactionRequestCost => beneath.PaidFactionRequestCostRecovery is not null,
        PostEventContinuation.RecoveryReplacement => beneath is RecoveryReplacementFrame,
        _ => false
    };
    internal static bool PaidCardUseHpFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => RuntimeSkills.RuntimeRules_Core_RecoveryReplacementDamageRides.PaidCardUseHpFrameRidesOn(ride, beneath);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 110)]
    internal bool IsPaidCardUseProgramDying() => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.RecoveryPaidContinuation?.Kind is RecoveryPaidCardUseKind.DodgeCompletion or RecoveryPaidCardUseKind.CommittedSlash && IsRecoveryReplacementProgramDying(attack.ResolutionId);
    internal bool RecoveryReplacementDamageObserverRidesOn(int index)
    {
        if (!CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]))
            return false;
        // Only a contiguous chain of exact observer returns may borrow the recovery
        // producer's damage cursor. An unrelated HP or movement observer still stops it.
        for (var parentIndex = index - 1; parentIndex >= 1; parentIndex--)
        {
            var frame = _resolutionStackOperations._resolutionStack[parentIndex];
            var parent = _resolutionStackOperations._resolutionStack[parentIndex - 1];
            if (RecoveryReplacementFrameRidesOn(frame, parent) || PaidCardUseHpFrameRidesOn(frame, parent))
                return true;
            if (!_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(frame, parent) && !CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(frame, parent) && !_corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(frame, parent) && !_coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(frame, parent))
                return false;
        }

        return false;
    }
    internal bool IsRecoveryReplacementProgramDying(long? requiredPaidUseId = null)
    {
        var paymentCursor = _coreTypedRecoveryDamageCursorOperations.ProjectTypedCommittedSlashPaymentCursor();
        if (paymentCursor.IsMalformed || paymentCursor.Owner is { } typedOwner && (requiredPaidUseId is null || requiredPaidUseId == typedOwner.Id))
            return false;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { ResumesProgramSkill: true } dying || _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(f => f.Id == dying.FrameId)is not { } child || child.ParentFrameId != dying.ParentFrameId)
            return false;
        var effectiveTop = _coreDamageObserverCursorQueriesOperations.DamageCursorEffectiveTop(includeNestedObservers: true);
        var topMatches = effectiveTop is DyingFrame topDying && topDying.Id == child.Id || effectiveTop is ProgramSkillFrame { WindowContext: { } response } && response.ParentFrameId == child.Id && response.Window is SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse;
        if (!topMatches)
            return false;
        var parentIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(f => f.Id == child.ParentFrameId);
        if (parentIndex < 1 || _resolutionStackOperations._resolutionStack[parentIndex] is not ProgramSkillFrame)
            return false;
        var hasRecovery = false;
        for (var index = parentIndex; index >= 0; index--)
        {
            var frame = _resolutionStackOperations._resolutionStack[index];
            if (hasRecovery && (requiredPaidUseId is { } paidUseId ? frame is CardUseFrame { RecoveryPaidContinuation: not null } paid && paid.Id == paidUseId : _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } damage && frame is ProgramSkillFrame { WindowContext: { } context } && context.ParentFrameId == damage.Id && context.Window is SkillProgramTriggerWindow.DamageAppliedBeforeDying or SkillProgramTriggerWindow.AfterDamageApplied))
                return true;
            if (index == 0)
                return false;
            var parent = _resolutionStackOperations._resolutionStack[index - 1];
            if (RecoveryReplacementFrameRidesOn(frame, parent) || PaidCardUseHpFrameRidesOn(frame, parent))
            {
                hasRecovery = true;
                continue;
            }

            if (parent is CardUseFrame paymentOwner && frame is CardsMovedTriggerWindowFrame movement && _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(paymentOwner, movement))
            {
                hasRecovery = true;
                continue;
            }

            if (!_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(frame, parent) && !CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(frame, parent) && !_corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(frame, parent) && !_coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(frame, parent))
                return false;
        }

        return false;
    }
}
