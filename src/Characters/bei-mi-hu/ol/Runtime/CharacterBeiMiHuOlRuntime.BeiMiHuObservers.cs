#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal bool ValidBeiMiHu(ProgramSkillFrame f)
    {
        if (f.BeiMiHu is not { } r || r.Source != BeiMiHuSource(f) || r.GameplayHash != f.GameplayHash || r.ParentFrameId != f.WindowContext?.ParentFrameId || r.InstructionIndex != f.InstructionIndex || r.Turn != BeiMiHuReceiptTurn(f) || f.InstructionIndex != 1 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (r.Stage is "round-choice" or "before-turn-choice")
            return ExactBeiMiHuLifecycleParent(f) && r.Candidates.Count > 0 && r.Candidates.Distinct().Count() == r.Candidates.Count && events.OfType<BeiMiHuChoiceStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var started] && started.Source == r.Source && started.GameplayHash == r.GameplayHash && started.ParentFrameId == r.ParentFrameId && started.Turn == r.Turn && started.Round == r.Round && started.Candidates.SequenceEqual(r.Candidates);
        if (r.Stage == "draw-children")
            return r.DamageParent is { } damage && r.DamageTarget is { } target && r.Before <= r.After && r.ActualDraw is >= 0 and <= 1 && events.OfType<BeiMiHuGujuDrawPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var draw] && draw == new BeiMiHuGujuDrawPaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, damage, target, r.Turn, r.Before, r.After, r.ActualDraw) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BeiMiHuDrawReason) == r.ActualDraw;
        if (r.Stage is "maximum-children" or "recovery-children" or "grants-children")
            return ExactBeiMiHuLifecycleParent(f) && (r.CumulativeDraw >= 7 || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningReceipt(f)) && BeiMiHuActualCards(f.OwnerSeat) >= r.CumulativeDraw && r.MaximumAfter == r.MaximumBefore + 1 && events.OfType<BeiMiHuAwakeningMaximumPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var maximum] && maximum == new BeiMiHuAwakeningMaximumPaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.Turn, r.CumulativeDraw, r.MaximumBefore, r.MaximumAfter) && events.OfType<MaximumHpChangedEvent>().Any(e => e.PlayerSeat == f.OwnerSeat && e.MaximumHp == r.MaximumAfter && e.Delta == 1 && e.SkillId == f.SkillId) && r.RecoveryRequested is >= 0 and <= 1;
        if (r.Stage == "append-choice")
            return r.CardUseId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && use.Action?.ActionId == r.ActionId && use.TargetSeats.SequenceEqual(r.OriginalTargets) && events.OfType<BeiMiHuChoiceStartedEvent>().Where(e => e.FrameId == f.Id && e.Kind == "append").ToArray()is [var choice] && choice.Source == r.Source && choice.Candidates.SequenceEqual(r.Candidates) && r.SelectedTargets.All(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) && r.Choices.All(c => c.Parameters.GetValueOrDefault("frame-id") == f.Id.ToString(CultureInfo.InvariantCulture));
        return r.Stage is "cancel-paid" or "append-paid" && r.CardUseId is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { } paidUse && ValidBeiMiHuTargetMutation(paidUse);
    }

    internal void AssertBeiMiHu(ProgramSkillFrame f)
    {
        if (f.BeiMiHu is not null && !ValidBeiMiHu(f))
            throw new InvalidOperationException("Bei Mi Hu lost its actual lifecycle/damage invoice or exact paid target mutation.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 170)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 170)]
    internal bool CanContinueBeiMiHu(ProgramSkillFrame f) => f.BeiMiHu is { } r && r.Stage is not ("round-choice" or "before-turn-choice" or "append-choice") && ValidBeiMiHu(f);
    internal bool BeiMiHuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BeiMiHu is not { } r || !ValidBeiMiHu(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == "draw-children" && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.OwnerSeat && (r.Stage == "maximum-children" ? hp.Change.Kind == HpChangeKind.MaximumHp : r.Stage == "recovery-children" && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.Amount <= r.RecoveryRequested);
        if (child is ProgramLifecycleTriggerWindowFrame skills)
            return r.Stage == "grants-children" && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage == "recovery-children" && recovery.ParentFrameId == f.Id && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == r.RecoveryRequested && recovery.Attempt.ActualTurnNumber == r.Turn && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == r.InstructionIndex;
        return false;
    }

    internal ProgramSkillFrame? BeiMiHuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.BeiMiHuFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 230)]
    internal bool TryAdvanceBeiMiHuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BeiMiHuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
