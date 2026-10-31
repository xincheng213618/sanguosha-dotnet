#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFengXiAnniversaryRuntime
{
    internal bool HasAnniversaryFengXiPrompt()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryFengXi: { } r, Step: ResolutionFrameStep.AwaitingResponse } f || r.Stage is not (AnniversaryFengXiStage.Choice or AnniversaryFengXiStage.DiscardChoice) || p.PlayerSeat != (r.Stage == AnniversaryFengXiStage.Choice ? f.OwnerSeat : r.Target) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, FengXiChoices(f)))
            return false;
        RequireFengXi(f);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 44500)]
    internal void RequireFengXi(ProgramSkillFrame f)
    {
        if (f.AnniversaryFengXi is not { } r)
            return;
        if (r.Source != FengXiSource(f) || r.Hash != f.GameplayHash || r.Instruction != f.InstructionIndex || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.Operation != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op || r.ParentId != f.WindowContext?.ParentFrameId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || r.Target == f.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Amount < 0 || r.Cards.Distinct().Count() != r.Cards.Count || r.Cards.Count > r.Amount || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryFengXiStartedEvent>().Count(e => e == new AnniversaryFengXiStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.Instruction, r.Turn, r.ParentId, r.UseId, r.ActionId, r.Target)) != 1)
            throw new InvalidOperationException("Feng Xi lost his accepted source, current instruction, real target or once-issued receipt.");
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryFengXiYusui && (f.InstructionIndex != 2 || f.WindowContext?.CardUse is not { IsPublicRed: false } card || card.ParentCardUseFrameId != r.UseId || card.CardActionId != r.ActionId || card.ActorSeat != r.Target || f.SkillId != AnniversaryFengXiComposition.Yusui))
            throw new InvalidOperationException("Yusui cannot replace the original black-card user after its native one-HP cost.");
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryFengXiBoyan && (f.InstructionIndex != 1 || f.SelectedTargetSeats is not [var target] || target != r.Target || f.SkillId != AnniversaryFengXiComposition.Boyan))
            throw new InvalidOperationException("Boyan cannot replace its chosen other player.");
        if (r.Stage is AnniversaryFengXiStage.HealthApplying or AnniversaryFengXiStage.HealthChildren)
            if (r.OwnerHp is not { } own || r.TargetHp is not { } other || r.Amount != Math.Max(0, checked(other - own)) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryFengXiHealthIssuedEvent>().Count(e => e == new AnniversaryFengXiHealthIssuedEvent(f.Id, r.Target, r.Amount, own, other)) != 1)
                throw new InvalidOperationException("Yusui requires its frozen real HP comparison before the native loss.");
        if (r.Stage == AnniversaryFengXiStage.MovementChildren && (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat || r.Cards.Count != r.Amount || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryFengXiPaidEvent>().Count(e => e == new AnniversaryFengXiPaidEvent(f.Id, r.Target, r.Amount, r.Before, r.After)) != 1 || r.Cards.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && (m.From == CardLocation.Hand(r.Target) || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(r.Target)) && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)) && m.Reason == FengXiDiscardReason) != 1)))
            throw new InvalidOperationException("Yusui lost its actual user's one-time Hand discard.");
        if (r.Draw is { } d && (d.RecipientSeat != r.Target || d.RequestedCount != r.Amount || d.SequenceBefore != r.Before || d.SequenceAfter != r.After || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryFengXiDrawReturnedEvent>().Count(e => e == new AnniversaryFengXiDrawReturnedEvent(f.Id, r.Target, d.RequestedCount, d.ActualDrawIds.Count, r.Before, r.After, d.AttemptFrameId)) != 1))
            throw new InvalidOperationException("Boyan lost its completed native draw invoice.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 44500)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 44500)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 44500)]
    internal bool FengXiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.AnniversaryFengXi is not { Stage: AnniversaryFengXiStage.MovementChildren or AnniversaryFengXiStage.DrawChildren } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        RequireFengXi(f);
        return true;
    }

    internal bool FengXiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryFengXi is not { } r)
            return false;
        RequireFengXi(f);
        if (child is HappyQiaoHealthChoiceFrame qiao)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(qiao, f);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryFengXiStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == r.Target && draw.Context.RequestedCount == r.Amount && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is IdentityAppearanceFrame a)
            return r.Stage is AnniversaryFengXiStage.HealthApplying or AnniversaryFengXiStage.HealthChildren && a.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && a.Receipt.ParentFrameId == f.Id && a.Receipt.OwnerSeat == r.Target && a.Receipt.OriginalInstructionIndex == f.InstructionIndex && a.Receipt.RequestedDeduction == r.Amount;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && FengXiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == (f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == AnniversaryFengXiStage.HealthChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == r.Target && hp.Change.Kind == HpChangeKind.Loss && hp.Continuation == PostEventContinuation.Program;
        if (child is DyingFrame dying)
            return r.Stage == AnniversaryFengXiStage.HealthChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Target && dying.Continuation == DyingContinuationKind.ProgramSkill && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.TargetSeat == r.Target && e.Amount == r.Amount && e.RemainingHp <= 0);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal ProgramSkillFrame? FengXiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterFengXiAnniversaryOperations.FengXiFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations2.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 44500)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 44500)]
    internal bool FengXiDamageObserver(long id) => FengXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 44500)]
    internal bool FengXiBeforeDamageObserver(long id) => FengXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 44500)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44500)]
    internal bool FengXiDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && FengXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 44500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 44500)]
    internal bool FengXiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryFengXi is null)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n && FengXiFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && (c == n + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(n)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44500)]
    internal bool AdvanceFengXiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FengXiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
