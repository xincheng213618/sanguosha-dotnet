#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangFuZhaoLeiMobileRuntime
{
    internal global::CardGame.Core.GameEngine.IDamageAttempt? XunyiOriginalAttack(XunyiDamageReceipt receipt) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == receipt.OriginalAttackId) switch
    {
        ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(receipt.OriginalAttackId),
        CardUseFrame { CardAttack.Active: true } or ProgramSkillFrame { CardAttack.Active: true } or JudgmentFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(receipt.OriginalAttackId),
        _ => null
    };
    internal bool ValidXunyi(ProgramSkillFrame frame)
    {
        if (frame.Xunyi is not { } pending)
            return false;
        var receipt = pending.Receipt;
        var relation = receipt.Relation;
        if (relation.Source.OwnerSeat != frame.OwnerSeat || relation.Source.SkillId != frame.SkillId || relation.Source.SkillInstanceId != frame.SkillInstanceId || relation.GameplayHash != frame.GameplayHash || frame.InstructionIndex != pending.InstructionIndex || pending.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.DamageFrameId != receipt.DamageFrameId || context.Amount != receipt.ActualAmount || context.TargetSeat != receipt.ActualTargetSeat || receipt.ActualAmount <= 0 || relation.HolderSeat == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(relation.HolderSeat) || XunyiBranches(receipt)is < 1 or > 2 || pending.StepIndex < 0 || pending.StepIndex > XunyiStepCount(receipt) || !Enum.IsDefined(pending.Stage) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.SettleXunyiDamagePoint || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiRelationChangedEvent>().Count(e => e.Relation == relation) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiDamageQualifiedEvent>().Count(e => e.Receipt == receipt) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiDamageIssuedEvent>().Count(e => e.FrameId == frame.Id && e.Receipt == receipt && e.InstructionIndex == pending.InstructionIndex) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != receipt.DamageFrameId || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack[index - 2] is not DamageFrame damage || damage.Id != receipt.DamageFrameId || damage.ParentFrameId != receipt.OriginalAttackId || damage.TargetSeat != receipt.ActualTargetSeat || damage.Amount != receipt.ActualAmount)
            return false;
        var attack = XunyiOriginalAttack(receipt);
        if (attack is null || !attack.DamageWasApplied || attack.TargetSeat != receipt.ActualTargetSeat || attack.DamageAmount != receipt.ActualAmount || (attack.IsSourceLess || attack.IsDelayedJudgmentDamage ? (int? )null : attack.SourceSeat) != receipt.ActualSourceSeat)
            return false;
        var source = receipt.ActualSourceSeat;
        var owner = frame.OwnerSeat;
        var other = relation.HolderSeat;
        int? discard = receipt.ActualTargetSeat == owner && source != other ? other : receipt.ActualTargetSeat == other && source != owner ? owner : null;
        int? draw = source == owner && receipt.ActualTargetSeat != other ? other : source == other && receipt.ActualTargetSeat != owner ? owner : null;
        if (discard != receipt.DiscardRecipientSeat || draw != receipt.DrawRecipientSeat)
            return false;
        var payments = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XunyiPointPaidEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        var paidCount = pending.StepIndex + (pending.Stage == XunyiStage.Children ? 1 : 0);
        if (payments.Length != paidCount || payments.Select(e => e.StepIndex).Order().Where((step, n) => step != n).Any() || payments.Any(e => e.DamageFrameId != receipt.DamageFrameId || e.StepIndex >= XunyiStepCount(receipt)))
            return false;
        if (pending.Stage == XunyiStage.DiscardChoice)
            return frame.PendingMovementContinuation is null && pending.Before == 0 && pending.After == 0 && pending.Count == 0 && pending.CardId is null && pending.From is null;
        if (pending.StepIndex >= XunyiStepCount(receipt) || pending.Before < 0 || pending.After < pending.Before || pending.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || pending.Count is < 0 or > 1)
            return false;
        var stepInfo = XunyiStep(pending);
        if (frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } movement || movement.SubjectSeat != stepInfo.Seat || payments.Single(e => e.StepIndex == pending.StepIndex) != new XunyiPointPaidEvent(frame.Id, receipt.DamageFrameId, pending.StepIndex, stepInfo.Seat, stepInfo.Draw, pending.Before, pending.After, pending.Count, pending.CardId, pending.From))
            return false;
        if (stepInfo.Draw)
            return pending.CardId is null && pending.From is null && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > pending.Before && m.Sequence <= pending.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(stepInfo.Seat) && m.Reason.Value == "skill-program.xunyi.draw") == pending.Count;
        return pending.Count == 0 ? pending.CardId is null && pending.From is null : pending.CardId is { } id && pending.From is { OwnerSeat: { } cardOwner } from && cardOwner == stepInfo.Seat && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > pending.Before && m.Sequence <= pending.After && m.CardId == id && m.From == from && m.Reason.Value == "skill-program.xunyi.discard" && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(id).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)))) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26320)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26320)]
    internal bool ContinueIssuedXunyi(ProgramSkillFrame frame) => ValidXunyi(frame);
    internal bool XunyiFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidXunyi(frame) || frame.Xunyi is not { Stage: XunyiStage.Children } pending)
            return false;
        var(seat, draw) = XunyiStep(pending);
        var reason = draw ? "skill-program.xunyi.draw" : "skill-program.xunyi.discard";
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > pending.Before && m.Sequence <= pending.After && m.Reason.Value == reason);
        if (draw || pending.CardId is not { } id || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind != CardKind.SilverLion || pending.From != CardLocation.Equipment(seat))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == frame.Id && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion is { Producer: RecoveryAttemptProducer.SilverLion, MoveReason: { } moveReason } && moveReason.Value == reason;
        return child is HpChangedTriggerWindowFrame hp && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(frame, hp) && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 80)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 70)]
    internal bool IsXunyiRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame child) => XunyiFirstChild(frame, child);
    internal ProgramSkillFrame? XunyiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => _characterWangFuZhaoLeiMobileOperations.XunyiFirstChild(frame, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26320)]
    internal bool AdvanceXunyiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XunyiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
