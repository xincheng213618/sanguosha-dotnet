#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFrozenHandRosterRuntime
{
    internal const string FrozenHandRosterDrawReason = "program.frozen-hand-roster.draw";
    internal bool FrozenHandRosterEndingParentMatches(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        return f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && f.TriggerId is not null && f.ActivationId == f.TriggerId && f.InstructionIndex == 1 && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && c.OwnerSeat == f.OwnerSeat && c.SourceSeat == f.OwnerSeat && c.TargetSeat == f.OwnerSeat && index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame parent && parent.Id == c.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex] is { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } && c.OccurrenceIndex == candidate.OccurrenceIndex && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    }

    internal bool FrozenHandRosterCurrent(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId && p.GameplayHash == f.GameplayHash);
    internal SkillProgramStepOutcome ResolveFrozenHandRosterEndingExchange(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.FrozenHandRoster is not null || !FrozenHandRosterEndingParentMatches(f))
            throw new InvalidOperationException("Frozen ending roster lost its exact initial lifecycle parent.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        var seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count >= hand).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, p.Seat)).Select(p => p.Seat).ToArray();
        var r = new FrozenHandRosterReceipt
        {
            Source = new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            ParentFrameId = f.WindowContext!.ParentFrameId,
            Turn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            InstructionIndex = f.InstructionIndex,
            InitialOwnerHandCount = hand,
            Seats = seats
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FrozenHandRosterStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.Turn, hand, seats));
        PublishFrozenHandRoster(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static Dictionary<string, string> FrozenHandParameters(ProgramSkillFrame f, string branch) => RuntimeSkills.RuntimeRules_Core_FrozenHandRoster.FrozenHandParameters(f, branch);
    internal int FrozenHandRosterChooser(ProgramSkillFrame f) => f.FrozenHandRoster!.Stage == "participant" ? f.FrozenHandRoster.Seats[f.FrozenHandRoster.Cursor] : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> FrozenHandRosterChoices(ProgramSkillFrame f)
    {
        var r = f.FrozenHandRoster!;
        var choices = new List<PromptChoice>();
        if (r.Stage == "participant")
        {
            choices.Add(new(new($"frozen-hand.{(f.Id)}.{(r.Cursor)}.draw"), $"令 {(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 摸一张牌", [], [], FrozenHandParameters(f, "draw")));
            choices.Add(new(new($"frozen-hand.{(f.Id)}.{(r.Cursor)}.decline"), "不令其摸牌", [], [], FrozenHandParameters(f, "decline")));
        }
        else
        {
            var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
            foreach (var seat in r.Seats.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && (r.Stage == "damage" ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count == count : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count < count)))
                choices.Add(new(new($"frozen-hand.{(f.Id)}.{(r.Stage)}.{(seat)}"), r.Stage == "damage" ? $"对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 造成1点伤害" : $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 摸一张牌", [], [seat], FrozenHandParameters(f, r.Stage)));
            if (r.Stage == "damage")
                choices.Add(new(new($"frozen-hand.{(f.Id)}.skip-damage"), "不造成伤害", [], [], FrozenHandParameters(f, "skip-damage")));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishFrozenHandRoster(ProgramSkillFrame f)
    {
        if (!FrozenHandRosterCurrent(f))
        {
            FinishFrozenHandRoster(f);
            return;
        }

        var r = f.FrozenHandRoster!;
        if (r.Stage == "participant")
        {
            var cursor = r.Cursor;
            while (cursor < r.Seats.Count && !_coreMatchParticipantsPlayersAccess._players[r.Seats[cursor]].IsAlive)
                cursor++;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r = r with { Cursor = cursor, Stage = cursor == r.Seats.Count ? "damage" : "participant" } });
        }

        var choices = FrozenHandRosterChoices(f);
        if (choices.Count == 0)
        {
            FinishFrozenHandRoster(f);
            return;
        }

        var chooser = FrozenHandRosterChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == "participant" ? $"澜疆：是否令 {(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 摸一张牌？" : "澜疆：从最初参加选择的角色中选择后续效果。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("frozen-hand-roster")]
    internal void ResolveFrozenHandRosterChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FrozenHandRoster: not null } f || !FrozenHandRosterCurrent(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != FrozenHandRosterChooser(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], FrozenHandRosterChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Frozen roster choice lost its real chooser and exact current frozen-set candidates.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var r = f.FrozenHandRoster!;
        var branch = selected.Parameters["branch"];
        if (r.Stage == "participant")
        {
            var cursor = r.Cursor;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FrozenHandRosterParticipantAnsweredEvent(f.Id, cursor, r.Seats[cursor], branch == "draw"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r with { Cursor = cursor + 1 } });
            if (branch == "draw")
                PayFrozenHandRosterDraw(f, f.OwnerSeat, "participant");
            else
                PublishFrozenHandRoster(f);
            return;
        }

        if (branch == "skip-damage")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r with { Stage = "reward" } });
            PublishFrozenHandRoster(f);
            return;
        }

        var target = selected.Targets.Single();
        if (r.Stage == "reward")
        {
            PayFrozenHandRosterDraw(f, target, "complete");
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r with { Stage = "damage-issued", DamageTarget = target, DamageIssued = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FrozenHandRosterDamageIssuedEvent(f.Id, f.OwnerSeat, target, 1));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
    }

    internal void PayFrozenHandRosterDraw(ProgramSkillFrame f, int target, string next)
    {
        var r = f.FrozenHandRoster!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r with { Stage = "movement", ReturnStage = next, DrawRecipient = target, Before = before, After = before, ActualDrawCount = 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, target, 1, null, null, SkillProgramCardSetVisibility.Private, new(FrozenHandRosterDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.FrozenHandRoster!;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(target) && m.Reason.Value == FrozenHandRosterDrawReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualDrawCount = actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FrozenHandRosterDrawPaidEvent(f.Id, r.Cursor, target, next, before, r.After, actual));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    internal SkillProgramEffect? FrozenHandRosterPausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool IsFrozenHandRosterMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveFrozenHandRosterEndingExchange && f.FrozenHandRoster is { Stage: "movement" } r && r.InstructionIndex == f.InstructionIndex && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 22000)]
    internal bool ReturnFrozenHandRosterMovement(ProgramSkillFrame f)
    {
        if (f.FrozenHandRoster is not { } r)
            return false;
        if (f.PendingMovementContinuation is not { } p || !IsFrozenHandRosterMovement(f, FrozenHandRosterPausedEffect(f), p))
            throw new InvalidOperationException("Frozen roster consumed another instruction's movement return.");
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd() || _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, FrozenHandRoster = r with { Stage = r.ReturnStage } });
        if (r.ReturnStage == "complete" || !FrozenHandRosterCurrent(f))
            FinishFrozenHandRoster(f);
        else
            PublishFrozenHandRoster(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 22000)]
    internal bool ResumeFrozenHandRoster(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FrozenHandRoster: not null } f || f.Id != id)
            return false;
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnFrozenHandRosterMovement(f);
            return true;
        }

        var r = f.FrozenHandRoster!;
        if (r.Stage == "damage-issued")
        {
            if (f.AttackAttempt is not null)
                return false;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = r with { Stage = "reward" } });
        }

        if (!FrozenHandRosterCurrent(f))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishFrozenHandRoster(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishFrozenHandRoster(f);
        return true;
    }

    internal void FinishFrozenHandRoster(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FrozenHandRoster = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool ValidFrozenHandRoster(ProgramSkillFrame f)
    {
        if (f.FrozenHandRoster is not { } r || r.Source != new CardConversionSource(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ParentFrameId != f.WindowContext?.ParentFrameId || r.InstructionIndex != f.InstructionIndex || FrozenHandRosterPausedEffect(f)?.Op != SkillProgramEffectOp.ResolveFrozenHandRosterEndingExchange || !FrozenHandRosterEndingParentMatches(f) || r.Cursor < 0 || r.Cursor > r.Seats.Count || r.Seats.Distinct().Count() != r.Seats.Count || !r.Seats.Contains(f.OwnerSeat) || r.Seats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || !r.Seats.SequenceEqual(r.Seats.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, s))) || r.Stage is not ("participant" or "damage" or "damage-issued" or "reward" or "movement" or "complete"))
            return false;
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (events.OfType<FrozenHandRosterStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started.Source != r.Source || started.GameplayHash != r.GameplayHash || started.ParentFrameId != r.ParentFrameId || started.Turn != r.Turn || started.OwnerHandCount != r.InitialOwnerHandCount || !started.Seats.SequenceEqual(r.Seats))
            return false;
        var answers = events.OfType<FrozenHandRosterParticipantAnsweredEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (answers.Any(e => e.Cursor < 0 || e.Cursor >= r.Cursor || e.Cursor >= r.Seats.Count || e.ChooserSeat != r.Seats[e.Cursor]) || answers.Select(e => e.Cursor).Distinct().Count() != answers.Length)
            return false;
        var draws = events.OfType<FrozenHandRosterDrawPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (draws.Any(e => e.ActualCount is < 0 or > 1 || e.Before > e.After || e.ReturnStage is not ("participant" or "complete") || e.ReturnStage == "participant" && (e.RecipientSeat != f.OwnerSeat || !answers.Any(a => a.Cursor == e.Cursor - 1 && a.Accepted)) || e.ReturnStage == "complete" && !r.Seats.Contains(e.RecipientSeat) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > e.Before && m.Sequence <= e.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(e.RecipientSeat) && m.Reason.Value == FrozenHandRosterDrawReason) != e.ActualCount) || draws.Where(e => e.ReturnStage == "participant").Select(e => e.Cursor).Distinct().Count() != draws.Count(e => e.ReturnStage == "participant") || draws.Count(e => e.ReturnStage == "complete") > 1)
            return false;
        if (r.Stage == "movement" && (r.DrawRecipient is not { } target || r.Before > r.After || draws.Count(e => e.Cursor == r.Cursor && e.RecipientSeat == target && e.ReturnStage == r.ReturnStage && e.Before == r.Before && e.After == r.After && e.ActualCount == r.ActualDrawCount) != 1))
            return false;
        var damage = events.OfType<FrozenHandRosterDamageIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return r.DamageIssued ? r.DamageTarget is { } d && r.Seats.Contains(d) && damage is [var paid] && paid.SourceSeat == f.OwnerSeat && paid.TargetSeat == d && paid.Amount == 1 : r.DamageTarget is null && damage.Length == 0;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveFrozenHandRosterEndingExchange)]
    internal PromptChoice SelectAiFrozenHandRoster(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.FrozenHandRoster!;
        if (r.Stage == "participant")
            return p.Choices.Single(c => c.Parameters["branch"] == (p.PlayerSeat == f.OwnerSeat || CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat], _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ? "draw" : "decline"));
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        var hint = r.Stage == "damage" ? new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false) : new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false);
        var candidates = p.Choices.Where(c => c.Targets.Count == 1).Select(c => (Choice: c, Score: _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(snapshot, c.Targets[0], hint))).OrderByDescending(c => c.Score).ThenBy(c => c.Choice.Targets[0]).ToArray();
        if (r.Stage == "damage" && (candidates.Length == 0 || candidates[0].Score <= 0))
            return p.Choices.Single(c => c.Parameters["branch"] == "skip-damage");
        return candidates[0].Choice;
    }

    SkillProgramStepOutcome IFrozenHandRosterHost.ResolveFrozenHandRosterEndingExchange(ProgramSkillFrame f) => ResolveFrozenHandRosterEndingExchange(f);
}
