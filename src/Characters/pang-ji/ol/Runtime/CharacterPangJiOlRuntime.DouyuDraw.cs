#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28132)]
    internal void CaptureDouyuFinalResult(IGameEvent payload)
    {
        if (!HasDouyuCapability || payload is not PindianResultDeterminedEvent result || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { DouyuContest: { Paid: true } r } parent || parent.Id != result.FrameId)
            return;
        int comparison;
        if (parent is PindianFrame pair)
        {
            if (result.SkillId != pair.SkillId || !IsDouyuPairAssignment(pair, result.Result) || (pair.Result is { } final ? final != result.Result : result.Result.SourceRank != r.Assignments[0].Rank || result.Result.OpponentRank != r.Assignments[1].Rank))
                throw new InvalidOperationException("Douyu cannot qualify a fabricated native pair result.");
            comparison = 0;
        }
        else if (parent is ProgramSkillFrame { SimultaneousPindian: { ComparisonIssued: true } simultaneous } f)
        {
            if (result.SkillId != f.SkillId || _coreSimultaneousPindianOperations.Value.SimultaneousResult(f) != result.Result)
                throw new InvalidOperationException("Douyu cannot qualify a different native common-group comparison.");
            comparison = simultaneous.ComparisonIndex;
        }
        else
            return;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuResultRecordedEvent>().Any(e => e.ContestId == parent.Id && e.ComparisonIndex == comparison))
            throw new InvalidOperationException("Douyu cannot repeat a settled native comparison.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuResultRecordedEvent(parent.Id, comparison, result.Result));
    }

    internal bool TryBeginDouyuPairWinDraws(PindianFrame pair) => pair.DouyuContest is not null && pair.Result is { } result && TryBeginDouyuWinWindow(pair, result, 0);
    internal bool TryBeginDouyuSimultaneousWinDraws(ProgramSkillFrame f, PindianResult result) => f.DouyuContest is not null && f.SimultaneousPindian is { } r && TryBeginDouyuWinWindow(f, result, r.ComparisonIndex);
    internal bool DouyuSwappedPair(DouyuContestReceipt r, DouyuSwap swap, PindianResult result) => result.SourceSeat == r.SourceSeat && result.OpponentSeat == (swap.Source.OwnerSeat == r.SourceSeat ? swap.OpponentSeat : swap.Source.OwnerSeat);
    internal bool TryBeginDouyuWinWindow(ResolutionFrame parent, PindianResult result, int comparison)
    {
        var r = parent.DouyuContest!;
        if (!r.Paid || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuResultRecordedEvent>().Count(e => e.ContestId == parent.Id && e.ComparisonIndex == comparison && e.Result == result) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == parent.Id && e.Result == result) != 1)
            throw new InvalidOperationException("Douyu win draws require the exact settled native result before its claims.");
        var entries = new List<DouyuWindowEntry>();
        foreach (var swap in r.Swaps.Where(s => DouyuSwappedPair(r, s, result)))
        {
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuRewardClosedEvent>().Any(e => e.ContestId == parent.Id && e.SwapFrameId == swap.FrameId))
                continue;
            if (!result.WonBy(swap.Source.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[swap.Source.OwnerSeat].IsAlive)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuRewardClosedEvent(parent.Id, comparison, swap.FrameId, false));
                continue;
            }

            var program = _contentRegistryOperations._contentRegistry.GetSkill(swap.Source.SkillId).Program!;
            var trigger = program.Triggers.Single(t => t.Id == "swapped-win");
            entries.Add(new(new(swap.Source.OwnerSeat, swap.Source.SkillId, trigger.Id, swap.Source.SkillInstanceId, swap.GameplayHash, trigger.Priority, swap.Ordinal), swap.FrameId));
        }

        if (entries.Count == 0)
            return false;
        _coreRuntimeOperations.PushRuntimeFrame(new DouyuWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, DouyuWindowStage.WinDraw, comparison, entries));
        ContinueDouyuWindow(_resolutionStackOperations._resolutionStack[^1].Id);
        return true;
    }

    internal SkillProgramStepOutcome BeginDouyuWinDraw(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.Douyu!;
        if (r.Stage != DouyuStage.DrawWaiting || r.DrawStarted || r.SwapFrameId is not { } swap || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuWinDrawStartedEvent>().Any(e => e.SwapFrameId == swap))
            throw new InvalidOperationException("Douyu cannot repeat or invent a swapped-pair win draw.");
        r = r with
        {
            DrawStarted = true,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, Douyu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuWinDrawStartedEvent(f.Id, r.ContestId, r.ComparisonIndex, swap, r.IssuedSource, f.OwnerSeat, r.Before));
        return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(DouyuDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw)]
    internal void ReturnDouyuWinDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.Douyu is not { DrawStarted: true, Stage: DouyuStage.DrawWaiting, SwapFrameId: { } swap } r || f.NativeDraw is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 2 || outcome.SequenceBefore < r.Before || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Count > 2 || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.ReplacementGainedIds.Distinct().Count() != outcome.ReplacementGainedIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuWinDrawStartedEvent>().Count(e => e.FrameId == f.Id && e.ContestId == r.ContestId && e.ComparisonIndex == r.ComparisonIndex && e.SwapFrameId == swap && e.Source == r.IssuedSource && e.RecipientSeat == f.OwnerSeat && e.Before == r.Before) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuWinDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Douyu lost its one actual native draw or replacement return.");
        if (outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == f.Id && e.RecipientSeat == f.OwnerSeat && e.RequestedCount == 2 && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("Douyu cannot accept another native draw attempt.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Douyu = r with { Stage = DouyuStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, AttemptFrameId = outcome.AttemptFrameId, ActualDrawIds = outcome.ActualDrawIds, ReplacementGainedIds = outcome.ReplacementGainedIds }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < outcome.ActualDrawIds.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuWinDrawEntityEvent(f.Id, i, outcome.ActualDrawIds[i], false));
        for (var i = 0; i < outcome.ReplacementGainedIds.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuWinDrawEntityEvent(f.Id, i, outcome.ReplacementGainedIds[i], true));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuWinDrawReturnedEvent(f.Id, r.ContestId, r.ComparisonIndex, swap, f.OwnerSeat, 2, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueDouyuProgram(f.Id);
    }

    internal bool DrainDouyu(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueDouyuProgram(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Douyu: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == DouyuStage.OpponentChoice)
        {
            PublishDouyu(f);
            return;
        }

        if (f.NativeDraw is not null || r.Stage == DouyuStage.DrawWaiting)
            return;
        if (DrainDouyu(f))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuRewardClosedEvent(r.ContestId, r.ComparisonIndex, r.SwapFrameId!.Value, true));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { Douyu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28132)]
    internal bool ResumeDouyu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Douyu: not null } f || f.Id != id || f.InstructionIndex == 0 || f.NativeDraw is not null)
            return false;
        ContinueDouyuProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28132)]
    internal bool ReturnDouyuMovement(ProgramSkillFrame f)
    {
        if (f.Douyu is null)
            return false;
        ContinueDouyuProgram(f.Id);
        return true;
    }
}
