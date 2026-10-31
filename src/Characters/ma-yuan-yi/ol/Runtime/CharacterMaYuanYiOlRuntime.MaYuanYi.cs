#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaYuanYiOlRuntime
{
    internal const string JibingStoreReason = "skill-program.ma-yuan-yi.jibing.store", WangjingDrawReason = "skill-program.ma-yuan-yi.wangjing.draw", BinghuoBind = "ma-yuan-yi-binghuo";
    internal bool HasMaYuanYiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReplaceJibingDrawWithTwoBing);

    internal static CardConversionSource MaYuanYiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_ma_yuan_yi_ol_MaYuanYi.MaYuanYiSource(f);
    SkillProgramStepOutcome IMaYuanYiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMaYuanYi(f, op);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27370)]
    internal void RecordMaYuanYiBingAction(IGameEvent fact)
    {
        if (!HasMaYuanYiCapability || fact is not CardActionAcceptedEvent { Action: var a } || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && a.EffectiveKind != CardKind.Dodge || a.PhysicalCards is not [var cost] || cost.From.Zone != CardZoneKind.PublicPersistentPile || cost.From.OwnerSeat != a.ProviderSeat || a.ConversionChain.SingleOrDefault(s => s.SkillId == "ol:jibing" && s.OwnerSeat == a.ProviderSeat)is not { } source || source.BindingId != (CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) ? "bing-slash" : "bing-dodge") || _coreProgramEventHistoryOperations.ProgramEventHistory<MaYuanYiBingActionEvent>().Any(e => e.ActionId == a.ActionId))
            return;
        var stored = _coreProgramEventHistoryOperations.ProgramEventHistory<MaYuanYiStoredEvent>().LastOrDefault(e => e.Source.SkillInstanceId == source.SkillInstanceId && e.Source.OwnerSeat == a.ProviderSeat && e.CardId == cost.CardId && e.Pile == cost.From);
        if (stored is null || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == stored.Sequence && m.CardId == cost.CardId && m.From == CardLocation.DrawPile && m.To == cost.From && m.Reason.Value == JibingStoreReason) != 1)
            throw new InvalidOperationException("A Jibing action requires its original stored pile entity and source instance.");
        var payment = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.Sequence > stored.Sequence && m.CardId == cost.CardId && m.From == cost.From && m.To == CardLocation.Processing) ?? throw new InvalidOperationException("A Jibing accepted action requires its actual paid original pile movement.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaYuanYiBingActionEvent(a.ActionId, a.ParentActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, a.Type, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, a.OpponentSeat, a.EffectiveKind, cost.CardId, cost.From, source, stored.FrameId, stored.Sequence, payment.Sequence));
    }

    internal bool ValidBingAction(MaYuanYiBingActionEvent e)
    {
        var stored = _coreProgramEventHistoryOperations.ProgramEventHistory<MaYuanYiStoredEvent>().SingleOrDefault(s => s.FrameId == e.StoredFrameId && s.CardId == e.CardId && s.Sequence == e.StoredSequence && s.Source.OwnerSeat == e.ProviderSeat && s.Source.SkillInstanceId == e.Source.SkillInstanceId && s.Pile == e.From);
        var accepted = _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Select(x => x.Action).FirstOrDefault(a => a.ActionId == e.ActionId);
        if (stored is null || accepted is null || accepted.Type != e.Type || accepted.ActorSeat != e.ActorSeat || accepted.ProviderSeat != e.ProviderSeat || accepted.RequesterSeat != e.RequesterSeat || accepted.OpponentSeat != e.OpponentSeat || accepted.EffectiveKind != e.Kind || accepted.PhysicalCards is not [var cost] || cost.CardId != e.CardId || cost.From != e.From || !accepted.ConversionChain.Contains(e.Source) || e.Source.SkillId != "ol:jibing" || e.Source.OwnerSeat != e.ProviderSeat)
            return false;
        return e.PaymentSequence > e.StoredSequence && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.PaymentSequence && m.CardId == e.CardId && m.From == e.From && m.To == CardLocation.Processing) == 1;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27370)]
    internal bool CanOfferMaYuanYi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var e] || !MaYuanYiComposition.Handles(e.Op))
            return true;
        if (!_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || context.OwnerSeat != c.OwnerSeat)
            return false;
        return e.Op switch
        {
            SkillProgramEffectOp.ReplaceJibingDrawWithTwoBing => context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && JibingPileCards(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count < _coreRuleQueriesOperations.GetLivingFactionCount() && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { NormalDrawReplaced: false, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase },
            SkillProgramEffectOp.ResolveMoucuanAwakening => context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].MaxHp > 0 && (JibingPileCards(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count >= MoucuanFactionCount() || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(c.OwnerSeat, c.SkillId, c.BindingId)) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoucuanMaximumPaidEvent>().Any(x => x.OwnerSeat == c.OwnerSeat),
            SkillProgramEffectOp.ResolveWangjingBingOpponentDraw => WangjingQualification(c, context)is not null,
            _ => context.Window == SkillProgramTriggerWindow.TurnEnding && _coreProgramEventHistoryOperations.ProgramEventHistory<MaYuanYiBingActionEvent>().Any(x => x.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && x.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && x.ProviderSeat == c.OwnerSeat && ValidBingAction(x))};
    }

    internal SkillProgramStepOutcome BeginMaYuanYi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext;
        if (f.MaYuanYi is not null || f.InstructionIndex != 1 || f.TriggerId is null || c is null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !CanOfferMaYuanYi(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Ma Yuan Yi lost its actual native trigger, unpaid qualification or source.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var wang = op == SkillProgramEffectOp.ResolveWangjingBingOpponentDraw ? WangjingQualification(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c) : null;
        var stage = op switch
        {
            SkillProgramEffectOp.ReplaceJibingDrawWithTwoBing => MaYuanYiStage.StoreChildren,
            SkillProgramEffectOp.ResolveWangjingBingOpponentDraw => MaYuanYiStage.OpponentDrawChildren,
            SkillProgramEffectOp.ResolveMoucuanAwakening => MaYuanYiStage.MaximumChildren,
            _ => MaYuanYiStage.TargetChoice
        };
        var r = new MaYuanYiReceipt(MaYuanYiSource(f), f.GameplayHash, op, c.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, op == SkillProgramEffectOp.ResolveMoucuanAwakening ? MoucuanFactionCount() : _coreRuleQueriesOperations.GetLivingFactionCount(), JibingPileCards(owner).Count, owner.MaxHp, owner.Hp, stage, wang?.Fact.ActionId, wang?.Opponent);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaYuanYi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaYuanYiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, r.TurnOwner, r.FactionCount, r.BingBefore, r.MaximumBefore, r.HpBefore, r.ActionId, r.OpponentSeat));
        if (stage == MaYuanYiStage.StoreChildren)
        {
            var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var count = 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaYuanYi = r with { Pile = pile.Location, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            for (var i = 0; i < 2 && _corePhysicalDrawFacadesOperations.EnsureDrawPile(); i++)
            {
                var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
                var index = i;
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, pile.Location, new(JibingStoreReason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaYuanYiStoredEvent(f.Id, r.Source, index, card.Id, pile.Location, m.Sequence)));
                count++;
            }

            FinishMaYuanYiMovement(f.Id, before, count);
        }
        else if (stage == MaYuanYiStage.OpponentDrawChildren)
        {
            var opposite = wang!.Value.Opponent;
            var hp = _coreMatchParticipantsPlayersAccess._players[opposite].Hp;
            var highest = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue).Max(p => p.Hp);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MaYuanYi = r with { Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var count = _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, new(WangjingDrawReason)).Count;
            FinishMaYuanYiMovement(f.Id, before, count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangjingDrawIssuedEvent(f.Id, r.ActionId!.Value, f.OwnerSeat, opposite, hp, highest, count));
        }
        else if (stage == MaYuanYiStage.MaximumChildren)
        {
            var after = r.MaximumBefore - 1;
            var hp = Math.Min(r.HpBefore, after);
            if (after == 0)
            {
                owner.MaxHp = 0;
                owner.Hp = hp;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(owner.Seat, -1, 0, f.SkillId) { HpBeforeClamp = r.HpBefore });
            }
            else
                _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoucuanMaximumPaidEvent(f.Id, f.OwnerSeat, r.MaximumBefore, after, r.HpBefore, hp));
            if (after == 0)
            {
                _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, owner, null, null, null, causingProgramSkillFrameId: f.Id);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        ContinueMaYuanYi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void FinishMaYuanYiMovement(long id, long before, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MaYuanYi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MaYuanYi = r with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaYuanYiMovementEvent(id, r.Stage, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
    }

    internal IReadOnlyList<PromptChoice> MaYuanYiTargets(ProgramSkillFrame f) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => new PromptChoice(new($"ma-yuan-yi.{(f.Id)}.target.{(p.Seat)}"), p.Name, [], [p.Seat], new Dictionary<string, string> { { "program-action", "ma-yuan-yi" }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    internal void PublishMaYuanYiTarget(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMaYuanYi(f);
            return;
        }

        var choices = MaYuanYiTargets(f);
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "兵祸：选择一名角色判定。", [], choices.SelectMany(x => x.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ma-yuan-yi")]
    internal void AnswerMaYuanYiTarget(PromptChoice supplied)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MaYuanYi: { Stage: MaYuanYiStage.TargetChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MaYuanYiTargets(f).Any(x => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [supplied])))
            throw new InvalidOperationException("Binghuo requires its published actual target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = supplied.Targets.Single();
        var judgment = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, MaYuanYi = r with { Stage = MaYuanYiStage.Judgment, TargetSeat = target, JudgmentId = judgment } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BinghuoTargetIssuedEvent(f.Id, target, judgment));
        _ = _coreNativeJudgmentOperations.BeginJudgment(null, target, "binghuo", f.Id, null, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: BinghuoBind, programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    internal bool DrainMaYuanYi(ProgramSkillFrame f) => HasPaidMoucuanSourceLoss(f) && _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueMaYuanYi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MaYuanYi: { } r } f && f.Id == id)
        {
            if (r.Stage == MaYuanYiStage.TargetChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishMaYuanYiTarget(f);
                return;
            }

            if (r.Stage == MaYuanYiStage.Judgment || f.AttackAttempt is not null)
                return;
            if (DrainMaYuanYi(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MaYuanYi!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == MaYuanYiStage.MaximumChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaYuanYi = r with { Stage = MaYuanYiStage.GrantChildren } });
                _coreProgramSkillChangesOperations.GrantProgramSkills(f, ["ol:binghuo"]);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoucuanSkillGrantedEvent(f.Id, f.OwnerSeat));
                continue;
            }

            if (r.Stage == MaYuanYiStage.JudgmentChildren && r.JudgmentSuit is Suit.Spade or Suit.Club && r.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaYuanYi = r with { Stage = MaYuanYiStage.DamageChildren } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BinghuoDamageIssuedEvent(f.Id, f.OwnerSeat, target, 1, DamageNature.Thunder));
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1, nature: DamageNature.Thunder);
                return;
            }

            FinishMaYuanYi(f);
            return;
        }
    }

    internal void FinishMaYuanYi(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaYuanYiCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaYuanYi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27370)]
    internal bool ResumeMaYuanYi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MaYuanYi: not null } f || f.Id != id)
            return false;
        ContinueMaYuanYi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27370)]
    internal bool ReturnMaYuanYiMovement(ProgramSkillFrame f)
    {
        if (f.MaYuanYi is null)
            return false;
        ContinueMaYuanYi(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27370)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27370)]
    internal bool ContinuePaidMaYuanYi(ProgramSkillFrame f)
    {
        if (f.MaYuanYi is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBinghuoEndingJudgment)]
    internal PromptChoice ChooseMaYuanYi(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Targets is [var t] ? _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), t, new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false)) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
