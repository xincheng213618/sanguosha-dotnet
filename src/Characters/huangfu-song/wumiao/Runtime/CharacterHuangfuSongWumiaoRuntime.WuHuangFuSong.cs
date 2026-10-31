#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    internal const string WuHfObtain = "skill-program.wumiao:chaozhen.obtain", WuHfBottom = "skill-program.wumiao:lianjie.bottom", WuHfDraw = "skill-program.wumiao:lianjie.draw";
    internal bool HasWuHuangFuSongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuHuangFuSongLianjie) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ArmWuHuangFuSongJiangxian);

    internal WuHuangFuSongTurn WuHfTurn() => new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
    internal bool WuHfTurnOpen(WuHuangFuSongTurn t) => t == WuHfTurn() && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == t.Number && e.OwnerSeat == t.OwnerSeat);
    internal static CardConversionSource WuHfSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_huangfu_song_wumiao_WuHuangFuSong.WuHfSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30000, SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen, SkillProgramEffectOp.ResolveWuHuangFuSongLianjie, SkillProgramEffectOp.SettleWuHuangFuSongJiangxian)]
    internal bool CanOfferWuHuangFuSong(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c)
    {
        if (t.Effects[0].Op == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie)
            return WuHfLianjieUse(candidate.OwnerSeat, c)is not null;
        if (t.Effects[0].Op == SkillProgramEffectOp.SettleWuHuangFuSongJiangxian)
            return WuHfOutstandingArm(candidate.OwnerSeat)is not null;
        return WuHfChaozhenAvailable(candidate.OwnerSeat) && (c.Window == SkillProgramTriggerWindow.DyingEntering && c.TargetSeat == candidate.OwnerSeat || c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Preparation } phase && phase.ActorSeat == candidate.OwnerSeat);
    }

    SkillProgramStepOutcome IWuHuangFuSongHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWuHuangFuSong(f, op);
    internal SkillProgramStepOutcome BeginWuHuangFuSong(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WuHuangFuSong is not null || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op)
            throw new InvalidOperationException("Wu Huangfu Song needs his original unissued instruction.");
        if (op == SkillProgramEffectOp.ArmWuHuangFuSongJiangxian)
        {
            if (f.SkillId != WuHuangFuSongComposition.Jiangxian || f.ActivationId != "arm-turn" || f.TriggerId is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
                throw new InvalidOperationException("Jiangxian must be the native once-per-game Play activation.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongJiangxianArmedEvent(f.Id, WuHfSource(f), f.GameplayHash, WuHfTurn()));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.WindowContext is not { } context || f.TriggerId != f.ActivationId)
            throw new InvalidOperationException("Wu Huangfu Song lost his original native trigger window.");
        var use = op == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie ? WuHfLianjieUse(f.OwnerSeat, context) : null;
        if (op == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie && use is null || op == SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen && !WuHfChaozhenAvailable(f.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        var r = new WuHuangFuSongReceipt(WuHfSource(f), f.GameplayHash, op, WuHfTurn(), context.ParentFrameId, use?.Id ?? 0)
        {
            Stage = WuHuangFuSongStage.Choosing,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.Turn, r.WindowId, r.UseId));
        var choices = WuHfChoices(f);
        if (choices.Count == 0)
        {
            FinishWuHf(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishWuHf(f, choices);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> WuHfChoices(ProgramSkillFrame f)
    {
        var r = f.WuHuangFuSong!;
        if (r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen)
            return Array.AsReadOnly(WuHfMinimumPool().Select(x => new PromptChoice(new($"wu-huangfu-song.{(f.Id)}.minimum.{((x.From == CardLocation.DrawPile ? "deck" : x.Card.Id.ToString(CultureInfo.InvariantCulture)))}"), x.From == CardLocation.DrawPile ? "获得牌堆中点数最小的一张牌" : $"获得{(_coreMatchParticipantsPlayersAccess._players[x.From.OwnerSeat!.Value].Name)}场上的【{(x.Card.DisplayName)}】", x.From == CardLocation.DrawPile ? [] : [x.Card.Id], [], new Dictionary<string, string> { ["program-action"] = "wu-huangfu-song", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["selection"] = x.From == CardLocation.DrawPile ? "deck" : "field", ["field-card"] = x.From == CardLocation.DrawPile ? "0" : x.Card.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
        if (r.Operation == SkillProgramEffectOp.SettleWuHuangFuSongJiangxian)
            return Array.AsReadOnly(new[] { WuHuangFuSongComposition.Lianjie, WuHuangFuSongComposition.Chaozhen }.Where(id => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], id)).Select(id => new PromptChoice(new($"wu-huangfu-song.{(f.Id)}.lose.{(id)}"), $"失去【{(_contentRegistryOperations._contentRegistry.GetSkill(id).Name)}】", [], [], new Dictionary<string, string> { ["program-action"] = "wu-huangfu-song", ["lose-skill"] = id, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
        if (r.Stage == WuHuangFuSongStage.ChoosingMinimumHand)
        {
            var target = r.TargetSeat!.Value;
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]);
            if (hand.Count == 0)
                return[];
            var min = hand.Min(card => _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[target], card));
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[target], card) == min).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "wu-huangfu-song" } }).ToArray());
        }

        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => WuHfVictimAvailable(f.OwnerSeat, p.Seat)).Select(p => new PromptChoice(new($"wu-huangfu-song.{(f.Id)}.target.{(p.Seat)}"), $"将{(p.Name)}一张最小点数手牌置底", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "wu-huangfu-song", ["target-seat"] = p.Seat.ToString(CultureInfo.InvariantCulture), ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishWuHf(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wu-huangfu-song")]
    internal void AnswerWuHf(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.WuHuangFuSong!;
        if (!ValidWuHf(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], WuHfChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Wu Huangfu Song requires his current published private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !r.Paid && r.Operation != SkillProgramEffectOp.SettleWuHuangFuSongJiangxian && !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            FinishWuHf(f);
            return;
        }

        if (r.Operation == SkillProgramEffectOp.SettleWuHuangFuSongJiangxian)
        {
            var skill = selected.Parameters["lose-skill"];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r with { Paid = true, Stage = WuHuangFuSongStage.LossChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.IsEnabled && g.SkillId == skill).ToArray())
            {
                _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongSkillRemovedEvent(f.Id, f.OwnerSeat, skill, grant.GrantId));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongJiangxianSettledEvent(f.Id, f.OwnerSeat, skill, r.Turn));
            ContinueWuHf(f.Id);
            return;
        }

        if (r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie && r.Stage == WuHuangFuSongStage.Choosing)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r with { TargetSeat = selected.Targets.Single(), Stage = WuHuangFuSongStage.ChoosingMinimumHand } });
            PublishWuHf(f, WuHfChoices(f));
            return;
        }

        Card card;
        CardLocation from;
        int rank;
        if (r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen)
        {
            var x = WuHfMinimumPool().Single(x => selected.Parameters["selection"] == "deck" ? x.From == CardLocation.DrawPile : x.Card.Id.ToString(CultureInfo.InvariantCulture) == selected.Parameters["field-card"]);
            card = x.Card;
            from = x.From;
            rank = card.Rank;
        }
        else
        {
            var target = r.TargetSeat!.Value;
            if (!WuHfVictimAvailable(f.OwnerSeat, target))
            {
                FinishWuHf(f);
                return;
            }

            card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target])[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            from = CardLocation.Hand(target);
            rank = _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[target], card);
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r with { CardId = card.Id, From = from, Rank = rank, Stage = WuHuangFuSongStage.MovementChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var to = r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DrawPile;
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, new(r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen ? WuHfObtain : WuHfBottom), m =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = current.WuHuangFuSong!;
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, current with { WuHuangFuSong = receipt with { Paid = true, To = m.To, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongMovedEvent(f.Id, card.Id, rank, from, m.To, m.Sequence));
            if (r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongBottomPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat!.Value, card.Id, rank, m.Sequence, r.Turn));
            else if (rank == 1 && m.To == CardLocation.Hand(f.OwnerSeat))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongChaozhenDisabledEvent(f.Id, f.OwnerSeat, r.Turn));
        }, disclosure: from.Zone == CardZoneKind.Hand || from == CardLocation.DrawPile ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        if (to == CardLocation.DrawPile && _cardZonesOperations._cardZones.GetLocation(card.Id) == to)
            _cardZonesOperations._cardZones.PlaceDrawPileCardsAtBottom([card.Id]);
        SealWuHf(f.Id, before);
        ContinueWuHf(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen, SkillProgramEffectOp.ResolveWuHuangFuSongLianjie, SkillProgramEffectOp.SettleWuHuangFuSongJiangxian)]
    internal PromptChoice AiWuHf(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.WuHuangFuSong?.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie && f.WuHuangFuSong.Stage == WuHuangFuSongStage.Choosing)
            return p.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets.Single(), new(0, 0, 0, 0, 0, 1, false, false))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        return p.Choices.First();
    }

    internal void ContinueWuHf(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuHuangFuSong: { } r } f || f.Id != id)
            return;
        if (!ValidWuHf(f))
            throw new InvalidOperationException("Wu Huangfu Song lost the owning paid receipt.");
        if (r.Stage is WuHuangFuSongStage.Choosing or WuHuangFuSongStage.ChoosingMinimumHand or WuHuangFuSongStage.Drawing)
            return;
        if (DrainWuHf(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WuHuangFuSong!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishWuHf(f);
            return;
        }

        if (r.Stage == WuHuangFuSongStage.MovementChildren && r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongChaozhen && r.Rank == 1 && r.To == CardLocation.Hand(f.OwnerSeat))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r with { Stage = WuHuangFuSongStage.Recovering }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
            SealWuHf(id, r.After);
            if (DrainWuHf(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                return;
            FinishWuHf(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return;
        }

        if (r.Stage == WuHuangFuSongStage.MovementChildren && r.Operation == SkillProgramEffectOp.ResolveWuHuangFuSongLianjie)
        {
            var count = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count);
            if (count == 0)
            {
                FinishWuHf(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r with { Stage = WuHuangFuSongStage.Drawing, RequestedDraw = count }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new(WuHfDraw));
            return;
        }

        FinishWuHf(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWuHuangFuSongLianjie)]
    internal void ReturnWuHfDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        var r = f.WuHuangFuSong!;
        if (!ValidWuHf(f) || r.Stage != WuHuangFuSongStage.Drawing || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.RequestedDraw)
            throw new InvalidOperationException("Lianjie returned a different draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = r with { Draw = o, Stage = WuHuangFuSongStage.DrawChildren, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongDrawReturnedEvent(f.Id, o.RecipientSeat, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter));
        foreach (var id in o.ActualDrawIds)
        {
            var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == WuHfDraw);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongLianjieEntityEvent(f.Id, f.OwnerSeat, id, move.Sequence, r.Turn));
        }

        SealWuHf(f.Id, o.SequenceBefore);
        ContinueWuHf(f.Id);
    }

    internal void FinishWuHf(ProgramSkillFrame f)
    {
        var r = f.WuHuangFuSong!;
        if (r.Operation == SkillProgramEffectOp.SettleWuHuangFuSongJiangxian && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongJiangxianSettledEvent>().Any(e => e.OwnerSeat == f.OwnerSeat && e.Turn == r.Turn))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongJiangxianSettledEvent(f.Id, f.OwnerSeat, null, r.Turn));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongCompletedEvent(f.Id, r.Operation, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuHuangFuSong = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
