#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal const string QiqinObtainReason = "skill-program.le-xiao-qiao.qiqin.obtain";

    internal static CardConversionSource QiqinSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_xiao_qiao_le_LeXiaoQiaoQiqin.QiqinSource(f);
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 220)]
    internal void ProjectQiqinJudgmentSlotMark(int cardId, ref CardSkillMarkProjection projection)
    {
        if (_characterDaQiaoLeOperations.ZixiQinJudgmentSlotLabel(cardId) is { } label)
            projection.OverrideMarkLabel("qiqin", label);
    }

    internal bool HasQiqinEntityMark(int id) => _cardEntityMarks.Has(id, "qiqin");
    internal bool HasQiqinHandEntityMark(int owner, int id) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner) && HasQiqinEntityMark(id);
    internal bool IsQiqinHandLimitExempt(CharacterState owner, int id) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:qiqin") && HasQiqinHandEntityMark(owner.Seat, id);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28610, SkillProgramEffectOp.ResolveQiqinDiscardPileQin)]
    internal bool CanOfferQiqin(ProgramTriggerLegalityContext gate) => gate.Trigger.Id == "collect-discard-qin" && gate.Context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && gate.Context.OwnerSeat == gate.Candidate.OwnerSeat && gate.Context.SourceSeat == gate.Candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[gate.Candidate.OwnerSeat].IsAlive;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28610)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28610)]
    internal bool PermitIssuedQiqin(ProgramSkillFrame f)
    {
        if (f.SkillId != "le:qiqin" || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        return f.TriggerId == "collect-discard-qin" && ValidQiqinReceipt(f);
    }

    SkillProgramStepOutcome ILeXiaoQiaoQiqinHost.Collect(ProgramSkillFrame f) => BeginQiqinCollect(f);
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
