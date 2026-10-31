#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaMiaoAnniversaryRuntime
{
    internal bool MaMiaoOriginalParent(ProgramSkillFrame f)
    {
        if (f.AnniversaryMaMiao is not { } r || f.WindowContext is not { } w || r.ParentId != w.ParentFrameId)
            return false;
        if (r.Mode == "congfeng")
            return f.SkillId == AnniversaryMaMiaoComposition.Congfeng && MaMiaoGainQualification(MaMiaoCandidate(f), w)is { } e && r.Holder == e.Holder && r.Recipient == e.Recipient;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return r.Mode == "zhangguan" && f.SkillId == AnniversaryMaMiaoComposition.Zhangguan && w.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && r.Holder == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && r.Recipient == f.OwnerSeat && i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Id == r.ParentId && parent.Window == w.Window && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && parent.Continuation == ProgramLifecycleContinuation.NormalTurnStart && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]);
    }

    internal bool ValidMaMiao(ProgramSkillFrame f)
    {
        if (f.AnniversaryMaMiao is not { } r || r.Source != MaMiaoSource(f) || r.Hash != f.GameplayHash || f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (r.Mode == "zhangguan" ? SkillProgramEffectOp.ResolveAnniversaryMaMiaoZhangguan : SkillProgramEffectOp.ResolveAnniversaryMaMiaoCongfeng) || !MaMiaoOriginalParent(f) || r.Stage is not ("adjust" or "drawing" or "discard" or "adjust-children" or "after-adjust" or "recovery-children" or "obtain-choice" or "extra-choice" or "gain-children") || r.InitialHand < 0 || r.AdjustmentHp < 0 || r.Count < 0 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Distinct().Count() != r.Cards.Count || r.Cards.Count > r.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoStartedEvent>().Count(e => e == new AnniversaryMaMiaoStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Mode, r.Holder, r.Recipient, r.InitialHand, r.AdjustmentHp)) != 1)
            return false;
        if (r.Stage == "extra-choice")
            return r.Mode == "congfeng" && r.RandomPool.Count == 0 && r.Gain is null && f.PendingMovementContinuation is null;
        if (r.Mode == "zhangguan" && r.Stage is "recovery-children" or "obtain-choice" or "gain-children")
            if (r.HandAfter < 0 || r.HandAfter > r.InitialHand || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoAdjustedEvent>().Count(e => e == new AnniversaryMaMiaoAdjustedEvent(f.Id, r.InitialHand, r.HandAfter, r.AdjustmentHp)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoRecoveryIssuedEvent>().Count(e => e == new AnniversaryMaMiaoRecoveryIssuedEvent(f.Id, f.OwnerSeat, 1)) != 1)
                return false;
        if (r.Stage == "gain-children" && r.Mode == "congfeng")
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoDecisionEvent>().Count(e => e.FrameId == f.Id && e.Chooser == r.Recipient && e.Accepted) != 1 || r.RandomPool.Count == 0 || r.RandomPool.Distinct().Count() != r.RandomPool.Count || r.RandomIndex < 0 || r.RandomIndex >= r.RandomPool.Count || r.RandomPool[r.RandomIndex] != new AnniversaryMaMiaoEntity(r.CardId ?? 0, r.From ?? CardLocation.Processing))
                return false;
            var random = new DeterministicRandom(unchecked((int)r.RandomBefore));
            if (random.Next(r.RandomPool.Count) != r.RandomIndex || random.State != r.RandomAfter || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoRandomEvent>().Count(e => e == new AnniversaryMaMiaoRandomEvent(f.Id, r.Recipient, r.Holder, r.CardId!.Value, r.From!.Value, r.RandomBefore, r.RandomAfter, r.RandomIndex, r.RandomPool.Count)) != 1)
                return false;
        }

        if (r.Stage is "adjust-children" or "gain-children")
        {
            if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat)
                return false;
            if (r.Stage == "gain-children")
            {
                if (r.CardId is not { } id || r.From is not { } from || from.OwnerSeat != r.Holder || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.Gain is not { } gain || gain.Reason != MaMiaoGainReason(f))
                    return false;
                _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(gain, f.Id, [id], CardLocation.Hand(r.Recipient), r.Before);
                return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoMovementEvent>().Count(e => e == new AnniversaryMaMiaoMovementEvent(f.Id, "gain", 1, r.Before, r.After)) == 1 && gain.Movements.All(m => m.CardId == id && m.From == from && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
            }

            var draw = r.Draw is not null;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMaMiaoMovementEvent>().Count(e => e == new AnniversaryMaMiaoMovementEvent(f.Id, draw ? "draw" : "discard", r.Count, r.Before, r.After)) != 1)
                return false;
            return draw ? r.Draw!.RecipientSeat == f.OwnerSeat && r.Draw.RequestedCount == r.Count && r.Draw.SequenceBefore == r.Before && r.Draw.SequenceAfter == r.After : r.Cards.Count == r.Count && r.Cards.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == MaMiaoDiscardReason && m.Sequence > r.Before && m.Sequence <= r.After) == 1);
        }

        return f.PendingMovementContinuation is null;
    }
    internal bool MaMiaoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryMaMiao is { Stage: "adjust-children" or "gain-children" } && e?.Op is SkillProgramEffectOp.ResolveAnniversaryMaMiaoZhangguan or SkillProgramEffectOp.ResolveAnniversaryMaMiaoCongfeng && p == f.PendingMovementContinuation && ValidMaMiao(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 48800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 48800)]
    internal bool RetainMaMiao(ProgramSkillFrame f) => f.AnniversaryMaMiao is { Stage: "adjust-children" or "gain-children" or "recovery-children" } && ValidMaMiao(f);
    internal bool MaMiaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryMaMiao is not { } r || !ValidMaMiao(f))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == "drawing" && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && MaMiaoMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == (r.Stage == "recovery-children" ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == (r.Stage == "recovery-children" ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool MaMiaoObserverEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MaMiaoFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal ProgramSkillFrame? MaMiaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterMaMiaoAnniversaryOperations.MaMiaoFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterMaMiaoAnniversaryOperations.MaMiaoObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 48800)]
    internal bool MaMiaoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MaMiaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 48800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 48800)]
    internal bool MaMiaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryMaMiao is null)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && j > i && MaMiaoFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && (j == i + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 48800)]
    internal bool AdvanceMaMiaoChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaMiaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
