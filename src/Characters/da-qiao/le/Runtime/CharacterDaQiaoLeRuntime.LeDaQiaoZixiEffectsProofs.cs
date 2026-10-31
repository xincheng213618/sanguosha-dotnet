#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoLeRuntime
{
    internal bool ValidZixiEffectsStarted(ProgramSkillFrame f)
    {
        if (f.ZixiEffects is not { } r || f.SkillId != "le:zixi" || f.TriggerId != "unique-target-judgment-effects" || f.InstructionIndex != 1 || r.Source != ZixiEffectsSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveZixiUniqueTargetJudgmentEffects || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || !TryGetZixiEffectsUse(f.OwnerSeat, context, out var use, out var window, out var target) || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || use.CardKind != r.CardKind || target != r.TargetSeat || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context, window))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZixiEffectsStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var started] && started == new ZixiEffectsStartedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.WindowId, r.TargetSeat, r.JudgmentCount, r.CardKind, r.SequenceBefore) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZixiEffectsStartedEvent>().Count(e => e.UseId == r.UseId && e.Source.OwnerSeat == f.OwnerSeat) == 1;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 230)]
    internal bool IsZixiEffectsMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.ZixiEffects is { Stage: ZixiEffectsStage.DrawChildren or ZixiEffectsStage.ObtainChildren } && effect?.Op == SkillProgramEffectOp.ResolveZixiUniqueTargetJudgmentEffects && ValidZixiEffectsStarted(f) && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    internal bool ZixiEffectsPrimaryMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ZixiEffects!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == ZixiEffectsStage.ObtainChildren)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<ZixiEffectsEntityObtainedEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.PrintedKind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence && e.BatchId == r.BatchId && m.Reason.Value == ZixiEffectsObtainReason);
        return r.Stage == ZixiEffectsStage.DrawChildren && r.DrawOutcome is { } o && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZixiEffectsDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    internal bool ZixiEffectsOxSpill(ProgramSkillFrame f, CardMovementBatchContext batch) => f.ZixiEffects is { Stage: ZixiEffectsStage.ObtainChildren, BatchId: > 0 } r && batch.ParentBatchId == r.BatchId && batch.Movements.Count > 0 && batch.Movements.All(m => m.From == CardLocation.WoodenOxGrain(r.TargetSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && r.Entities.Any(e => e.PrintedKind == CardKind.WoodenOx && e.From == CardLocation.Equipment(r.TargetSeat));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 390)]
    internal bool IsZixiEffectsRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.ZixiEffects is { Stage: ZixiEffectsStage.ObtainChildren } r && ValidZixiEffectsStarted(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == ZixiEffectsObtainReason && recovery.Attempt.SourceSeat == r.TargetSeat && recovery.Attempt.TargetSeat == r.TargetSeat && recovery.Attempt.Amount == 1 && r.Entities.Any(e => e.PrintedKind == CardKind.SilverLion && e.From == CardLocation.Equipment(r.TargetSeat));
}
