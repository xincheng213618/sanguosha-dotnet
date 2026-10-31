#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiBoundaryMobileRuntime
{
    internal const string BoundaryFangzhuDiscard = "skill-program.boundary-cao-pi.fangzhu.discard", BoundaryFangzhuDraw = "skill-program.boundary-cao-pi.fangzhu.draw";
    internal static CardConversionSource BoundaryCaoPiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IBoundaryCaoPiHost.Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang ? BeginBoundaryXingshang(f, e) : BeginBoundaryFangzhu(f, e);
    internal DeathFrame? BoundaryCaoPiDeath(ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CharacterDied || _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window)
            return null;
        return _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == window.DeathFrameId && d.VictimSeat != context.OwnerSeat);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 54500)]
    internal bool QualifyBoundaryCaoPi(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId == BoundaryCaoPiComposition.Xingshang)
            return BoundaryCaoPiDeath(context)is { } death && (_coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]) || _coreDeathProgramsOperations.QueryProgramDeathCleanupCards(death, [CardZoneKind.Hand, CardZoneKind.Equipment]).Length > 0);
        if (c.SkillId == BoundaryCaoPiComposition.Fangzhu)
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue;
        if (c.SkillId != BoundaryCaoPiComposition.Songwei)
            return true;
        return context is { Window: SkillProgramTriggerWindow.JudgmentFinalized, Judgment: { } j } && j.SubjectSeat != c.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(j.SubjectSeat) && _coreMatchParticipantsPlayersAccess._players[j.SubjectSeat].IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[j.SubjectSeat]) == "wei" && j.Suit is Suit.Spade or Suit.Club && _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.Judgment == j && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] is { } candidate && candidate.OwnerSeat == c.OwnerSeat && candidate.SkillId == c.SkillId && candidate.SkillInstanceId == c.SkillInstanceId && candidate.GameplayHash == c.GameplayHash;
    }

    internal SkillProgramStepOutcome BeginBoundaryXingshang(ProgramSkillFrame input, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.BoundaryXingshang is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || BoundaryCaoPiDeath(context)is not { } death)
            throw new InvalidOperationException("Xingshang requires the original other role's completed native cleanup.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXingshang = new(BoundaryCaoPiSource(f), f.GameplayHash, context.ParentFrameId, death.Id, death.VictimSeat, e.ResultBind!) });
        PublishBoundaryCaoPi(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome BeginBoundaryFangzhu(ProgramSkillFrame input, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.BoundaryFangzhu is not null || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreHealthQueriesOperations.UnambiguousHp(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])is not { } hp)
            throw new InvalidOperationException("Fangzhu requires the injured owner's original damage frame and distinct actual recipient.");
        var x = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - hp);
        var r = new BoundaryFangzhuReceipt(BoundaryCaoPiSource(f), f.GameplayHash, context.ParentFrameId, context.DamageFrameId, target, x, e.ResultBind!);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryFangzhuStartedEvent(f.Id, r.Source, r.Hash, r.DamageWindow, r.DamageId, target, x, e.ResultBind!));
        PublishBoundaryCaoPi(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card[] BoundaryFangzhuMaterials(int target) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target])).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(target, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<PromptChoice> BoundaryCaoPiChoices(ProgramSkillFrame f)
    {
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"boundary-cao-pi.{(f.Id)}.{(branch)}.{(string.Join('-', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "boundary-cao-pi", ["branch"] = branch });
        var choices = new List<PromptChoice>();
        if (f.BoundaryXingshang is { } x)
        {
            var death = _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(d => d.Id == x.DeathId);
            if (_coreDeathProgramsOperations.QueryProgramDeathCleanupCards(death, [CardZoneKind.Hand, CardZoneKind.Equipment]).Length > 0)
                choices.Add(Choice("claim", "获得其所有手牌和装备牌"));
            if (_coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                choices.Add(Choice("recover", "回复1点体力"));
            return choices;
        }

        var r = f.BoundaryFangzhu!;
        var cards = BoundaryFangzhuMaterials(r.Target);
        foreach (var card in cards.Where(c => r.Selected.Contains(c.Id)))
            choices.Add(Choice("unpick", "取消弃置【" + card.DisplayName + "】", [card.Id]));
        if (cards.Length >= r.X)
        {
            if (r.Selected.Count < r.X)
                foreach (var card in cards.Where(c => !r.Selected.Contains(c.Id)))
                    choices.Add(Choice("card", "选择弃置【" + card.DisplayName + "】", [card.Id]));
            else
                choices.Add(Choice("discard", $"同时弃置{(r.X)}张牌并失去1点体力"));
        }

        choices.Add(Choice("draw", $"摸{(r.X)}张牌，然后翻面"));
        return choices;
    }

    internal void PublishBoundaryCaoPi(ProgramSkillFrame f)
    {
        var chooser = f.BoundaryFangzhu?.Target ?? f.OwnerSeat;
        var choices = BoundaryCaoPiChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, chooser, f.BoundaryFangzhu is { } r ? $"放逐：由你选择摸{(r.X)}张牌后翻面，或弃置等量手牌/装备并失去1点体力。" : "行殇：获得该角色所有手牌和装备，或回复1点体力。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = f.BoundaryFangzhu is { } cost ? _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(prompt, f, "fangzhu-target", "branch", ["card"], ["discard"], cost.Selected, cost.X - cost.Selected.Count, cost.X - cost.Selected.Count) : prompt;
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("boundary-cao-pi")]
    internal void AnswerBoundaryCaoPi(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Cao Pi choice lost its original trigger frame.");
        if (!BoundaryCaoPiChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets)))
            throw new InvalidOperationException("Cao Pi rejects an unrelated strategy or stale payment entity.");
        var branch = choice.Parameters["branch"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (f.BoundaryXingshang is { } corpse)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryXingshang = null });
            CommitBoundaryCaoPiResult(f, corpse.ResultBind, branch, f.OwnerSeat, corpse.Victim, 0);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var r = f.BoundaryFangzhu!;
        if (branch is "card" or "unpick")
        {
            var selected = branch == "card" ? r.Selected.Append(choice.Cards.Single()).ToArray() : r.Selected.Where(id => id != choice.Cards.Single()).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Selected = selected } });
            PublishBoundaryCaoPi(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (branch == "draw")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Stage = BoundaryFangzhuStage.DrawRequest, Outcome = "drawn", Before = before, After = before } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Target, r.X, new(BoundaryFangzhuDraw));
            return;
        }

        if (branch != "discard" || r.Selected.Count != r.X)
            throw new InvalidOperationException("Fangzhu must choose its full exact HE cost or its frozen-X draw.");
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r = r with { Stage = BoundaryFangzhuStage.DiscardChildren, Outcome = "discarded", Before = before, After = before }, PendingMovementContinuation = new(r.Target, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, r.Selected, new(BoundaryFangzhuDiscard));
        var invoice = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id && e.Reason.Value == BoundaryFangzhuDiscard && e.SequenceBefore == before);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DiscardInvocation = invoice?.InvocationId, Actual = invoice?.ActualCount ?? 0 } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryFangzhuPaidEvent(f.Id, r.Target, r.X, invoice?.InvocationId, invoice?.ActualCount ?? 0, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryFangzhu(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu)]
    internal void ReturnBoundaryFangzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.BoundaryFangzhu is not { Stage: BoundaryFangzhuStage.DrawRequest } r || outcome.RecipientSeat != r.Target || outcome.RequestedCount != r.X || outcome.ActualDrawIds.Count > r.X || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0)
            throw new InvalidOperationException("Fangzhu draw lost its original frozen-X request and exact typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Stage = BoundaryFangzhuStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Actual = outcome.ActualDrawIds.Count, DrawAttempt = outcome.AttemptFrameId, DrawReplaced = outcome.Replacement is not null }, PendingMovementContinuation = new(r.Target, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryFangzhuDrawReturnedEvent(f.Id, r.Target, r.X, outcome.ActualDrawIds.Count, outcome.AttemptFrameId, outcome.Replacement is not null, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryFangzhu(f.Id);
    }

    internal void ContinueBoundaryFangzhu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryFangzhu: { } r } f || f.Id != id || r.Stage is not (BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren))
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Stage = BoundaryFangzhuStage.Complete }, PendingMovementContinuation = null });
        if (r.Outcome == "discarded" && r.Actual != r.X)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        CommitBoundaryCaoPiResult(f, r.ResultBind, r.Outcome!, r.Target, r.Target, r.X);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void CommitBoundaryCaoPiResult(ProgramSkillFrame input, string bind, string outcome, int chooser, int subject, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding(bind, outcome, chooser)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryCaoPiChoiceResultEvent(f.Id, f.SkillId, bind, outcome, chooser, subject, count));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 54500)]
    internal bool ResumeBoundaryFangzhu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryFangzhu.Stage: BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren } f || f.Id != id)
            return false;
        ContinueBoundaryFangzhu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 54500)]
    internal bool ReturnBoundaryFangzhuMovement(ProgramSkillFrame f)
    {
        if (f.BoundaryFangzhu?.Stage is not (BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren))
            return false;
        ContinueBoundaryFangzhu(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54500)]
    internal bool KeepBoundaryFangzhuPaid(ProgramSkillFrame f) => f.BoundaryFangzhu is { Stage: not BoundaryFangzhuStage.Choice };
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu, SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang)]
    internal static PromptChoice BoundaryCaoPiAi(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? p.Choices.First();
}
