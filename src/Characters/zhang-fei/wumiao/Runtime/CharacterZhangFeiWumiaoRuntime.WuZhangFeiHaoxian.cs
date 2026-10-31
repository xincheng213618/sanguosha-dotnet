#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiWumiaoRuntime
{
    internal const string WuHaoxianSkill = "wumiao:haoxian";
    internal const string WuHaoxianBinding = "collect-rank-three";
    internal const string WuHaoxianRecycleReason = "skill-program.wumiao-zhang-fei.haoxian.recycle";
    internal const string WuHaoxianObtainReason = "skill-program.wumiao-zhang-fei.haoxian.obtain";
    internal static CardConversionSource WuHaoxianSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_fei_wumiao_WuZhangFeiHaoxian.WuHaoxianSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IWuZhangFeiHaoxianHost.ResolveHaoxian(ProgramSkillFrame frame) => BeginWuHaoxian(frame);
    internal bool HasWuHaoxianOriginalPayment(ProgramSkillFrame f)
    {
        if (f.SkillId != WuHaoxianSkill || f.ActivationId != WuHaoxianBinding || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var starts = history.Select((e, i) => (e, i)).Where(x => x.e is ProgramSkillStartedEvent s && s.FrameId == f.Id && s.OwnerSeat == f.OwnerSeat && s.SkillId == f.SkillId && s.ActivationId == f.ActivationId).ToArray();
        return starts is [var start] && history.Take(start.i).OfType<SkillUsageConsumedEvent>().LastOrDefault(e => e.SkillOwnerSeat == f.OwnerSeat && e.SkillId == WuHaoxianSkill && e.UsageId == WuHaoxianBinding && e.Scope == SkillUsageScope.Game)is { Count: 1 };
    }

    internal SkillProgramStepOutcome BeginWuHaoxian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!HasWuHaoxianOriginalPayment(f) || f.WuHaoxian is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianStartedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Haoxian requires its exact once-paid native limited activation.");
        var materials = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => c.Rank == 3).Select(c => new WuHaoxianMaterial(c.Id, c.Kind, CardLocation.DiscardPile, c.Rank)).ToArray();
        var r = new WuHaoxianReceipt(WuHaoxianSource(f), f.GameplayHash, WuHaoxianStage.Recycle, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            RecycleMaterials = materials
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHaoxian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHaoxianStartedEvent(f.Id, r.Source, r.GameplayHash, r.Before));
        for (var i = 0; i < materials.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHaoxianMaterialFrozenEvent(f.Id, true, i, materials[i]));
        ContinueWuHaoxian(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueWuHaoxian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuHaoxian: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        RequireWuHaoxian(f);
        if (r.Stage == WuHaoxianStage.Recycle)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHaoxian = r with { Stage = WuHaoxianStage.RecycleChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (r.RecycleMaterials.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.RecycleMaterials.Select(m => m.CardId).ToArray(), CardLocation.DrawPile, new(WuHaoxianRecycleReason), (batchId, moves) => SealWuHaoxianMovement(id, true, batchId, moves), disclosure: CardMovementDisclosure.FaceDown);
            else
                SealWuHaoxianMovement(id, true, null, []);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.WuHaoxian!;
            // No discarded card means no shuffle insertion; the later Hand
            // acquisition still runs and the limited activation stays paid.
            if (r.RecycleMaterials.Count > 0)
            {
                var before = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
                var random = _randomOperations._random.State;
                _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
                var after = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHaoxian = r with { Shuffled = true } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHaoxianShuffledEvent(id, random, _randomOperations._random.State) { BeforeOrder = before, AfterOrder = after });
            }
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WuHaoxian!;
        if (r.Stage == WuHaoxianStage.RecycleChildren)
        {
            if (AwaitWuHaoxianChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.WuHaoxian!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteWuHaoxian(f);
                return;
            }

            var materials = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, p.Seat)).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Where(c => _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(p, c) == 3).Select(c => new WuHaoxianMaterial(c.Id, c.Kind, CardLocation.Hand(p.Seat), _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(p, c)))).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHaoxian = r with { Stage = WuHaoxianStage.Obtain, ObtainMaterials = materials, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            for (var i = 0; i < materials.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHaoxianMaterialFrozenEvent(id, false, i, materials[i]));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WuHaoxian!;
        if (r.Stage == WuHaoxianStage.Obtain)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHaoxian = r with { Stage = WuHaoxianStage.ObtainChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (r.ObtainMaterials.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.ObtainMaterials.Select(m => m.CardId).ToArray(), CardLocation.Hand(f.OwnerSeat), new(WuHaoxianObtainReason), (batchId, moves) => SealWuHaoxianMovement(id, false, batchId, moves), disclosure: CardMovementDisclosure.FaceDown);
            else
                SealWuHaoxianMovement(id, false, null, []);
        }

        if (AwaitWuHaoxianChildren(id))
            return;
        CompleteWuHaoxian(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void SealWuHaoxianMovement(long id, bool recycle, long? batchId, IReadOnlyList<CardMovementRecord> moves)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.WuHaoxian!;
        var expected = recycle ? r.RecycleMaterials : r.ObtainMaterials;
        var destination = recycle ? CardLocation.DrawPile : CardLocation.Hand(f.OwnerSeat);
        if (moves.Count != expected.Count || !moves.Select(m => m.CardId).SequenceEqual(expected.Select(m => m.CardId)) || moves.Where((m, i) => m.From != expected[i].From || m.CardKind != expected[i].PrintedKind || m.To != destination || m.Reason.Value != (recycle ? WuHaoxianRecycleReason : WuHaoxianObtainReason)).Any())
            throw new InvalidOperationException("Haoxian must move the exact real rank-three entities once.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianMovementPaidEvent>().Any(e => e.FrameId == id && e.Recycle == recycle))
            throw new InvalidOperationException("Haoxian cannot pay one movement twice.");
        var before = recycle ? _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianStartedEvent>().Single(e => e.FrameId == id).Before : r.Before;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuHaoxian = recycle ? r with { RecycleBatchId = batchId, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } : r with { ObtainBatchId = batchId, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHaoxianMovementPaidEvent(id, recycle, batchId, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, moves.Count));
    }

    internal bool AwaitWuHaoxianChildren(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        return false;
    }

    internal void RequireWuHaoxian(ProgramSkillFrame f)
    {
        if (f.WuHaoxian is not { } r || !HasWuHaoxianOriginalPayment(f) || r.Source != WuHaoxianSource(f) || r.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash) != 1 || r.RecycleMaterials.Select(m => m.CardId).Distinct().Count() != r.RecycleMaterials.Count || r.ObtainMaterials.Select(m => m.CardId).Distinct().Count() != r.ObtainMaterials.Count || r.RecycleMaterials.Any(m => m.From != CardLocation.DiscardPile || m.EffectiveRank != 3 || _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Rank != 3) || r.ObtainMaterials.Any(m => m.From.Zone != CardZoneKind.Hand || m.From.OwnerSeat is null || m.From.OwnerSeat == f.OwnerSeat || m.EffectiveRank != 3))
            throw new InvalidOperationException("Haoxian lost its native limited payment, exact source or frozen real rank-three roster.");
        foreach (var paid in _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianMovementPaidEvent>().Where(e => e.FrameId == f.Id))
        {
            var materials = paid.Recycle ? r.RecycleMaterials : r.ObtainMaterials;
            var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.Reason.Value == (paid.Recycle ? WuHaoxianRecycleReason : WuHaoxianObtainReason)).ToArray();
            if (paid.Count != materials.Count || moves.Length != materials.Count || !moves.Select(m => m.CardId).SequenceEqual(materials.Select(m => m.CardId)) || moves.Where((m, i) => m.From != materials[i].From || m.To != (paid.Recycle ? CardLocation.DrawPile : CardLocation.Hand(f.OwnerSeat))).Any())
                throw new InvalidOperationException("Haoxian's committed movement no longer matches its frozen primary materials.");
        }

        var shuffle = _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianShuffledEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Shuffled != (shuffle.Length == 1) || shuffle.Length > 1)
            throw new InvalidOperationException("Haoxian's shuffle must happen once.");
        if (shuffle is [var s])
        {
            var order = s.BeforeOrder.ToList();
            var random = new DeterministicRandom(unchecked((int)s.RandomBefore));
            random.Shuffle(order);
            if (random.State != s.RandomAfter || !order.SequenceEqual(s.AfterOrder) || s.BeforeOrder.Distinct().Count() != s.BeforeOrder.Count || !s.BeforeOrder.Order().SequenceEqual(s.AfterOrder.Order()) || r.RecycleMaterials.Any(m => !s.BeforeOrder.Contains(m.CardId)))
                throw new InvalidOperationException("Haoxian's actual shuffle lost its exact deck partition or deterministic RNG.");
        }
    }

    internal void CompleteWuHaoxian(ProgramSkillFrame f)
    {
        RequireWuHaoxian(f);
        var r = f.WuHaoxian!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHaoxianCompletedEvent(f.Id, r.RecycleMaterials.Count, _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianMovementPaidEvent>().SingleOrDefault(e => e.FrameId == f.Id && !e.Recycle)?.Count ?? 0, r.Shuffled));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHaoxian = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29063)]
    internal bool ResumeWuHaoxian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuHaoxian: not null } f || f.Id != id)
            return false;
        ContinueWuHaoxian(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29063)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29063)]
    internal bool ContinueIssuedWuHaoxian(ProgramSkillFrame f) => f.WuHaoxian is not null && HasWuHaoxianOriginalPayment(f);
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 29063)]
    internal void AssertWuHaoxianFrames()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.WuHaoxian is not null))
            RequireWuHaoxian(f);
    }

    internal bool WuHaoxianFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuHaoxian is not { Stage: WuHaoxianStage.RecycleChildren or WuHaoxianStage.ObtainChildren } r || f.PendingMovementContinuation is null || !HasWuHaoxianOriginalPayment(f))
            return false;
        RequireWuHaoxian(f);
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is not CardsMovedTriggerWindowFrame moved || !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId)
            return false;
        var recycle = r.Stage == WuHaoxianStage.RecycleChildren;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<WuHaoxianMovementPaidEvent>().SingleOrDefault(e => e.FrameId == f.Id && e.Recycle == recycle);
        return paid is not null && paid.BatchId == moved.Batch.Id && moved.Batch.Movements.Count == paid.Count && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > paid.Before && m.Sequence <= paid.After && m.Reason.Value == (recycle ? WuHaoxianRecycleReason : WuHaoxianObtainReason));
    }
}
