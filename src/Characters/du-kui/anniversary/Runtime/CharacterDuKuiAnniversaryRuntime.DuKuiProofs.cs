#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuKuiAnniversaryRuntime
{
    internal bool ValidDuKuiFrame(ProgramSkillFrame f)
    {
        if (f.DuKui is not { } r || f.WindowContext is not { } w || f.InstructionIndex != 1 || r.Source != DuKuiSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ParentId != w.ParentFrameId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<DuKuiStartedEvent>().Count(e => e == new DuKuiStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, r.Operation)) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin)
        {
            if (f.SkillId != AnniversaryDuKuiComposition.Fanyin || w.Window != SkillProgramTriggerWindow.PlayPhaseStarting || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack[index - 1] is not PlayPhaseStartingBoundaryFrame p || p.Id != r.ParentId || p.OwnerSeat != f.OwnerSeat || p.ItemIndex < 0 || p.ItemIndex >= p.Items.Count || p.Items[p.ItemIndex].Candidate is not { } c || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) || r.Iteration is < 0 or > 4 || r.Moves != 0 || r.Iteration > 0 && (r.Rank is < 1 or > 13 || r.CardId <= 0) || r.Stage is DuKuiStage.MoveChoice or DuKuiStage.MoveChildren)
                return false;
        }
        else if (r.Operation != SkillProgramEffectOp.ResolveAnniversaryDuKuiPeiqi || f.SkillId != AnniversaryDuKuiComposition.Peiqi || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.TargetSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame d || d.Id != r.ParentId || d.TargetSeat != f.OwnerSeat || d.CandidateIndex < 0 || d.CandidateIndex >= d.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, d.Candidates[d.CandidateIndex].ToProgramCandidate()) || r.Moves is < 0 or > 2 || r.Stage is not (DuKuiStage.MoveChoice or DuKuiStage.MoveChildren))
            return false;
        return true;
    }

    internal bool DuKuiShownPaid(ProgramSkillFrame f)
    {
        if (f.DuKui is not { } r || r.Operation != SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin || r.Iteration < 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<DuKuiRevealedEvent>().Where(e => e.FrameId == f.Id && e.Iteration == r.Iteration).ToArray()is not [var shown] || shown.CardId != r.CardId || shown.Rank != r.Rank || shown.Sequence <= 0 || shown.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !_coreProgramLifecycleOperations.GetProgramCardSet(f, DuKuiShownBind(r)).CardIds.SequenceEqual([r.CardId]))
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == shown.Sequence && m.CardId == r.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == "anniversary.fanyin.reveal") == 1;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 43200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 43200)]
    internal bool ContinuePaidDuKui(ProgramSkillFrame f) => f.DuKui is not null && ValidDuKuiFrame(f);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.ProcessingAllowance, 43200)]
    internal bool DuKuiRevealedProcessing(IReadOnlyList<Card> cards) => cards.Count == 1 && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(p => p.DuKui is not null)is { DuKui: { } r } f && r.Operation == SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin && cards[0].Id == r.CardId && ValidDuKuiFrame(f) && DuKuiShownPaid(f);
}
