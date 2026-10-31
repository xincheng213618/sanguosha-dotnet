#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuWumiaoRuntime
{
    internal const string WuYixianObtainReason = "skill-program.wu-guan-yu.yixian.obtain";
    internal const string WuYixianDrawReason = "skill-program.wu-guan-yu.yixian.draw";
    internal const string WuYixianUsage = "all-equipment";
    internal static CardConversionSource WuYixianSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_guan_yu_wumiao_WuGuanYuYixian.WuYixianSource(f);
    SkillProgramStepOutcome IWuYixianHost.Resolve(ProgramSkillFrame frame) => BeginWuYixian(frame);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28530, SkillProgramEffectOp.ResolveYixianAllEquipmentObtainAndCompensate)]
    internal bool CanActivateWuYixian(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(c.OwnerSeat, c.Program.Id, WuYixianUsage, SkillUsageScope.Game) == 0;
    internal SkillProgramStepOutcome BeginWuYixian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Yixian is not null)
        {
            ContinueWuYixian(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != "wumiao:yixian" || f.ActivationId != "all-equipment" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, WuYixianUsage, SkillUsageScope.Game) != 0)
            throw new InvalidOperationException("Yixian requires its owner's current unused limited Play activation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = new(WuYixianSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, YixianStage.BranchChoice) });
        PublishWuYixian(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> WuYixianChoices(ProgramSkillFrame f)
    {
        if (f.Yixian is not { } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        PromptChoice Choice(string branch, string label, int? target = null) => new(new($"wu-yixian.{(f.Id)}.{(r.Cursor)}.{(branch)}"), label, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "wu-yixian", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        if (r.Stage == YixianStage.BranchChoice)
            return Array.AsReadOnly(new[] { Choice("field", "获得场上所有装备牌"), Choice("discard", "获得弃牌堆中所有装备牌"), Choice("cancel", "取消") });
        if (r.Stage != YixianStage.RecipientChoice || r.Cursor >= r.Recipients.Count)
            return[];
        var recipient = r.Recipients[r.Cursor];
        return Array.AsReadOnly(new[] { Choice("benefit", $"令{(_coreMatchParticipantsPlayersAccess._players[recipient.Seat].Name)}摸{(recipient.ObtainedCount)}张牌并回复1点体力", recipient.Seat), Choice("skip", "不令其摸牌及回复体力", recipient.Seat) });
    }

    internal void PublishWuYixian(ProgramSkillFrame f)
    {
        var choices = WuYixianChoices(f);
        if (choices.Count == 0)
        {
            CompleteWuYixian(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.Yixian!.Stage == YixianStage.BranchChoice ? "义贤：选择获得装备牌的范围。" : "义贤：可令原装备持有者摸牌并回复体力。", [], [], SourceSeat: f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wu-yixian")]
    internal void AnswerWuYixian(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Yixian: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !WuYixianChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Yixian requires its original private branch or recipient choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == YixianStage.BranchChoice)
        {
            if (branch == "cancel")
            {
                CompleteWuYixian(f);
                return;
            }

            CommitWuYixianEquipment(f, branch == "field" ? YixianBranch.Field : YixianBranch.DiscardPile);
            return;
        }

        var recipient = r.Recipients[r.Cursor];
        var accepted = branch == "benefit";
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianRecipientDecisionEvent(f.Id, r.Cursor, recipient.Seat, recipient.ObtainedCount, accepted));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = r with { BenefitAccepted = accepted, Stage = accepted ? YixianStage.Drawing : YixianStage.RecipientChoice, Cursor = accepted ? r.Cursor : r.Cursor + 1 } });
        ContinueWuYixian(f.Id);
    }

    internal void CommitWuYixianEquipment(ProgramSkillFrame f, YixianBranch branch)
    {
        if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(f.OwnerSeat, f.SkillId, WuYixianUsage, SkillUsageScope.Game, 1))
            throw new InvalidOperationException("Yixian cannot pay an expired or already-consumed limited activation.");
        var ids = branch == YixianBranch.Field ? _coreMatchParticipantsPlayersAccess._players.SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p)).Select(c => c.Id).ToArray() : _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind)).Select(c => c.Id).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = f.Yixian!with { Paid = true, Branch = branch, Stage = YixianStage.EquipmentChildren, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new CardMoveReason(WuYixianObtainReason), (batchId, movements) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { Yixian = current.Yixian!with { BatchId = batchId, Entities = movements.Select(m => new YixianEntity(m.CardId, m.CardKind, m.From, m.To, m.Sequence)).ToArray() } });
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.Yixian!;
        var recipients = r.Entities.Where(e => e.From.Zone == CardZoneKind.Equipment && e.From.OwnerSeat is not null && e.To == CardLocation.Hand(f.OwnerSeat)).GroupBy(e => e.From.OwnerSeat!.Value).OrderBy(g => _coreTableOrderOperations.RelativeTableOrder(r.TurnOwner, g.Key)).Select(g => new YixianRecipient(g.Key, g.Count())).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = r = r with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Recipients = recipients } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(f.OwnerSeat, f.SkillId, WuYixianUsage, SkillUsageScope.Game, 1));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, branch, r.BatchId, r.Entities.Count, r.Recipients.Count, r.SequenceBefore, r.SequenceAfter));
        for (var i = 0; i < r.Entities.Count; i++)
        {
            var e = r.Entities[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianEntityObtainedEvent(f.Id, i, e.CardId, e.PrintedKind, e.From, e.To, e.MovementSequence));
        }

        for (var i = 0; i < recipients.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianRecipientFrozenEvent(f.Id, i, recipients[i].Seat, recipients[i].ObtainedCount));
        ContinueWuYixian(f.Id);
    }

    internal bool DrainWuYixian(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueWuYixian(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Yixian: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (f.NativeDraw is not null)
                return;
            if (r.Stage is YixianStage.EquipmentChildren or YixianStage.DrawChildren or YixianStage.RecoveryChildren)
            {
                if (DrainWuYixian(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.Yixian!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
                if (r.Stage == YixianStage.DrawChildren)
                {
                    var seat = r.Recipients[r.Cursor].Seat;
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = r = r with { Stage = YixianStage.RecoveryChildren }, PendingMovementContinuation = new(seat, 0, null) });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianRecoveryIssuedEvent(id, r.Cursor, seat, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive));
                    if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
                        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, seat, 1, null, null);
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = r = r with { Stage = YixianStage.RecipientChoice, Cursor = r.Cursor + (r.Stage == YixianStage.RecoveryChildren ? 1 : 0), BenefitAccepted = false, DrawOutcome = null } });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteWuYixian(f);
                return;
            }

            if (r.Stage == YixianStage.Drawing)
            {
                var peer = r.Recipients[r.Cursor];
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, peer.Seat, peer.ObtainedCount, new(WuYixianDrawReason));
                return;
            }

            if (r.Stage == YixianStage.RecipientChoice)
            {
                while (r.Cursor < r.Recipients.Count && !_coreMatchParticipantsPlayersAccess._players[r.Recipients[r.Cursor].Seat].IsAlive)
                {
                    var peer = r.Recipients[r.Cursor];
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianRecipientDecisionEvent(id, r.Cursor, peer.Seat, peer.ObtainedCount, false));
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = r = r with { Cursor = r.Cursor + 1 } });
                }

                if (r.Cursor >= r.Recipients.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    CompleteWuYixian(f);
                    return;
                }

                PublishWuYixian(f);
                return;
            }

            if (r.Stage == YixianStage.BranchChoice)
            {
                PublishWuYixian(f);
                return;
            }

            throw new InvalidOperationException("Yixian returned to an unsupported native stage.");
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveYixianAllEquipmentObtainAndCompensate)]
    internal void ReturnWuYixianDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.Yixian is not { Paid: true, Stage: YixianStage.Drawing, BenefitAccepted: true } r || r.Cursor >= r.Recipients.Count || outcome.RecipientSeat != r.Recipients[r.Cursor].Seat || outcome.RequestedCount != r.Recipients[r.Cursor].ObtainedCount || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<YixianDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Cursor))
            throw new InvalidOperationException("Yixian's native draw returned to a different or already-consumed original owner.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Yixian = r with { Stage = YixianStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianDrawReturnedEvent(f.Id, r.Cursor, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueWuYixian(f.Id);
    }

    internal void CompleteWuYixian(ProgramSkillFrame f)
    {
        var r = f.Yixian!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YixianCompletedEvent(f.Id, r.Paid, r.Branch, r.Entities.Count, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<YixianRecipientDecisionEvent>().Count(e => e.FrameId == f.Id && e.Accepted)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28530)]
    internal bool ResumeWuYixian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Yixian: not null } f || f.Id != id)
            return false;
        if (f.NativeDraw is null)
            ContinueWuYixian(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28530)]
    internal bool ReturnWuYixianMovement(ProgramSkillFrame f)
    {
        if (f.Yixian is null)
            return false;
        ContinueWuYixian(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28530)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28530)]
    internal bool PermitPaidWuYixian(ProgramSkillFrame f) => CanContinueIssuedWuYixian(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveYixianAllEquipmentObtainAndCompensate)]
    internal PromptChoice ChooseWuYixianAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.Yixian!;
        if (r.Stage == YixianStage.BranchChoice)
        {
            var field = _coreMatchParticipantsPlayersAccess._players.Sum(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count);
            var discard = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Count(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind));
            return decision.Choices.Single(c => c.Parameters["branch"] == (field >= discard ? "field" : "discard"));
        }

        var recipient = r.Recipients[r.Cursor];
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        var value = _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, recipient.Seat, new(recipient.ObtainedCount, 0, 0, 0, 1, 0, false, false));
        return decision.Choices.Single(c => c.Parameters["branch"] == (value > 0 ? "benefit" : "skip"));
    }
}
