#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiWenJiLeRuntime
{
    internal const string BeifenObtainReason = "skill-program.le-cai-wen-ji.beifen.obtain";
    internal bool HasLeCaiWenJiBeifenCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBeifenLostMarkedSuitGains);

    internal static CardConversionSource BeifenSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool IsBeifenLossTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveBeifenLostMarkedSuitGains }];
    internal IReadOnlyList<BeifenLossSource> CaptureBeifenLossSources(IEnumerable<CardLocation> locations)
    {
        if (!HasLeCaiWenJiBeifenCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete)
            return Array.AsReadOnly(Array.Empty<BeifenLossSource>());
        return Array.AsReadOnly(locations.Where(l => l.Zone is CardZoneKind.Hand or CardZoneKind.Equipment).Where(l => l.OwnerSeat is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)).Select(l => l.OwnerSeat!.Value).Distinct().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).SelectMany(seat => _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.SelectMany(instance => instance.Program.Triggers.Where(IsBeifenLossTrigger).Select(trigger => new BeifenLossSource(new(instance.SkillId, trigger.Id, seat, instance.SkillInstanceId), instance.Program.GameplayHash))).OrderBy(s => s.Source.SkillInstanceId, StringComparer.Ordinal).Take(1)).OrderBy(s => s.Source.OwnerSeat).ThenBy(s => s.Source.SkillInstanceId, StringComparer.Ordinal).ToArray());
    }

    internal bool IsBeifenOwnedLoss(CardMovementRecord m, int owner) => m.From.OwnerSeat == owner && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !(m.To.OwnerSeat == owner && m.To.Zone is CardZoneKind.Hand or CardZoneKind.Equipment) && HasShuangjiaEntityMark(m.CardId);
    internal void CaptureBeifenLossQualifications(CardMovementBatchContext batch, IReadOnlyList<BeifenLossSource> sources)
    {
        if (!HasLeCaiWenJiBeifenCapability || batch.Movements.Count == 0)
            return;
        foreach (var source in sources)
            foreach (var m in batch.Movements.Where(m => IsBeifenOwnedLoss(m, source.Source.OwnerSeat)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenLossQualifiedEvent(batch.Id, source.Source, source.GameplayHash, m.CardId, m.From, m.To, m.Sequence, batch.TurnNumber));
    }

    internal bool ExactBeifenLoss(CardMovementBatchContext batch, CardConversionSource source, string hash)
    {
        if (source.SkillId != "le:beifen" || source.BindingId != "lost-hujia-suits" || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash != hash)
            return false;
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<BeifenLossQualifiedEvent>().Where(e => e.BatchId == batch.Id && e.Source == source).ToArray();
        var actual = batch.Movements.Where(m => IsBeifenOwnedLoss(m, source.OwnerSeat)).ToArray();
        return facts.Length > 0 && facts.Length == actual.Length && facts.Select(e => e.MovementSequence).Distinct().Count() == facts.Length && actual.All(m => facts.Count(e => e.GameplayHash == hash && e.CardId == m.CardId && e.From == m.From && e.To == m.To && e.MovementSequence == m.Sequence && e.ActualTurn == batch.TurnNumber) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(history => history == m) == 1);
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectBeifenLossCandidates(CardMovementBatchContext batch) => !HasLeCaiWenJiBeifenCapability ? [] : Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<BeifenLossQualifiedEvent>().Where(e => e.BatchId == batch.Id).Select(e => new BeifenLossSource(e.Source, e.GameplayHash)).Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s.Source.OwnerSeat].IsAlive && ExactBeifenLoss(batch, s.Source, s.GameplayHash) && !_coreProgramEventHistoryOperations.ProgramEventHistory<BeifenStartedEvent>().Any(e => e.LossBatchId == batch.Id && e.Source == s.Source)).Select(s => new ProgramTriggerCandidate(s.Source.OwnerSeat, s.Source.SkillId, s.Source.BindingId, s.Source.SkillInstanceId, s.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger2(new ProgramTriggerCandidate(s.Source.OwnerSeat, s.Source.SkillId, s.Source.BindingId, s.Source.SkillInstanceId, s.GameplayHash, 0)).Priority)).ToArray());
    internal ProgramSkillWindowContext CreateBeifenMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsMoved, window.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: window.Batch, OccurrenceIndex: candidate.OccurrenceIndex);
    internal bool BeifenMotherMatches(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == "le:beifen" && c.BindingId == "lost-hujia-suits" && c.OccurrenceIndex == 0 && context.Window == SkillProgramTriggerWindow.CardsMoved && context.OwnerSeat == c.OwnerSeat && context.SourceSeat == c.OwnerSeat && context.TargetSeat == c.OwnerSeat && context.OccurrenceIndex == 0 && context.MovementBatch is { } batch && context.ParentFrameId == batch.Id && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == batch.Id)is CardsMovedTriggerWindowFrame window && window.Batch == batch && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == c && ExactBeifenLoss(batch, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash);
    [ProgramIssuedTrigger(220)]
    internal bool IsIssuedBeifenCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => HasLeCaiWenJiBeifenCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && BeifenMotherMatches(c, context) && !_coreProgramEventHistoryOperations.ProgramEventHistory<BeifenStartedEvent>().Any(e => e.LossBatchId == context.ParentFrameId && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28420, SkillProgramEffectOp.ResolveBeifenLostMarkedSuitGains)]
    internal bool CanOfferBeifen(ProgramTriggerLegalityContext gate) => IsIssuedBeifenCandidate(gate.Candidate, gate.Context);
    SkillProgramStepOutcome ILeCaiWenJiBeifenHost.Resolve(ProgramSkillFrame frame) => BeginBeifen(frame);
    internal SkillProgramStepOutcome BeginBeifen(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Beifen is not null)
            throw new InvalidOperationException("Beifen cannot issue the same loss obtain twice.");
        if (f.WindowContext is not { } context || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.ActivationId != "lost-hujia-suits" || f.TriggerId != "lost-hujia-suits")
            throw new InvalidOperationException("Beifen requires its actual frozen original loss window.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (!IsIssuedBeifenCandidate(candidate, context))
            throw new InvalidOperationException("Beifen lost its exact original source and marked HE loss qualification.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => HasShuangjiaEntityMark(c.Id)).ToArray();
        var heldSuits = hand.Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c)).ToHashSet();
        var search = CharacterCaiWenJiLeDefinitions.BeifenStandardSuits.Where(s => !heldSuits.Contains(s)).Select(s => new BeifenSearchedEntity(s, _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(c => c.Suit == s && !c.IsGeneralWeapon)?.Id)).ToArray();
        var r = new BeifenReceipt(BeifenSource(f), f.GameplayHash, context.ParentFrameId, f.InstructionIndex, hand.Length, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Stage = BeifenStage.ObtainChildren,
            SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Search = search
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Beifen = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenStartedEvent(f.Id, r.Source, r.GameplayHash, r.LossBatchId, r.MarkedHandCount, search.Length, r.SequenceBefore));
        foreach (var entity in search)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenSuitSearchedEvent(f.Id, entity.Suit, entity.CardId));
        var ids = search.Where(e => e.CardId is not null).Select(e => e.CardId!.Value).ToArray();
        if (ids.Length > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(BeifenObtainReason), (id, movements) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { Beifen = current.Beifen!with { ObtainBatchId = id, SequenceAfter = movements[^1].Sequence } });
                foreach (var movement in movements)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenObtainedEvent(f.Id, movement.CardId, movement.CardKind, search.Single(e => e.CardId == movement.CardId).Suit, movement.Sequence, id));
            });
        ContinueBeifen(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueBeifen(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Beifen: not null } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TryStartPostInstructionWindow(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenCompletedEvent(id, f.Beifen!.LossBatchId, _coreProgramEventHistoryOperations.ProgramEventHistory<BeifenObtainedEvent>().Count(e => e.FrameId == id)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28420)]
    internal bool ResumeBeifen(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Beifen: not null } f || f.Id != id)
            return false;
        ContinueBeifen(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28420)]
    internal bool ReturnBeifenMovement(ProgramSkillFrame f)
    {
        if (f.Beifen is null)
            return false;
        ContinueBeifen(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28420)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28420)]
    internal bool ContinueIssuedBeifen(ProgramSkillFrame f)
    {
        if (f.SkillId != "le:beifen" || f.TriggerId != "lost-hujia-suits" || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            return false;
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (!BeifenMotherMatches(c, context))
            return false;
        return f.Beifen is not null ? ValidBeifenReceipt(f) : IsIssuedBeifenCandidate(c, context);
    }

    internal bool ValidBeifenReceipt(ProgramSkillFrame f)
    {
        if (f.Beifen is not { } r || f.SkillId != "le:beifen" || f.TriggerId != "lost-hujia-suits" || f.ActivationId != f.TriggerId || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.Source != BeifenSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Stage != BeifenStage.ObtainChildren || f.WindowContext?.ParentFrameId != r.LossBatchId || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Search is not System.Collections.IList { IsReadOnly: true } || r.Search.Count > 4 || r.Search.Select(e => e.Suit).Distinct().Count() != r.Search.Count || r.Search.Any(e => !CharacterCaiWenJiLeDefinitions.BeifenStandardSuits.Contains(e.Suit)) || _coreProgramEventHistoryOperations.ProgramEventHistory<BeifenStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started.Source != r.Source || started.GameplayHash != r.GameplayHash || started.LossBatchId != r.LossBatchId || started.MarkedHandCount != r.MarkedHandCount || started.MissingSuitCount != r.Search.Count || started.SequenceBefore != r.SequenceBefore || !_coreProgramEventHistoryOperations.ProgramEventHistory<BeifenSuitSearchedEvent>().Where(e => e.FrameId == f.Id).Select(e => new BeifenSearchedEntity(e.Suit, e.CardId)).SequenceEqual(r.Search))
            return false;
        var obtained = _coreProgramEventHistoryOperations.ProgramEventHistory<BeifenObtainedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.ObtainBatchId == 0)
            return obtained.Length == 0 && r.SequenceAfter == r.SequenceBefore;
        var chosen = r.Search.Where(e => e.CardId is not null).ToArray();
        return obtained.Length == chosen.Length && obtained.Select(e => e.CardId).Distinct().Count() == chosen.Length && chosen.All(e => obtained.Count(o => o.CardId == e.CardId && o.Suit == e.Suit) == 1) && obtained.All(e => e.BatchId == r.ObtainBatchId && e.MovementSequence > r.SequenceBefore && e.MovementSequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BeifenObtainReason) == 1);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1800)]
    internal void CollectWindow_CollectBeifenLossCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectBeifenLossCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1500)]
    internal bool ExcludeOrdinaryMovement_IsBeifenLossTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsBeifenLossTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1700)]
    internal ProgramSkillWindowContext? TryCreateBeifenMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsBeifenLossTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateBeifenMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 500)]
    internal void CaptureMovementBatchStartBeifenLossSources(CardMovementBatchBuilder batch) => batch.BeifenLossSources = CaptureBeifenLossSources(batch.SourceCountsBefore.Keys);
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 3100)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureBeifenLossQualifications(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureBeifenLossQualifications(completed, batch.BeifenLossSources);
        return completed;
    }
}
