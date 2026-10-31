#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaTieAnniversaryRuntime
{
    internal bool MaTieTurnParent(ProgramSkillFrame f)
    {
        if (f.SkillId != AnniversaryMaTieComposition.Quxian || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (c.Window == SkillProgramTriggerWindow.AfterTurnEnded)
            return f.TriggerId == "turn-ended" && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is DeferredTurnEndFrame parent && parent.OwnerSeat == f.OwnerSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && parent.AfterTurnEnded?.CurrentChild?.FrameId == f.Id && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, f, c);
        return f.TriggerId == "turn-start" && c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } start && start.OwnerSeat == f.OwnerSeat && start.CandidateIndex >= 0 && start.CandidateIndex < start.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, start.Candidates[start.CandidateIndex]);
    }

    internal bool ValidMaTie(ProgramSkillFrame f)
    {
        if (f.AnniversaryMaTie is not { } r || f.InstructionIndex != 1 || r.Source != MaTieSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || r.ParentId != f.WindowContext?.ParentFrameId || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieStartedEvent>().Count(e => e == new AnniversaryMaTieStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.TurnOwner)) != 1)
            return false;
        if (r.RewardParent > 0)
            return f.SkillId == AnniversaryMaTieComposition.Quxian && f.TriggerId == "issued-slash-reward" && MaTieCardParent(f, SkillProgramTriggerWindow.CardUseCommitted) && r.Stage is AnniversaryMaTieStage.RewardDrawing or AnniversaryMaTieStage.RewardChildren && r.UseId > 0 && r.ActionId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieRewardIssuedEvent>().Count(e => e.FrameId == f.Id && e.ParentId == r.RewardParent && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1 && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == r.RewardParent)is { AnniversaryMaTie: { Stage: AnniversaryMaTieStage.NativeUse } original } && original.UseId == r.UseId && original.ActionId == r.ActionId;
        if (!MaTieTurnParent(f) || r.Actors.Any(a => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(a) || a == r.Target) || r.Actors.Distinct().Count() != r.Actors.Count || r.Cursor < 0 || r.Cursor > r.Actors.Count || r.PenaltyCursor < 0 || r.PenaltyCursor > r.Actors.Count || r.Used.Distinct().Count() != r.Used.Count || r.Used.Any(a => !r.Actors.Contains(a)))
            return false;
        if (r.Target < 0)
            return r.Actors.Count == 0 && r.Used.Count == 0 && r.Cursor == 0 && r.Stage is AnniversaryMaTieStage.ObtainChildren or AnniversaryMaTieStage.Target;
        return _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) && r.Target != f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieSelectedEvent>().Count(e => e.FrameId == f.Id && e.Target == r.Target && e.Count == r.Actors.Count) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieQueueEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Actor).SequenceEqual(r.Actors);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 46700)]
    internal void RequireMaTie(ProgramSkillFrame f)
    {
        if (f.AnniversaryMaTie is not { } r)
            return;
        if (!ValidMaTie(f))
            throw new InvalidOperationException("Ma Tie lost its original lifecycle/card candidate, frozen queue, content identity or actual turn.");
        if (r.TargetDamaged && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieTargetDamagedEvent>().Any(e => e.FrameId == f.Id && e.Target == r.Target && e.Amount > 0))
            throw new InvalidOperationException("Quxian cannot replace actual issued-use damage with unrelated HP loss.");
        if (r.Stage is AnniversaryMaTieStage.NativeUse or AnniversaryMaTieStage.NativeReturned && (r.UseId <= 0 || r.ActionId <= 0 || r.SelectedAction is null || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieSlashIssuedEvent>().Count(e => e.Origin.FrameId == f.Id && e.Origin.Index == r.Cursor && e.Origin.UseId == r.UseId && e.Origin.ActionId == r.ActionId) != 1))
            throw new InvalidOperationException("Quxian native child lost its accepted use identity.");
        if (r.HpPaid && (r.Stage != AnniversaryMaTieStage.HpChildren || r.Used.Count <= 0 || r.HpBefore - r.HpAfter != r.Used.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaTieHpPaidEvent>().Count(e => e == new AnniversaryMaTieHpPaidEvent(f.Id, r.PenaltyCursor, r.HpTarget, r.Used.Count, r.HpBefore, r.HpAfter)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == r.HpTarget && e.Amount == r.Used.Count && e.RemainingHp == r.HpAfter) != 1))
            throw new InvalidOperationException("Quxian lost its once-paid actual HP loss before Dying.");
    }

    internal bool HasAnniversaryMaTiePrompt()
    {
        if (_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicInvariantTop()is not ProgramSkillFrame { AnniversaryMaTie: { Stage: AnniversaryMaTieStage.Target or AnniversaryMaTieStage.SlashChoice } r } f || !ValidMaTie(f) || _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicInvariantDecision()is not { } p)
            return false;
        p = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramNativeDecision(_characterChengJiChengCuiOlOperations.TousuiProgramNativeDecision(_characterMaXiuMaTieOlOperations.KenshangProgramNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime.WeijieNativeDecision(p))))));
        var actor = r.Stage == AnniversaryMaTieStage.Target ? f.OwnerSeat : r.Actors[r.Cursor];
        return p.Kind == DecisionKind.ProgramTrigger && p.IsPrivate && p.PlayerSeat == actor && p.TargetSeat == actor && p.SkillPrompt?.SkillId == f.SkillId && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && f.KenshangRequest is null && f.TousuiRequest is null ? MaTieChoices(f) : r.OriginalDecision!.Choices);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 46700)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 46700)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 46700)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 46700)]
    internal bool MaTieMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (!ValidMaTie(f) || f.AnniversaryMaTie is not { } r || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        if (r.Stage == AnniversaryMaTieStage.ObtainChildren)
            return effect?.Op == SkillProgramEffectOp.ResolveAnniversaryMaTieQuxian && (r.ObtainedCard == 0 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.ObtainedCard && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MaTieObtainReason) == 1);
        return r.Stage == AnniversaryMaTieStage.RewardChildren && effect?.Op == SkillProgramEffectOp.ResolveAnniversaryMaTieQuxianReward && r.Draw is { } draw && draw.RecipientSeat == f.OwnerSeat && draw.RequestedCount == 1;
    }

    internal bool MaTieFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMaTie(f))
            return false;
        var r = f.AnniversaryMaTie!;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryMaTieStage.NativeUse && MaTieNativeMatches(use, f);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryMaTieStage.RewardDrawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == 1 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is DrawFundedDistinctBasicFrame paidDraw)
            return IsAnniversaryMaTiePaymentFrame(paidDraw) && paidDraw.Payment.ParentFrameId == f.Id;
        if (child is ChainedStateBasicFrame paidChain)
            return IsAnniversaryMaTiePaymentFrame(paidChain) && paidChain.Payment.ParentFrameId == f.Id;
        if (child is RequestedDeckBasicFrame requested)
            return _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicFrameRidesOn(requested, f) && AnniversaryMaTieRequestActor(f, requested.Receipt.ActorSeat);
        if (child is ProgramSkillFrame { EquipmentSlotNamedUse: { } slot })
            return AnniversaryMaTieSlotParent(slot)?.Id == f.Id;
        if (child is DyingFrame dying)
            return r.Stage == AnniversaryMaTieStage.HpChildren && r.HpPaid && dying.ParentFrameId == f.Id && dying.VictimSeat == r.HpTarget && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && MaTieMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), f.PendingMovementContinuation) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && (r.Stage != AnniversaryMaTieStage.HpChildren || r.HpPaid && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == r.HpTarget && hp.Change.Amount == r.Used.Count);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool MaTieNativeEdge(int i)
    {
        if (_resolutionStackOperations._resolutionStack[i - 1] is CardUseFrame { AnniversaryMaTieOrigin: { } n } use && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } parent && MaTieNativeMatches(use, parent) && _resolutionStackOperations._resolutionStack[i] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == use.Id && w.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[i - 1] is ProgramCardTriggerWindowFrame window && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { AnniversaryMaTieOrigin: not null } && _resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(window, window.Candidates[window.CandidateIndex]);
        return false;
    }

    internal bool MaTieSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _characterMaTieAnniversaryOperations.MaTieNativeEdge(i) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame f && _characterMaTieAnniversaryOperations.MaTieFirstChild(f, _resolutionStackOperations2._resolutionStack()[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i, dying));
    internal ProgramSkillFrame? MaTieObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterMaTieAnniversaryOperations.MaTieFirstChild(f, child), (i) => _characterMaTieAnniversaryOperations.MaTieSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 46700)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 46700)]
    internal bool MaTieDamage(long id) => MaTieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 46700)]
    internal bool MaTieBeforeDamage(long id) => MaTieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 46700)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 46700)]
    internal bool MaTieDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MaTieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 46700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 46700)]
    internal bool MaTieRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidMaTie(f))
            return false;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return a >= 0 && b > a && MaTieFirstChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && (b == a + 1 || MaTieSuffix(a)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46700)]
    internal bool RetainMaTie(ProgramSkillFrame f) => f.AnniversaryMaTie is not null && ValidMaTie(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 46700)]
    internal bool ResumeMaTie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaTie: not null } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        ContinueMaTie(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46700)]
    internal bool ReturnMaTieMovement(ProgramSkillFrame f) => f.AnniversaryMaTie is not null && f.PendingMovementContinuation is not null && ResumeMaTie(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46700)]
    internal bool AdvanceMaTieChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaTieObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
