#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiLeRuntime
{
    internal bool HasLeZhouFeiLingkongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeLingkongInitialEntities) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain);

    internal static CardConversionSource LingkongSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_fei_le_LeZhouFeiLingkong.LingkongSource(f);
    internal static bool IsLingkongGainTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_fei_le_LeZhouFeiLingkong.IsLingkongGainTrigger(t);
    // Registration only; the query still belongs to legacy GameEngine.LeZhouFeiLingkong.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 110)]
    internal void ProjectLingkongCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasLingkongEntityMark(cardId) ? "箜篌" : null);
    }

    internal bool HasLingkongEntityMark(int cardId) => HasLeZhouFeiLingkongCapability && (_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongInitialEntityMarkedEvent>().Any(e => e.CardId == cardId) || _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainEntityMarkedEvent>().Any(e => e.CardId == cardId));
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasLingkongEntityMark(cardId);
    internal bool IsLingkongHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:lingkong") && HasLingkongHandEntityMark(owner.Seat, cardId);
    internal ProgramLifecycleTriggerWindowFrame CaptureLingkongInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasLeZhouFeiLingkongCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.LingkongInitialEntities is not null)
            throw new InvalidOperationException("Lingkong's actual opening Hand can only be frozen once.");
        var owners = window.Candidates.Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.InitializeLingkongInitialEntities }]).Select(c => c.OwnerSeat).Distinct().Order();
        var entities = owners.SelectMany(owner => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(c =>
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.Id && m.To == CardLocation.Hand(owner));
            if (entry is null || entry.CardKind != c.Kind)
                throw new InvalidOperationException("A Lingkong initial entity requires its actual opening-Hand entry.");
            return new LingkongInitialEntity(owner, c.Id, c.Kind, entry.Sequence);
        })).ToArray();
        return window with
        {
            LingkongInitialEntities = entities
        };
    }

    internal CardMovementBatchContext CaptureLingkongGainBatch(CardMovementBatchContext batch)
    {
        if (!HasLeZhouFeiLingkongCapability || batch.TurnNumber <= 0 || batch.MovementTiming is not { } timing || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(timing.ActualTurnOwnerSeat))
            return batch;
        var captures = new List<LingkongGainCapture>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.OrderBy(p => p.Seat))
        {
            if (timing.Phase == TurnPhase.Draw && timing.PhaseActorSeat == owner.Seat)
                continue;
            var gains = _coreActualHandGainProgramsOperations.MatchingActualHandGainIndexes(batch, owner.Seat, CardLocation.Hand(owner.Seat)).Select(i => batch.Movements[i]).OrderBy(m => m.Sequence).ToArray();
            if (gains.Length == 0)
                continue;
            var first = !_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongOutsideGainObservedEvent>().Any(e => e.RecipientSeat == owner.Seat && e.ActualTurn == batch.TurnNumber && e.TurnOwner == timing.ActualTurnOwnerSeat) && !_pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Any(p => p.LingkongGains?.Any(r => r.First && r.RecipientSeat == owner.Seat && r.ActualTurn == batch.TurnNumber && r.TurnOwner == timing.ActualTurnOwnerSeat) == true);
            var candidate = !first || !owner.IsAlive ? null : _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained).Where(c => IsLingkongGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, c.SkillId, c.SkillInstanceId)).OrderByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
            captures.Add(new(owner.Seat, batch.TurnNumber, timing.ActualTurnOwnerSeat, timing.Phase, timing.PhaseActorSeat, first, candidate is null ? null : new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate?.GameplayHash) { Entities = gains.Select(m => new LingkongGainEntity(m.CardId, m.CardKind, m.Sequence)).ToArray() });
        }

        return captures.Count == 0 ? batch : batch with
        {
            LingkongGains = captures
        };
    }

    internal void PrepareLingkongGainBatch(CardMovementBatchContext batch)
    {
        if (!HasLeZhouFeiLingkongCapability || batch.LingkongGains is not { } captures)
            return;
        foreach (var r in captures.Where(r => r.First))
        {
            var prior = _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongOutsideGainObservedEvent>().Where(e => e.RecipientSeat == r.RecipientSeat && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner).ToArray();
            if (prior.Length != 0)
            {
                if (prior is not [var existing] || existing.BatchId != batch.Id)
                    throw new InvalidOperationException("Lingkong cannot replace an earlier actual acquisition with a later prepared draw.");
                continue;
            }

            if (r.Entities.Count == 0 || r.Entities.Select(e => e.CardId).Distinct().Count() != r.Entities.Count)
                throw new InvalidOperationException("Lingkong cannot consume first acquisition with empty or repeated entities.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongOutsideGainObservedEvent(batch.Id, r.RecipientSeat, r.ActualTurn, r.TurnOwner, r.Phase, r.PhaseActorSeat, r.Entities.Count, r.Entities[0].GainSequence - 1, r.Entities[^1].GainSequence));
            if (r.Source is not { } source || r.GameplayHash is not { } hash)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainQualifiedEvent(batch.Id, source, hash, r.ActualTurn, r.TurnOwner, r.Entities.Count));
            foreach (var entity in r.Entities)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainEntityIssuedEvent(batch.Id, r.RecipientSeat, entity.CardId, entity.PrintedKind, entity.GainSequence));
        }
    }

    internal void CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof)
    {
        if (!HasLeZhouFeiLingkongCapability)
            return;
        if (!_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof))
            throw new InvalidOperationException("Lingkong requires the real completed native DrawCards invoice.");
        MergeLingkongGainInvocation(proof.ParentFrameId, proof.OwnerSeat, proof.Materials.Select(m => new LingkongGainEntity(m.CardId, m.PrintedKind, checked((int)m.MovementSequence))).ToArray(), proof.LastBatchId, proof.InvocationId, null);
    }

    internal void CompleteLingkongNativeDrawReplacement(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement)
    {
        if (!HasLeZhouFeiLingkongCapability || replacement.GainedIds.Count == 0)
            return;
        if (_resolutionStackOperations._resolutionStack.OfType<NativeDrawAttemptFrame>().SingleOrDefault(a => a.Id == attempt.Id)is not { } current || current.Context != attempt.Context || current.Replacement != replacement || frame.WindowContext?.NativeDraw != attempt.Context || replacement.OwnerSeat != attempt.Context.RecipientSeat || replacement.GainedIds.Count > attempt.Context.RequestedCount)
            throw new InvalidOperationException("Lingkong's replacement gain lost the exact native paid request.");
        var entities = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > replacement.SequenceBefore && m.Sequence <= replacement.SequenceAfter && m.To == CardLocation.Hand(attempt.Context.RecipientSeat) && m.Reason == replacement.Reason && replacement.GainedIds.Contains(m.CardId)).OrderBy(m => m.Sequence).ToArray();
        if (!entities.Select(m => m.CardId).SequenceEqual(replacement.GainedIds) || entities.Any(m => m.From.OwnerSeat != replacement.SourceSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            throw new InvalidOperationException("Lingkong cannot include unrelated child gains in a native draw replacement.");
        var lastBatch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.ParentFrameId == frame.Id && b.Movements.Contains(entities[^1])) ?? throw new InvalidOperationException("Lingkong lost the replacement's real final movement batch.");
        MergeLingkongGainInvocation(frame.Id, attempt.Context.RecipientSeat, entities.Select(m => new LingkongGainEntity(m.CardId, m.CardKind, m.Sequence)).ToArray(), lastBatch.Id, null, attempt.Id);
    }

    internal void MergeLingkongGainInvocation(long? parent, int recipient, IReadOnlyList<LingkongGainEntity> entities, long lastBatchId, long? invocationId, long? attemptId)
    {
        var relevant = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == parent && b.LingkongGains?.Any(r => r.RecipientSeat == recipient && r.Entities.Any(entities.Contains)) == true).ToArray();
        if (relevant.Length == 0)
            return; // The recipient's own actual Draw phase was excluded at capture.
        var captures = relevant.SelectMany(b => b.LingkongGains!).Where(r => r.RecipientSeat == recipient && r.Entities.Any(entities.Contains)).ToArray();
        if (captures.Any(r => r.Entities.Any(e => !entities.Contains(e))) || !captures.SelectMany(r => r.Entities).OrderBy(e => e.GainSequence).SequenceEqual(entities))
            throw new InvalidOperationException("Lingkong's whole gain cannot merge a different native acquisition or child batch.");
        var firsts = captures.Where(r => r.First).ToArray();
        if (firsts.Length == 0)
            return; // An earlier ordinary pending gain keeps first rights.
        if (firsts is not [var first] || first.Entities[0] != entities[0] || captures.Any(r => r.ActualTurn != first.ActualTurn || r.TurnOwner != first.TurnOwner || r.Phase != first.Phase || r.PhaseActorSeat != first.PhaseActorSeat))
            throw new InvalidOperationException("Lingkong changed the first actual source or timing while merging its draw materials.");
        for (var i = 0; i < _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Count; i++)
        {
            var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[i];
            if (!relevant.Contains(batch))
                continue;
            var kept = batch.LingkongGains!.Where(r => r.RecipientSeat != recipient || !r.Entities.Any(entities.Contains)).ToList();
            if (batch.Id == lastBatchId)
                kept.Add(first with { Entities = entities, NativeDrawInvocationId = invocationId, NativeDrawAttemptId = attemptId });
            _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[i] = batch with
            {
                LingkongGains = kept.Count == 0 ? null : kept
            };
        }

        var final = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Single(b => b.Id == lastBatchId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainInvocationBoundEvent(lastBatchId, recipient, invocationId, attemptId, parent, entities[0].GainSequence - 1, entities[^1].GainSequence, entities.Count));
        PrepareLingkongGainBatch(final);
    }

    internal bool ExactLingkongGainQualification(CardMovementBatchContext batch, CardConversionSource source, string hash, out LingkongGainQualifiedEvent qualified)
    {
        qualified = null !;
        if (!HasLeZhouFeiLingkongCapability || source.SkillId != "le:lingkong" || source.BindingId != "mark-first-outside-draw-gain" || batch.MovementTiming is not { } timing || _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainQualifiedEvent>().Where(e => e.BatchId == batch.Id && e.Source == source && e.GameplayHash == hash).ToArray()is not [var q] || q.ActualTurn != batch.TurnNumber || q.TurnOwner != timing.ActualTurnOwnerSeat || batch.LingkongGains?.SingleOrDefault(r => r.First && r.RecipientSeat == source.OwnerSeat)is not { } capture || capture.Source != source || capture.GameplayHash != hash || capture.Entities.Count != q.EntityCount || capture.ActualTurn != q.ActualTurn || capture.TurnOwner != q.TurnOwner || _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongOutsideGainObservedEvent>().Where(e => e.RecipientSeat == source.OwnerSeat && e.ActualTurn == q.ActualTurn && e.TurnOwner == q.TurnOwner).ToArray()is not [var first] || first.BatchId != batch.Id || first.EntityCount != q.EntityCount || first.Phase != timing.Phase || first.PhaseActorSeat != timing.PhaseActorSeat || timing.Phase == TurnPhase.Draw && timing.PhaseActorSeat == source.OwnerSeat)
            return false;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainEntityIssuedEvent>().Where(e => e.BatchId == batch.Id && e.RecipientSeat == source.OwnerSeat).OrderBy(e => e.GainSequence).ToArray();
        var actual = capture.Entities;
        if (issued.Length == 0 || issued.Length != q.EntityCount || issued.Length != actual.Count || issued.Select(e => e.CardId).Distinct().Count() != issued.Length || first.SequenceBefore != issued[0].GainSequence - 1 || first.SequenceAfter != issued[^1].GainSequence || issued.Where((e, i) => e.CardId != actual[i].CardId || e.PrintedKind != actual[i].PrintedKind || e.GainSequence != actual[i].GainSequence || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.GainSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(source.OwnerSeat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m, batch) != CardLocation.Hand(source.OwnerSeat))).Any())
            return false;
        if (capture.NativeDrawInvocationId is null && capture.NativeDrawAttemptId is null)
        {
            if (actual.Any(e => !batch.Movements.Any(m => m.CardId == e.CardId && m.Sequence == e.GainSequence)))
                return false;
        }
        else if (_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainInvocationBoundEvent>().Where(e => e.BatchId == batch.Id && e.RecipientSeat == source.OwnerSeat).ToArray()is not [var bound] || bound.ParentFrameId != batch.ParentFrameId || bound.NativeDrawInvocationId != capture.NativeDrawInvocationId || bound.NativeDrawAttemptId != capture.NativeDrawAttemptId || bound.EntityCount != actual.Count || bound.SequenceBefore != first.SequenceBefore || bound.SequenceAfter != first.SequenceAfter || capture.NativeDrawInvocationId is { } drawId && (batch.NativeDrawInvocation is not { } proof || proof.InvocationId != drawId || !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof) || !proof.Materials.Select(m => new LingkongGainEntity(m.CardId, m.PrintedKind, checked((int)m.MovementSequence))).SequenceEqual(actual)))
            return false;
        qualified = q;
        return true;
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectLingkongMovementCandidates(CardMovementBatchContext batch) => !HasLeZhouFeiLingkongCapability ? [] : Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainQualifiedEvent>().Where(e => e.BatchId == batch.Id && ExactLingkongGainQualification(batch, e.Source, e.GameplayHash, out _) && !_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainMarkedEvent>().Any(m => m.BatchId == batch.Id && m.Source.OwnerSeat == e.Source.OwnerSeat)).Select(e => new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger2(new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, 0)).Priority)).ToArray());
    internal ProgramSkillWindowContext CreateLingkongMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsGained, window.Id, candidate.OwnerSeat, SourceSeat: window.Batch.OriginOwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: window.Batch, OccurrenceIndex: candidate.OccurrenceIndex);
    internal bool IsIssuedLingkongGainCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!HasLeZhouFeiLingkongCapability || !IsLingkongGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) || context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat || context.MovementBatch is not { } batch || context.ParentFrameId != batch.Id || context.OccurrenceIndex != 0 || candidate.OccurrenceIndex != 0 || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == batch.Id)is not CardsMovedTriggerWindowFrame window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || window.Batch != batch || context.SourceSeat != batch.OriginOwnerSeat || !ExactLingkongGainQualification(batch, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash, out _) || _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainMarkedEvent>().Any(e => e.BatchId == batch.Id && e.Source.OwnerSeat == candidate.OwnerSeat))
            return false;
        return true;
    }

    internal bool IsIssuedLingkongInitialCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => HasLeZhouFeiLingkongCapability && candidate.SkillId == "le:lingkong" && candidate.BindingId == "mark-initial-entities" && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.InitializeLingkongInitialEntities }] && context.Window == SkillProgramTriggerWindow.GameStarting && context.OwnerSeat == candidate.OwnerSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, Continuation: ProgramLifecycleContinuation.CompleteGameStarting, LingkongInitialEntities: not null } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongInitializedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat);
    [ProgramIssuedTrigger(180)]
    internal bool IsIssuedLingkongCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedLingkongGainCandidate(candidate, context) || IsIssuedLingkongInitialCandidate(candidate, context);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28310, SkillProgramEffectOp.InitializeLingkongInitialEntities, SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain)]
    internal bool CanOfferLingkong(ProgramTriggerLegalityContext gate)
    {
        if (IsLingkongGainTrigger(gate.Trigger))
            return IsIssuedLingkongGainCandidate(gate.Candidate, gate.Context);
        return gate.Context.Window == SkillProgramTriggerWindow.GameStarting && !_coreProgramEventHistoryOperations.ProgramEventHistory<LingkongInitializedEvent>().Any(e => e.Source.OwnerSeat == gate.Candidate.OwnerSeat);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28310)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28310)]
    internal bool PermitIssuedLingkong(ProgramSkillFrame f)
    {
        if (f.SkillId != "le:lingkong" || f.TriggerId is null || f.WindowContext is not { } context || f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != context.ParentFrameId)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (IsIssuedLingkongCandidate(candidate, context))
            return true;
        // The executor checks paid rights once more before closing a completed
        // instruction. Its already-issued marker is not a second entitlement.
        if (f.InstructionIndex != 1)
            return false;
        if (f.TriggerId == "mark-initial-entities")
            return _coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.GameStarting) && _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongInitializedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var initial] && initial.Source == LingkongSource(f) && initial.GameplayHash == f.GameplayHash;
        return context.MovementBatch is { } batch && _resolutionStackOperations._resolutionStack[index - 1] is CardsMovedTriggerWindowFrame window && window.Batch == batch && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate && ExactLingkongGainQualification(batch, LingkongSource(f), f.GameplayHash, out _) && _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainMarkedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var marked] && marked.BatchId == batch.Id && marked.Source == LingkongSource(f) && marked.GameplayHash == f.GameplayHash;
    }

    SkillProgramStepOutcome global::CardGame.Core.ILeZhouFeiLingkongHost.InitializeLingkong(ProgramSkillFrame frame) => InitializeLingkong(frame);
    SkillProgramStepOutcome global::CardGame.Core.ILeZhouFeiLingkongHost.MarkLingkongGain(ProgramSkillFrame frame) => MarkLingkongGain(frame);
    internal SkillProgramStepOutcome InitializeLingkong(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.TriggerId != "mark-initial-entities" || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.GameStarting) || !PermitIssuedLingkong(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongInitializedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Lingkong initialization requires its exact frozen game-start mother once.");
        var parent = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == f.WindowContext!.ParentFrameId);
        var entries = parent.LingkongInitialEntities!.Where(e => e.OwnerSeat == f.OwnerSeat).ToArray();
        if (entries.Select(e => e.CardId).Distinct().Count() != entries.Length || entries.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.DealSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(e.OwnerSeat))))
            throw new InvalidOperationException("Lingkong cannot mark an entity without its original opening-Hand entry.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongInitializedEvent(f.Id, LingkongSource(f), f.GameplayHash, entries.Length));
        foreach (var entity in entries)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongInitialEntityMarkedEvent(f.Id, entity.OwnerSeat, entity.CardId, entity.PrintedKind, entity.DealSequence));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.TriggerId != "mark-first-outside-draw-gain" || !PermitIssuedLingkong(f) || f.WindowContext?.MovementBatch is not { } batch || !ExactLingkongGainQualification(batch, LingkongSource(f), f.GameplayHash, out var qualified) || _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainMarkedEvent>().Any(e => e.BatchId == batch.Id && e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Lingkong requires its issued first outside-own-Draw acquisition and exact original batch.");
        var entries = _coreProgramEventHistoryOperations.ProgramEventHistory<LingkongGainEntityIssuedEvent>().Where(e => e.BatchId == batch.Id && e.RecipientSeat == f.OwnerSeat).OrderBy(e => e.GainSequence).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainMarkedEvent(f.Id, batch.Id, qualified.Source, qualified.GameplayHash, qualified.ActualTurn, qualified.TurnOwner, entries.Length));
        foreach (var entity in entries)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainEntityMarkedEvent(f.Id, batch.Id, entity.RecipientSeat, entity.CardId, entity.PrintedKind, entity.GainSequence));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1700)]
    internal void CollectWindow_CollectLingkongMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectLingkongMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2400)]
    internal bool ExcludeOrdinaryMovement_IsLingkongGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsLingkongGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1600)]
    internal ProgramSkillWindowContext? TryCreateLingkongMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsLingkongGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateLingkongMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 5200)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureLingkongGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureLingkongGainBatch(completed);
}
