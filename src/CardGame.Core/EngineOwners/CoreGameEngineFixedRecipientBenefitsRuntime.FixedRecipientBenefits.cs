#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFixedRecipientBenefitsRuntime
{
    internal FixedRecipientBenefitIssuedEvent? OriginalFixedRecipient(int owner, string skill, string hash, string state) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedRecipientBenefitIssuedEvent>().SingleOrDefault(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.GameplayHash == hash && e.StateId == state);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 760)]
    internal bool CanRunFixedRecipientBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.IssueFixedRecipientBenefit)is { } issue)
            return context.Window == SkillProgramTriggerWindow.TurnEnding && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && OriginalFixedRecipient(candidate.OwnerSeat, candidate.SkillId, candidate.GameplayHash, issue.StateId!)is null;
        var select = trigger.Effects.FirstOrDefault(e => e.Op is SkillProgramEffectOp.SelectIssuedFixedRecipient or SkillProgramEffectOp.SelectIssuedFixedRecipientWithDeathReturn);
        if (select is null)
            return true;
        var issued = OriginalFixedRecipient(candidate.OwnerSeat, candidate.SkillId, candidate.GameplayHash, select.StateId!);
        return context.Window == SkillProgramTriggerWindow.OwnerDied && issued is not null && issued.Source.SkillInstanceId == candidate.SkillInstanceId && _coreTableSeatQueriesOperations.IsValidPlayerSeat(issued.RecipientSeat) && _coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].IsAlive && issued.RecipientSeat != candidate.OwnerSeat && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedRecipientDeathBenefitStartedEvent>().Any(e => e.IssuanceProgramFrameId == issued.ProgramFrameId);
    }

    internal IReadOnlyList<int>? PublishedFixedRecipientForAi(CharacterState owner, IReadOnlyList<SkillProgramEffect> effects, ProgramSkillWindowContext? context)
    {
        var selection = effects.FirstOrDefault(e => e.Op is SkillProgramEffectOp.SelectIssuedFixedRecipient or SkillProgramEffectOp.SelectIssuedFixedRecipientWithDeathReturn);
        if (selection is null)
            return null;
        if (context is not { Window: SkillProgramTriggerWindow.OwnerDied } || context.OwnerSeat != owner.Seat || _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().LastOrDefault()is not { } window || context.ParentFrameId != window.Id || window.OwnerSeat != owner.Seat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return[];
        var candidate = window.Candidates[window.CandidateIndex];
        // Original beneficiary issuance is already public. This query does not
        // read any concealed hand, card receipt or hidden source identity.
        return candidate.OwnerSeat == owner.Seat && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.SequenceEqual(effects) && OriginalFixedRecipient(owner.Seat, candidate.SkillId, candidate.GameplayHash, selection.StateId!)is { } issued && issued.Source.SkillInstanceId == candidate.SkillInstanceId && _coreTableSeatQueriesOperations.IsValidPlayerSeat(issued.RecipientSeat) && _coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].IsAlive ? Array.AsReadOnly(new[] { issued.RecipientSeat }) : [];
    }

    internal bool ExactFixedRecipientEnding(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } context && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().LastOrDefault()is { } ending && context.ParentFrameId == ending.Id && ending.OwnerSeat == f.OwnerSeat && ending.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && ending.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    internal bool ExactFixedRecipientDeath(ProgramSkillFrame f, out ProgramDeathTriggerWindowFrame window)
    {
        window = null !;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index < 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramDeathTriggerWindowFrame parent || _resolutionStackOperations._resolutionStack[index - 2] is not DeathFrame death || parent.DeathFrameId != death.Id || death.VictimSeat != f.OwnerSeat || parent.OwnerSeat != f.OwnerSeat || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || context.ParentFrameId != parent.Id || context.OwnerSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || context.SourceSeat != parent.KillerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        window = parent;
        return true;
    }

    internal SkillProgramStepOutcome IssueFixedRecipientBenefit(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.FixedRecipient is not null || f.InstructionIndex != 2 || !ExactFixedRecipientEnding(f) || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_corePairObtainShownGiftsOperations.PairBenefitSourceCurrent(f) || OriginalFixedRecipient(f.OwnerSeat, f.SkillId, f.GameplayHash, effect.StateId!)is not null)
            throw new InvalidOperationException("A fixed recipient must issue once from its original own actual Ending candidate.");
        var r = new ProgramFixedRecipientReceipt(1, effect.StateId!, f.Id, _corePairObtainShownGiftsOperations.PairBenefitSource(f), f.GameplayHash, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, false, null);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FixedRecipient = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FixedRecipientBenefitIssuedEvent(f.Id, r.StateId, r.Source, r.GameplayHash, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome SelectIssuedFixedRecipient(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.FixedRecipient is not null || f.InstructionIndex != 1 || !ExactFixedRecipientDeath(f, out var window))
            throw new InvalidOperationException("The death benefit must retain its exact original OwnerDied candidate and real Death parent.");
        var issued = OriginalFixedRecipient(f.OwnerSeat, f.SkillId, f.GameplayHash, effect.StateId!);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || issued is null || issued.Source.SkillInstanceId != f.SkillInstanceId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(issued.RecipientSeat) || !_coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].IsAlive || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedRecipientDeathBenefitStartedEvent>().Any(e => e.IssuanceProgramFrameId == issued.ProgramFrameId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "原受益者或同一来源实例已失效，死亡收益取消且不改选角色。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var receipt = new ProgramFixedRecipientReceipt(0, effect.StateId!, issued.ProgramFrameId, issued.Source, issued.GameplayHash, issued.RecipientSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, true, window.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SelectedTargetSeats = Array.AsReadOnly(new[] { issued.RecipientSeat }), FixedRecipient = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FixedRecipientDeathBenefitStartedEvent(f.Id, issued.ProgramFrameId, window.Id, effect.StateId!, issued.Source, issued.GameplayHash, issued.RecipientSeat));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidFixedRecipientReceipt(ProgramSkillFrame f)
    {
        if (f.FixedRecipient is not { } r || r.GameplayHash != f.GameplayHash || r.Source.SkillId != f.SkillId || r.Source.OwnerSeat != f.OwnerSeat || r.Source.SkillInstanceId != f.SkillInstanceId || r.RecipientSeat == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.RecipientSeat) || !f.SelectedTargetSeats.SequenceEqual([r.RecipientSeat]) || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || OriginalFixedRecipient(f.OwnerSeat, f.SkillId, f.GameplayHash, r.StateId)is not { } issued || issued.Source != r.Source || issued.ProgramFrameId != r.IssuanceProgramFrameId || issued.RecipientSeat != r.RecipientSeat || string.IsNullOrWhiteSpace(r.Source.BindingId) || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || !ValidFixedRecipientProgress(f, r))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions;
        if (!r.DeathReplay)
            return r.InstructionIndex == 1 && f.InstructionIndex is >= 2 and <= 4 && r.IssuanceProgramFrameId == f.Id && r.Source == _corePairObtainShownGiftsOperations.PairBenefitSource(f) && r.OwnerDeathWindowFrameId is null && ExactFixedRecipientEnding(f) && plan is [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLiving }, { Op: SkillProgramEffectOp.IssueFixedRecipientBenefit, StateId: var state }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 3 }, { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] && state == r.StateId;
        return r.InstructionIndex == 0 && f.InstructionIndex is >= 1 and <= 3 && ExactFixedRecipientDeath(f, out var window) && r.OwnerDeathWindowFrameId == window.Id && plan is [{ Op: var selectOp, StateId: var stateId }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 3 }, { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] && stateId == r.StateId && (selectOp == SkillProgramEffectOp.SelectIssuedFixedRecipient || selectOp == SkillProgramEffectOp.SelectIssuedFixedRecipientWithDeathReturn && _coreOwnedDeathBenefitReturnsOperations.ExactOwnedDeathBenefitReturn(f)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedRecipientDeathBenefitStartedEvent>().Count(e => e.ProgramFrameId == f.Id && e.IssuanceProgramFrameId == r.IssuanceProgramFrameId && e.OwnerDeathWindowFrameId == window.Id && e.StateId == r.StateId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.RecipientSeat == r.RecipientSeat) == 1;
    }

    internal bool ValidFixedRecipientProgress(ProgramSkillFrame f, ProgramFixedRecipientReceipt r)
    {
        var draws = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedRecipientBenefitDrawIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        var recoveries = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedRecipientBenefitRecoveryRequestedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (!r.DrawIssued)
            return r.DrawSequenceBefore == 0 && r.DrawSequenceAfter == 0 && r.ActualDrawCount == 0 && draws.Length == 0 && r.RecoveryHpBefore is null && r.RecoveryAmount == 0 && recoveries.Length == 0;
        if (r.DrawSequenceBefore < 0 || r.DrawSequenceAfter < r.DrawSequenceBefore || r.ActualDrawCount is < 0 or > 3 || draws is not [var draw] || draw.IssuanceProgramFrameId != r.IssuanceProgramFrameId || draw.RecipientSeat != r.RecipientSeat || draw.DeathReplay != r.DeathReplay || draw.SequenceBefore != r.DrawSequenceBefore || draw.SequenceAfter != r.DrawSequenceAfter || draw.ActualCount != r.ActualDrawCount || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.DrawSequenceBefore && m.Sequence <= r.DrawSequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.RecipientSeat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.Draw)}") != r.ActualDrawCount)
            return false;
        return r.RecoveryHpBefore is not { } hpBefore ? r.RecoveryAmount == 0 && recoveries.Length == 0 : r.RecoveryAmount == 1 && recoveries is [var recovery] && recovery.IssuanceProgramFrameId == r.IssuanceProgramFrameId && recovery.RecipientSeat == r.RecipientSeat && recovery.DeathReplay == r.DeathReplay && recovery.HpBefore == hpBefore && recovery.Amount == 1;
    }
}
