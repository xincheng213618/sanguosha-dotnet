#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuLeRuntime
{
    internal const string GuYinDrawReason = "skill-program.le-zhou-yu.guyin.draw";
    internal bool HasGuYinCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeGuYinInitialEntities);

    internal static CardConversionSource GuYinSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_yu_le_LeZhouYuGuYin.GuYinSource(f);
    internal static bool IsGuYinDiscardTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_yu_le_LeZhouYuGuYin.IsGuYinDiscardTrigger(t);
    internal int ApplyGuYinInitialHandSize(CharacterState player, int ordinaryCount)
    {
        if (!HasGuYinCapability)
            return ordinaryCount;
        var sources = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.Where(i => i.SkillId == "le:guyin").Select(i => p.Seat)).ToArray();
        return sources.Contains(player.Seat) ? 0 : (int)Math.Min(int.MaxValue, (long)ordinaryCount + sources.Length);
    }

    internal void CaptureGuYinOpeningOrigins()
    {
        if (!HasGuYinCapability)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<GuYinOpeningCapturedEvent>().Any())
            throw new InvalidOperationException("GuYin global opening identities are captured only once.");
        var entities = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Select(c =>
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.Id && m.To == CardLocation.Hand(p.Seat));
            if (entry is null || entry.CardKind != c.Kind)
                throw new InvalidOperationException("GuYin opening entity needs its actual final opening-Hand entry.");
            return new GuYinInitialEntity(p.Seat, c.Id, c.Kind, entry.Sequence);
        })).ToArray();
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuYinOpeningCapturedEvent(id, entities.Length));
        foreach (var e in entities)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuYinOpeningEntityEvent(id, e.OriginalOwnerSeat, e.CardId, e.PrintedKind, e.EntrySequence));
    }

    internal ProgramLifecycleTriggerWindowFrame CaptureGuYinInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasGuYinCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.GuYinInitialEntities is not null)
            throw new InvalidOperationException("GuYin opening entities are frozen once.");
        return window with
        {
            GuYinInitialEntities = GuYinOriginalEntities()
        };
    }

    internal bool IsIssuedGuYinInitialCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => HasGuYinCapability && c.SkillId == "le:guyin" && c.BindingId == "mark-initial-entities" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.InitializeGuYinInitialEntities }] && context.Window == SkillProgramTriggerWindow.GameStarting && context.OwnerSeat == c.OwnerSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, Continuation: ProgramLifecycleContinuation.CompleteGameStarting, GuYinInitialEntities: not null } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<GuYinInitializedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat);
    SkillProgramStepOutcome global::CardGame.Core.ILeZhouYuGuYinHost.Initialize(ProgramSkillFrame f) => InitializeGuYin(f);
    SkillProgramStepOutcome global::CardGame.Core.ILeZhouYuGuYinHost.Draw(ProgramSkillFrame f) => BeginGuYinDraw(f);
    internal SkillProgramStepOutcome InitializeGuYin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || !CanContinueIssuedGuYin(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<GuYinInitializedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("GuYin initialization needs its exact frozen native opening mother.");
        var parent = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == f.WindowContext!.ParentFrameId);
        var entities = parent.GuYinInitialEntities!.Where(e => e.OriginalOwnerSeat != f.OwnerSeat).ToArray();
        if (entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => !ValidGuYinInitialEntry(e)))
            throw new InvalidOperationException("GuYin opening entities lost their genuine entry invoices.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuYinInitializedEvent(f.Id, GuYinSource(f), f.GameplayHash, entities.Length));
        foreach (var e in entities)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuYinInitialEntityRegisteredEvent(f.Id, f.OwnerSeat, e.OriginalOwnerSeat, e.CardId, e.PrintedKind, e.EntrySequence));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidGuYinInitialEntry(GuYinInitialEntity e) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.OriginalOwnerSeat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.EntrySequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(e.OriginalOwnerSeat)) == 1;
    internal IReadOnlyList<GuYinInitialEntity> GuYinOriginalEntities()
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<GuYinOpeningCapturedEvent>().ToArray()is not [var captured])
            return[];
        var entities = _coreProgramEventHistoryOperations.ProgramEventHistory<GuYinOpeningEntityEvent>().Where(e => e.SnapshotId == captured.SnapshotId).Select(e => new GuYinInitialEntity(e.OriginalOwnerSeat, e.CardId, e.PrintedKind, e.EntrySequence)).ToArray();
        if (entities.Length != captured.EntityCount || entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => !ValidGuYinInitialEntry(e)))
            throw new InvalidOperationException("GuYin opening registry lost its original immutable entry invoices.");
        return Array.AsReadOnly(entities);
    }

    internal IReadOnlyList<GuYinSourceSnapshot> CaptureGuYinSources() => !HasGuYinCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete ? [] : Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.Where(i => i.SkillId == "le:guyin").SelectMany(i => i.Program.Triggers.Where(IsGuYinDiscardTrigger).Select(t => new GuYinSourceSnapshot(new(i.SkillId, t.Id, p.Seat, i.SkillInstanceId), i.Program.GameplayHash))).OrderBy(s => s.Source.SkillInstanceId, StringComparer.Ordinal).Take(1)).OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, s.Source.OwnerSeat)).ToArray());
    internal void CaptureGuYinDiscardBatch(CardMovementBatchContext batch, IReadOnlyList<GuYinSourceSnapshot> sources)
    {
        if (!HasGuYinCapability || sources.Count == 0)
            return;
        var initial = GuYinOriginalEntities();
        foreach (var m in batch.Movements.Where(m => m.To == CardLocation.DiscardPile && (_coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)is not null || _coreNativeUsedDiscardOriginsOperations.IsNativeTrueUseDiscardMovement(batch, m))))
        {
            var origin = initial.SingleOrDefault(e => e.CardId == m.CardId && e.PrintedKind == m.CardKind);
            if (origin is null || !ValidGuYinInitialEntry(origin))
                continue;
            foreach (var s in sources.Where(s => s.Source.OwnerSeat != origin.OriginalOwnerSeat))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuYinDiscardQualifiedEvent(batch.Id, s.Source, s.GameplayHash, origin.OriginalOwnerSeat, m.CardId, m.CardKind, m.Sequence, batch.TurnNumber, batch.MovementTiming?.ActualTurnOwnerSeat ?? _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
        }
    }

    internal GuYinDiscardQualifiedEvent[] ExactGuYinQualifications(CardMovementBatchContext batch, CardConversionSource source, string hash)
    {
        if (source.SkillId != "le:guyin" || source.BindingId != "initial-entity-discard-draw" || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash != hash)
            return[];
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<GuYinDiscardQualifiedEvent>().Where(e => e.BatchId == batch.Id && e.Source == source).OrderBy(e => e.MovementSequence).ToArray();
        if (facts.Length == 0 || facts.Select(e => e.MovementSequence).Distinct().Count() != facts.Length || facts.Any(e => e.GameplayHash != hash || e.ActualTurn != batch.TurnNumber || e.OriginalOwnerSeat == source.OwnerSeat || !GuYinOriginalEntities().Any(o => o.OriginalOwnerSeat == e.OriginalOwnerSeat && o.CardId == e.CardId && o.PrintedKind == e.PrintedKind && ValidGuYinInitialEntry(o)) || batch.Movements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(x => x == m) == 1) != 1))
            return[];
        return facts;
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectGuYinDiscardCandidates(CardMovementBatchContext batch) => !HasGuYinCapability ? [] : Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<GuYinDiscardQualifiedEvent>().Where(e => e.BatchId == batch.Id).Select(e => new GuYinSourceSnapshot(e.Source, e.GameplayHash)).Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s.Source.OwnerSeat].IsAlive && ExactGuYinQualifications(batch, s.Source, s.GameplayHash).Length > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<GuYinDrawStartedEvent>().Any(e => e.BatchId == batch.Id && e.Source == s.Source)).Select(s => new ProgramTriggerCandidate(s.Source.OwnerSeat, s.Source.SkillId, s.Source.BindingId, s.Source.SkillInstanceId, s.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger2(new ProgramTriggerCandidate(s.Source.OwnerSeat, s.Source.SkillId, s.Source.BindingId, s.Source.SkillInstanceId, s.GameplayHash, 0)).Priority)).ToArray());
    internal ProgramSkillWindowContext CreateGuYinMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.CardsMoved, window.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: c.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: window.Batch, OccurrenceIndex: c.OccurrenceIndex);
    internal bool GuYinMotherMatches(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == "le:guyin" && c.BindingId == "initial-entity-discard-draw" && c.OccurrenceIndex == 0 && context.Window == SkillProgramTriggerWindow.CardsMoved && context.OwnerSeat == c.OwnerSeat && context.SourceSeat == c.OwnerSeat && context.TargetSeat == c.OwnerSeat && context.OccurrenceIndex == 0 && context.MovementBatch is { } batch && context.ParentFrameId == batch.Id && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == batch.Id)is CardsMovedTriggerWindowFrame window && window.Batch == batch && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == c && ExactGuYinQualifications(batch, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash).Length > 0;
    [ProgramIssuedTrigger(240)]
    internal bool IsIssuedGuYinCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => IsIssuedGuYinInitialCandidate(c, context) || HasGuYinCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && GuYinMotherMatches(c, context) && !_coreProgramEventHistoryOperations.ProgramEventHistory<GuYinDrawStartedEvent>().Any(e => e.BatchId == context.ParentFrameId && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28810, SkillProgramEffectOp.InitializeGuYinInitialEntities, SkillProgramEffectOp.ResolveGuYinInitialEntityDiscardDraw)]
    internal bool CanOfferGuYin(ProgramTriggerLegalityContext g) => IsIssuedGuYinCandidate(g.Candidate, g.Context);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1900)]
    internal void CollectWindow_CollectGuYinDiscardCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectGuYinDiscardCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1400)]
    internal bool ExcludeOrdinaryMovement_IsGuYinDiscardTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsGuYinDiscardTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1800)]
    internal ProgramSkillWindowContext? TryCreateGuYinMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsGuYinDiscardTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateGuYinMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 400)]
    internal void CaptureMovementBatchStartGuYinSources(CardMovementBatchBuilder batch) => batch.GuYinSources = CaptureGuYinSources();
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4800)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureGuYinDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureGuYinDiscardBatch(completed, batch.GuYinSources);
        return completed;
    }
}
