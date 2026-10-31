#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanLeRuntime
{
    internal bool HasLeDiaoChanTanbanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeTanbanInitialEntities);

    internal static CardConversionSource TanbanSource(ProgramSkillFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_diao_chan_le_LeDiaoChanTanban.TanbanSource(frame);
    // One shared binary mark belongs to each physical entity, not to a holder or
    // grant. Losing/reacquiring a source never restores an old initial mark.
    // Registration only; the query still belongs to legacy GameEngine.LeDiaoChanTanban.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 140)]
    internal void ProjectTanbanCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasTanbanEntityMark(cardId) ? "檀板" : null);
    }

    internal bool HasTanbanEntityMark(int cardId)
    {
        if (!HasLeDiaoChanTanbanCapability || cardId <= 0)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<TanbanEntityMarkChangedEvent>().LastOrDefault(e => e.CardId == cardId && (_coreProgramEventHistoryOperations.ProgramEventHistory<TanbanInitializedEvent>().Any(done => done.FrameId == e.FrameId) || _coreProgramEventHistoryOperations.ProgramEventHistory<TanbanHandMarksSwappedEvent>().Any(done => done.FrameId == e.FrameId)))?.Marked == true;
    }

    internal bool IsTanbanHandLimitExempt(CharacterState owner, int cardId) => HasLeDiaoChanTanbanCapability && owner.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:tanban") && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && HasTanbanEntityMark(cardId);
    internal ProgramLifecycleTriggerWindowFrame CaptureTanbanInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasLeDiaoChanTanbanCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.TanbanInitialEntities is not null)
            throw new InvalidOperationException("Tanban's final opening Hand can only be frozen once.");
        var owners = window.Candidates.Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.InitializeTanbanInitialEntities }]).Select(candidate => candidate.OwnerSeat).Distinct().Order();
        var entities = owners.SelectMany(owner => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(card =>
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.To == CardLocation.Hand(owner));
            if (entry is null || entry.CardKind != card.Kind)
                throw new InvalidOperationException("A Tanban initial entity requires its actual final opening-Hand entry.");
            return new TanbanInitialEntity(owner, card.Id, card.Kind, entry.Sequence);
        })).ToArray();
        return window with
        {
            TanbanInitialEntities = entities
        };
    }

    [ProgramIssuedTrigger(200)]
    internal bool IsIssuedTanbanInitialCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => HasLeDiaoChanTanbanCapability && candidate.SkillId == "le:tanban" && candidate.BindingId == "mark-initial-entities" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash == candidate.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.InitializeTanbanInitialEntities }] && context.Window == SkillProgramTriggerWindow.GameStarting && context.OwnerSeat == candidate.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.NotStarted && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, Continuation: ProgramLifecycleContinuation.CompleteGameStarting, TanbanInitialEntities: not null } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<TanbanInitializedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29040, SkillProgramEffectOp.InitializeTanbanInitialEntities)]
    internal bool CanOfferTanbanInitial(ProgramTriggerLegalityContext gate) => IsIssuedTanbanInitialCandidate(gate.Candidate, gate.Context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29040)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29040)]
    internal bool PermitIssuedTanbanInitial(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "le:tanban" || frame.TriggerId != "mark-initial-entities" || frame.ActivationId != frame.TriggerId || frame.WindowContext is not { } context || frame.InstructionIndex is < 0 or > 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(frame, SkillProgramTriggerWindow.GameStarting) || !_coreOriginalHandEntityObserversOperations.OriginalHandBindingStarted(frame, SkillProgramTriggerWindow.GameStarting))
            return false;
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority, context.OccurrenceIndex);
        if (IsIssuedTanbanInitialCandidate(candidate, context))
            return true;
        return frame.InstructionIndex == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<TanbanInitializedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [var initialized] && initialized.Source == TanbanSource(frame) && initialized.GameplayHash == frame.GameplayHash;
    }

    SkillProgramStepOutcome global::CardGame.Core.ILeDiaoChanTanbanHost.InitializeTanban(ProgramSkillFrame frame) => InitializeTanban(frame);
    SkillProgramStepOutcome global::CardGame.Core.ILeDiaoChanTanbanHost.SwapTanbanHandMarks(ProgramSkillFrame frame) => SwapTanbanHandMarks(frame);
    internal SkillProgramStepOutcome InitializeTanban(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || !PermitIssuedTanbanInitial(frame) || _coreProgramEventHistoryOperations.ProgramEventHistory<TanbanInitializedEvent>().Any(e => e.Source.OwnerSeat == frame.OwnerSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<TanbanEntityMarkChangedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Tanban requires its exact frozen opening mother and initializes once per owner.");
        var parent = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == frame.WindowContext!.ParentFrameId);
        var entities = parent.TanbanInitialEntities!.Where(e => e.OwnerSeat == frame.OwnerSeat).ToArray();
        if (entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.EntrySequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(e.OwnerSeat))))
            throw new InvalidOperationException("Tanban cannot mark an entity without its exact frozen opening-Hand provenance.");
        var source = TanbanSource(frame);
        foreach (var entity in entities)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TanbanEntityMarkChangedEvent(frame.Id, source, frame.GameplayHash, entity.OwnerSeat, entity.CardId, entity.PrintedKind, entity.EntrySequence, HasTanbanEntityMark(entity.CardId), true));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TanbanInitializedEvent(frame.Id, source, frame.GameplayHash, parent.Id, entities.Length));
        return SkillProgramStepOutcome.Continue;
    }
}
