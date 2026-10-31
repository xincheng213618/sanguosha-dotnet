#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZuoCiHappyGodRuntime
{
    internal bool HappyZcOriginalFrame(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId is HappyZuoCiComposition.Huanhua or HappyZuoCiComposition.Huanjing && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
        if (f.SkillId != HappyZuoCiComposition.Huanshu || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.Window == context.Window) != 1)
            return false;
        return _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == context.ParentFrameId) switch
        {
            ProgramLifecycleTriggerWindowFrame w => w.Window == context.Window && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]),
            PlayPhaseStartingBoundaryFrame w => context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && f.TriggerId == "refresh-phantoms" && w.OwnerSeat == f.OwnerSeat && w.ItemIndex >= 0 && w.ItemIndex < w.Items.Count && w.Items[w.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && context == _programPhaseBoundaryOperations.Get().CreatePlayPhaseStartingProgramContext(w, candidate, w.Items[w.ItemIndex].Facts),
            DamageTriggerWindowFrame w => context.Window == SkillProgramTriggerWindow.AfterDamageApplied && w.TriggerWindow == context.Window && w.TargetSeat == f.OwnerSeat && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()),
            CardsMovedTriggerWindowFrame w => context.Window == SkillProgramTriggerWindow.CardsMoved && f.TriggerId == "foreign-phantom" && context.MovementBatch == w.Batch && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && context.MovementIndex == w.Candidates[w.CandidateIndex].OccurrenceIndex && context.MovementIndex is { } index && index >= 0 && index < w.Batch.Movements.Count && HappyZcForeignGain(w.Batch.Movements[index]) == f.OwnerSeat,
            _ => false
        };
    }

    internal bool ValidHappyZc(ProgramSkillFrame f)
    {
        if (f.HappyZuoCi is not { } r || r.Source != HappyZcSource(f) || r.GameplayHash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Operation || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !HappyZcOriginalFrame(f) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.GainedIds is not System.Collections.IList { IsReadOnly: true } || r.GainedIds.Distinct().Count() != r.GainedIds.Count || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiStartedEvent>().Count(e => e == new HappyZuoCiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.ParentId, r.ActualTurn)) != 1)
            return false;
        var state = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HappyZuoCi;
        if (state is null || !HappyZcSourceProof(f.OwnerSeat, state) || state.Entities is not System.Collections.IList { IsReadOnly: true } || state.UsedTemplates is not System.Collections.IList { IsReadOnly: true })
            return false;
        if (r.Stage == HappyZuoCiStage.InitialChildren)
            return r.Operation == SkillProgramEffectOp.InitializeHappyZuoCiPhantoms && state.InitialFrameId == f.Id;
        if (r.Stage == HappyZuoCiStage.RefreshChildren)
            return r.Operation == SkillProgramEffectOp.RefreshHappyZuoCiPhantoms && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiRefreshedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat) == 1;
        if (r.Stage is HappyZuoCiStage.PhantomChoice or HappyZuoCiStage.TemplateChoice)
            return r.Operation == SkillProgramEffectOp.TransformHappyZuoCiPhantom && f.SkillId == HappyZuoCiComposition.Huanhua && r.TemplateId == 0 && (r.Stage == HappyZuoCiStage.PhantomChoice ? r.PhantomId == 0 : r.PhantomId > 0 && state.Entities.Any(e => e.CardId == r.PhantomId && !e.ManuallyTransformed));
        if (r.Operation == SkillProgramEffectOp.TransformHappyZuoCiPhantom && (r.PhantomId <= 0 || r.TemplateId <= 0 || r.PhantomId == r.TemplateId || !state.UsedTemplates.Contains(r.TemplateId) || !state.Entities.Any(e => e.CardId == r.PhantomId && e.ManuallyTransformed) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiTransformedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SameSuit) != 1))
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveHappyZuoCiHuanjing && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiHuanjingIssuedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.ActualTurn == r.ActualTurn && e.Amount == r.Requested) != 1)
            return false;
        if (r.Stage == HappyZuoCiStage.GainChildren)
            return r.Requested > 0 && r.Overflow >= 0 && r.Overflow <= r.Requested && r.GainedIds.Count <= r.Requested - r.Overflow && r.GainedIds.All(id => state.Entities.Any(e => e.CardId == id) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == state.Deck && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.happy:huanshu.gain" && m.Sequence > r.Before && m.Sequence <= r.After) == 1) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiGainIssuedEvent>().Count(e => e == new HappyZuoCiGainIssuedEvent(f.Id, r.Requested, r.GainedIds.Count, r.Overflow, r.Before, r.After)) == 1;
        if (r.Stage == HappyZuoCiStage.Drawing)
            return r.Overflow > 0;
        return r.Stage == HappyZuoCiStage.DrawChildren && r.DrawOutcome is { } draw && draw.RecipientSeat == f.OwnerSeat && draw.RequestedCount == r.Overflow && draw.SequenceBefore == r.Before && draw.SequenceAfter == r.After && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiDrawReturnedEvent>().Count(e => e == new HappyZuoCiDrawReturnedEvent(f.Id, draw.RequestedCount, draw.ActualDrawIds.Count, r.Before, r.After)) == 1 && draw.ActualDrawIds.Distinct().Count() == draw.ActualDrawIds.Count && draw.ActualDrawIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Sequence > r.Before && m.Sequence <= r.After) == 1);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 42600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42600)]
    internal bool KeepPaidHappyZc(ProgramSkillFrame f) => f.HappyZuoCi is { Stage: not (HappyZuoCiStage.PhantomChoice or HappyZuoCiStage.TemplateChoice) } && ValidHappyZc(f);
}
