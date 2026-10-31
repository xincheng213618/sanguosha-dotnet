#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal const string QiqinObtainReason = "skill-program.le-xiao-qiao.qiqin.obtain";
    internal bool HasLeXiaoQiaoQiqinCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeQiqinInitialEntities);

    internal static CardConversionSource QiqinSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_xiao_qiao_le_LeXiaoQiaoQiqin.QiqinSource(f);
    // Registration only; the query still belongs to legacy GameEngine.LeXiaoQiaoQiqin and GameEngine.LeDaQiaoZixiPlacement.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 220)]
    internal void ProjectQiqinCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        if (HasQiqinEntityMark(cardId))
            projection.Append(_characterDaQiaoLeOperations.ZixiQinJudgmentSlotLabel(cardId) ?? "琴");
    }

    internal bool HasQiqinEntityMark(int id) => HasLeXiaoQiaoQiqinCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinInitialEntityMarkedEvent>().Any(e => e.CardId == id);
    internal bool HasQiqinHandEntityMark(int owner, int id) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner) && HasQiqinEntityMark(id);
    internal bool IsQiqinHandLimitExempt(CharacterState owner, int id) => HasLeXiaoQiaoQiqinCapability && _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:qiqin") && HasQiqinHandEntityMark(owner.Seat, id);
    internal ProgramLifecycleTriggerWindowFrame CaptureQiqinInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasLeXiaoQiaoQiqinCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.QiqinInitialEntities is not null)
            throw new InvalidOperationException("Qiqin opening Hand is frozen only once.");
        var owners = window.Candidates.Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.InitializeQiqinInitialEntities }]).Select(c => c.OwnerSeat).Distinct().Order();
        var entities = owners.SelectMany(owner => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(card =>
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.To == CardLocation.Hand(owner));
            if (entry is null || entry.CardKind != card.Kind)
                throw new InvalidOperationException("Qiqin initial entity needs its genuine opening entry.");
            return new QiqinInitialEntity(owner, card.Id, card.Kind, entry.Sequence);
        })).ToArray();
        return window with
        {
            QiqinInitialEntities = entities
        };
    }

    [ProgramIssuedTrigger(210)]
    internal bool IsIssuedQiqinCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => HasLeXiaoQiaoQiqinCapability && c.SkillId == "le:qiqin" && c.BindingId == "mark-initial-entities" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.InitializeQiqinInitialEntities }] && context.Window == SkillProgramTriggerWindow.GameStarting && context.OwnerSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.NotStarted && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, Continuation: ProgramLifecycleContinuation.CompleteGameStarting, QiqinInitialEntities: not null } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<QiqinInitializedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28610, SkillProgramEffectOp.InitializeQiqinInitialEntities, SkillProgramEffectOp.ResolveQiqinDiscardPileQin)]
    internal bool CanOfferQiqin(ProgramTriggerLegalityContext gate) => gate.Trigger.Id == "mark-initial-entities" ? IsIssuedQiqinCandidate(gate.Candidate, gate.Context) : gate.Trigger.Id == "collect-discard-qin" && gate.Context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && gate.Context.OwnerSeat == gate.Candidate.OwnerSeat && gate.Context.SourceSeat == gate.Candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[gate.Candidate.OwnerSeat].IsAlive;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28610)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28610)]
    internal bool PermitIssuedQiqin(ProgramSkillFrame f)
    {
        if (f.SkillId != "le:qiqin" || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (f.TriggerId == "collect-discard-qin")
            return ValidQiqinReceipt(f);
        if (f.TriggerId != "mark-initial-entities" || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.GameStarting))
            return false;
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        return IsIssuedQiqinCandidate(c, context) || f.InstructionIndex == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinInitializedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var issued] && issued.Source == QiqinSource(f) && issued.GameplayHash == f.GameplayHash;
    }

    SkillProgramStepOutcome global::CardGame.Core.ILeXiaoQiaoQiqinHost.Initialize(ProgramSkillFrame f) => InitializeQiqin(f);
    SkillProgramStepOutcome global::CardGame.Core.ILeXiaoQiaoQiqinHost.Collect(ProgramSkillFrame f) => BeginQiqinCollect(f);
    internal SkillProgramStepOutcome InitializeQiqin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || !PermitIssuedQiqin(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<QiqinInitializedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Qiqin requires its one exact frozen opening mother.");
        var parent = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(s => s.Id == f.WindowContext!.ParentFrameId);
        var entities = parent.QiqinInitialEntities!.Where(e => e.OwnerSeat == f.OwnerSeat).ToArray();
        if (entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.DealSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(e.OwnerSeat)) != 1))
            throw new InvalidOperationException("Qiqin lost its actual initial physical entry invoice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinInitializedEvent(f.Id, QiqinSource(f), f.GameplayHash, entities.Length));
        foreach (var e in entities)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinInitialEntityMarkedEvent(f.Id, e.OwnerSeat, e.CardId, e.PrintedKind, e.DealSequence));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome BeginQiqinCollect(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Qiqin is not null || f.SkillId != "le:qiqin" || f.TriggerId != "collect-discard-qin" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Qiqin needs its real living owner's preparation and cannot obtain twice.");
        var entities = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => HasQiqinEntityMark(c.Id)).Select(c => new QiqinCollectedEntity(c.Id, c.Kind)).ToArray();
        var r = new QiqinReceipt(QiqinSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Entities = entities
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qiqin = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinCollectStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.ActualTurn, r.TurnOwner, entities.Length, r.SequenceBefore));
        for (var i = 0; i < entities.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinCollectEntityIssuedEvent(f.Id, i, entities[i].CardId, entities[i].PrintedKind));
        if (entities.Length != 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(e => e.CardId).ToArray(), CardLocation.Hand(f.OwnerSeat), new(QiqinObtainReason), (batchId, moves) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var current = active.Qiqin!;
                if (current.BatchId != 0 || moves.Count != entities.Length || moves.Where((m, i) => m.CardId != entities[i].CardId || m.CardKind != entities[i].PrintedKind || m.From != CardLocation.DiscardPile || m.To != CardLocation.Hand(f.OwnerSeat) || m.Reason.Value != QiqinObtainReason || m.Sequence != current.SequenceBefore + i + 1).Any())
                    throw new InvalidOperationException("Qiqin lost its one whole actual discard-pile obtain batch.");
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { Qiqin = current with { BatchId = batchId, SequenceAfter = moves[^1].Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinCollectPaidEvent(f.Id, batchId, moves.Count, current.SequenceBefore, moves[^1].Sequence));
                for (var i = 0; i < moves.Count; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinCollectedEntityEvent(f.Id, i, moves[i].CardId, moves[i].CardKind, moves[i].Sequence));
            });
        ContinueQiqinCollect(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueQiqinCollect(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Qiqin: not null } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        AssertQiqin(f);
        if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TryStartPostInstructionWindow(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiqinCollectCompletedEvent(id, f.Qiqin!.WindowId, f.Qiqin.Entities.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28610)]
    internal bool ResumeQiqin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Qiqin: not null } f || f.Id != id)
            return false;
        ContinueQiqinCollect(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28610)]
    internal bool ReturnQiqinMovement(ProgramSkillFrame f)
    {
        if (f.Qiqin is null)
            return false;
        ContinueQiqinCollect(f.Id);
        return true;
    }
}
