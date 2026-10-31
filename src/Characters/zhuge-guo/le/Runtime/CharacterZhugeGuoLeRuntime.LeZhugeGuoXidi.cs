#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoLeRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 20)]
    internal bool LeZhugeGuoFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => HasLeZhugeGuoCapability && (CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind));
    internal const string LeXidi = "le:xidi", LeChengyan = "le:chengyan", LeChengyanDrawReason = "skill-program.le:chengyan.draw";
    internal bool HasLeZhugeGuoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveLeZhugeGuoChengyan) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeLeZhugeGuoXidi);

    internal static CardConversionSource LeZhugeGuoSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhuge_guo_le_LeZhugeGuoXidi.LeZhugeGuoSource(f);
    // Registration only; the query still belongs to legacy GameEngine.LeZhugeGuoXidi.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 150)]
    internal void ProjectLeXidiCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasLeXidiEntityMark(cardId) ? "笛" : null);
    }

    internal bool HasLeXidiEntityMark(int cardId) => HasLeZhugeGuoCapability && cardId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<LeXidiEntityMarkedEvent>().Any(e => e.CardId == cardId);
    internal bool IsLeXidiHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, LeXidi) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && HasLeXidiEntityMark(cardId);
    internal ProgramLifecycleTriggerWindowFrame CaptureLeXidiOpening(ProgramLifecycleTriggerWindowFrame w)
    {
        if (!HasLeZhugeGuoCapability || w.Window != SkillProgramTriggerWindow.GameStarting)
            return w;
        return w with
        {
            LeXidiOpening = w.Candidates.Where(c => c.SkillId == LeXidi && c.BindingId == "mark-initial-flutes").Select(c => c.OwnerSeat).Distinct().SelectMany(s => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Select(card => new LeXidiOpeningEntity(s, card.Id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == card.Id && m.To == CardLocation.Hand(s)).Sequence))).ToArray()
        };
    }

    internal SkillProgramStepOutcome InitializeLeXidi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != LeXidi || f.TriggerId != "mark-initial-flutes" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { LeXidiOpening: not null } w || _coreProgramEventHistoryOperations.ProgramEventHistory<LeXidiInitializedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Xidi requires its original frozen opening-Hand window.");
        var cards = w.LeXidiOpening.Where(e => e.OwnerSeat == f.OwnerSeat).ToArray();
        foreach (var e in cards)
        {
            if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.EntrySequence && m.CardId == e.CardId && m.To == CardLocation.Hand(e.OwnerSeat)))
                throw new InvalidOperationException("Xidi opening entity lost its actual Hand entry.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeXidiEntityMarkedEvent(f.Id, LeZhugeGuoSource(f), e.CardId, e.EntrySequence, true));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeXidiInitializedEvent(f.Id, LeZhugeGuoSource(f), w.Id, cards.Length));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome ReorderLeXidi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != LeXidi || f.ActivationId != f.TriggerId || f.WindowContext?.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.TurnEnding))
            throw new InvalidOperationException("Xidi reorder requires its real preparation or ending window.");
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count(c => HasLeXidiEntityMark(c.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeXidiReorderStartedEvent(f.Id, f.OwnerSeat, count, Math.Max(1, count)));
        return _coreProgramTopReorderOperations.BeginProgramTopReorder(f, Math.Max(1, count), null);
    }

    SkillProgramStepOutcome ILeZhugeGuoHost.Initialize(ProgramSkillFrame f) => InitializeLeXidi(f);
    SkillProgramStepOutcome ILeZhugeGuoHost.Reorder(ProgramSkillFrame f) => ReorderLeXidi(f);
    SkillProgramStepOutcome ILeZhugeGuoHost.Chengyan(ProgramSkillFrame f) => StartLeChengyan(f);
}
