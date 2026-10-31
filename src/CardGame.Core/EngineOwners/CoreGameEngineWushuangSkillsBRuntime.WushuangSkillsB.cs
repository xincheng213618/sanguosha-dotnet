#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangSkillsBRuntime
{
    internal const string BWeiyi = "wushuang:skill-3289", BZhendan = "wushuang:skill-3290", BLiantao = "wushuang:skill-3301", BBianyu = "wushuang:skill-3316", BYinglue = "wushuang:skill-3331", BXiaojie = "wushuang:skill-3333", BDingan = "wushuang:skill-3335", BChunlao = "wushuang:skill-3336";
    internal bool HasWushuangBSkills => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(BZhendan);

    internal static CardKind BCanonicalName(CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? CardKind.Slash : kind;
    internal static bool BBasic(CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || kind is CardKind.Dodge or CardKind.Peach or CardKind.Alcohol;
    internal static bool BOrdinaryTrick(CardKind kind) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(kind, SkillProgramCardCategory.Trick) && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(kind);
    internal CardConversionSource BSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool BSourceCurrent(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    internal bool BZhendanDisabled(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangZhendanDisabledEvent>().Any(e => e.OwnerSeat == seat && e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal IEnumerable<ProgramTriggerCandidate> CollectWushuangBChunlaoLossCandidates(HpChangeContext change)
    {
        if (!HasWushuangBSkills || change.Kind != HpChangeKind.Loss)
            yield break;
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _cardZonesOperations._cardZones.Count(CardLocation.Chunlao(p.Seat)) > 0))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(player, SkillProgramTriggerWindow.AfterHpLost).Where(c => c.SkillId == BChunlao))
                yield return candidate;
    }

    internal ProgramSkillWindowContext CreateWushuangBChunlaoLossContext(HpChangeContext change, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.AfterHpLost, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])with { HpChangeAmount = change.Amount, HpBeforeChange = change.HpBefore, HpAfterChange = change.HpAfter, HealthChangeKind = change.Kind }, HpChange: change);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 61200)]
    internal void ObserveWushuangBSkills(IGameEvent payload)
    {
        if (!HasWushuangBSkills)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } native && (native.Action is null || native.Action.Type == CardActionType.Use) && !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(e => e.OriginUseId == declared.ResolutionId && e.CardActionId == (native.Action?.ActionId ?? 0)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangCardUseNameRecordedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, declared.SourceSeat, BCanonicalName(declared.CardKind), native.Action?.ActionId ?? 0, declared.ResolutionId));
        if (payload is CardActionAcceptedEvent { Action: { Type: CardActionType.Response } a } && a.ActorSeat == a.ProviderSeat && a.ResponderSeat == a.ActorSeat && a.RequesterSeat is null && (a.EffectiveKind == CardKind.Nullification || a.EffectiveKind == CardKind.Dodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } defending && defending.TargetSeat == a.ActorSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(defending.EffectiveCardKind ?? CardKind.Slash) && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is null) && !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(e => e.CardActionId == a.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangCardUseNameRecordedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, a.ActorSeat, BCanonicalName(a.EffectiveKind), a.ActionId, 0));
        if (payload is ActualDiscardRecoveryPhaseStartedEvent { Phase: { Kind: ActualDiscardRecoveryPhaseKind.Draw or ActualDiscardRecoveryPhaseKind.Discard } } phase)
        {
            var kind = phase.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw ? TurnPhase.Draw : TurnPhase.Discard;
            var pending = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangYinglueArmedEvent>().Where(e => e.TargetSeat == phase.Phase.ActorSeat && e.Phase == kind && !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangYinglueAppliedEvent>().Any(applied => applied.ProgramFrameId == e.ProgramFrameId)).ToArray();
            foreach (var grant in pending)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangYinglueAppliedEvent(grant.ProgramFrameId, grant.TargetSeat, grant.Phase, grant.Amount, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        }

        if (payload is DamageAppliedEvent damage)
        {
            foreach (var frame in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.WushuangB is { Operation: SkillProgramEffectOp.ResolveWushuangLiantao, Stage: "using" }).ToArray())
            {
                var r = frame.WushuangB!;
                if (r.ChildUseId is { } useId && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Id == useId) && damage.SourceSeat == frame.OwnerSeat && damage.TargetSeat == r.TargetSeat && !damage.SourceLess)
                    _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { WushuangB = r with { Damage = r.Damage + damage.Amount } });
            }
        }

        if (payload is PlayerDyingEvent dying)
        {
            foreach (var frame in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.WushuangB is { Operation: SkillProgramEffectOp.ResolveWushuangLiantao }).ToArray())
                if (dying.VictimSeat == frame.OwnerSeat || dying.VictimSeat == frame.WushuangB!.TargetSeat)
                    _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { WushuangB = frame.WushuangB!with { EnteredDying = true } });
        }
    }

    internal IEnumerable<RuleQueryContribution> WushuangSkillsBNumericContributions(CharacterState player, SkillRuleQuery query)
    {
        if (!HasWushuangBSkills || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != player.Seat || !((_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && query == SkillRuleQuery.DrawCount) || (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Discard && query == SkillRuleQuery.HandLimit)))
            yield break;
        foreach (var item in _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangYinglueAppliedEvent>().Where(e => e.TargetSeat == player.Seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Phase == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase))
            yield return new FiniteRuleQueryContribution($"wushuang:yinglue:{(item.ProgramFrameId)}", SkillRuleOperation.Add, item.Amount);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 61200)]
    internal bool IsWushuangZhendanConversionForbidden(ProgramConfiguredViewAsQuery q) => q.Instance.SkillId == BZhendan && (BZhendanDisabled(q.Owner.Seat) || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.ActorSeat == q.Owner.Seat && e.Name == BCanonicalName(q.Rule.OutputKind)));
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 61200)]
    internal bool IsWushuangZhendanSubmittedConversionForbidden(ProgramViewAsLegalityQuery q) => q.Source.SkillId == BZhendan && (BZhendanDisabled(q.Owner.Seat) || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.ActorSeat == q.Owner.Seat && e.Name == BCanonicalName(q.Kind)));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 61200)]
    internal bool CanTriggerWushuangBSkill(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (c.SkillId == BWeiyi)
            return context.TargetSeat is { } target && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangWeiyiUsedEvent>().Any(e => e.OwnerSeat == c.OwnerSeat && e.SkillId == c.SkillId && e.TargetSeat == target);
        if (c.SkillId == BZhendan)
            return !BZhendanDisabled(c.OwnerSeat);
        if (c.SkillId == BXiaojie && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveWushuangXiaojie))
            return context.CardUse is { } card && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangXiaojieAcceptedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.IncomingUseId == card.ParentCardUseFrameId);
        if (c.SkillId == BDingan)
            return context.CardUse is { } use && BSecondTurnUse(use.CardActionId, use.EffectiveKind) && !BDyingThisTurn() && BDinganDrawTargets(c.OwnerSeat, BTargets(use)).Any();
        return true;
    }

    internal IEnumerable<IGameEvent> BTurnHistory() => _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent);
    internal IReadOnlyList<int> BTargets(ProgramCardUseContext use) => use.DesignatedTargetSeats ?? _coreCardUseLifecycleOperations.LifecycleCardUse(use.ParentCardUseFrameId)?.TargetSeats ?? [];
    internal bool BDyingThisTurn() => BTurnHistory().OfType<PlayerDyingEvent>().Any();
    internal bool BSecondTurnUse(long actionId, CardKind kind)
    {
        var declared = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Name == BCanonicalName(kind)).ToArray();
        return declared.Length == 2 && declared[^1].CardActionId == actionId;
    }

    internal IEnumerable<int> BDinganDrawTargets(int owner, IReadOnlyList<int> targets) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !targets.Contains(p.Seat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangDinganDrawnEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == p.Seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)).Select(p => p.Seat);
    SkillProgramStepOutcome global::CardGame.Core.IWushuangSkillsBHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWushuangB(f, op);
    internal SkillProgramStepOutcome BeginWushuangB(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!BSourceCurrent(f))
            return SkillProgramStepOutcome.Continue;
        if (f.WushuangB is not null)
            throw new InvalidOperationException("A Wushuang instruction cannot repeat its pending operation.");
        if (op == SkillProgramEffectOp.DrawWushuangZhendan)
        {
            if (BZhendanDisabled(f.OwnerSeat))
                return SkillProgramStepOutcome.Continue;
            var count = Math.Min(5, _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is RoundStartedEvent).OfType<TurnStartedEvent>().Count());
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangZhendanDisabledEvent(f.OwnerSeat, f.SkillId, _coreActualRoundProgressRoundNumberAccess._roundNumber, f.Id));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = new(f.InstructionIndex, op, "complete") });
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.zhendan.draw"));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ArmWushuangYinglue)
        {
            var target = f.SelectedTargetSeats.Single();
            var draw = f.ActivationId == "lose-hp-next-draw";
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangYinglueArmedEvent(f.Id, f.OwnerSeat, target, draw ? TurnPhase.Draw : TurnPhase.Discard, draw ? 2 : -2));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.ArmWushuangXiaojie)
        {
            var incoming = f.WindowContext?.CardUse?.ParentCardUseFrameId ?? throw new InvalidOperationException("Xiaojie lost its original incoming Slash.");
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || attack.ResolutionId != incoming || attack.TargetSeat != f.OwnerSeat)
                throw new InvalidOperationException("Xiaojie requires its actual incoming Slash target.");
            attack.ProhibitDodgeBy(_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangXiaojieAcceptedEvent(f.Id, BSource(f), f.GameplayHash, incoming));
            return SkillProgramStepOutcome.Continue;
        }

        var targetSeat = op == SkillProgramEffectOp.ResolveWushuangGuzheng ? f.WindowContext?.SourceSeat ?? -1 : op is SkillProgramEffectOp.ResolveWushuangWeiyi or SkillProgramEffectOp.ResolveWushuangBianyu ? f.WindowContext?.TargetSeat ?? -1 : -1;
        var stage = op switch
        {
            SkillProgramEffectOp.ResolveWushuangWeiyi => "health",
            SkillProgramEffectOp.ResolveWushuangLiantao => "target",
            SkillProgramEffectOp.ResolveWushuangBianyu => "mark",
            SkillProgramEffectOp.ResolveWushuangXiaojie => "trick",
            SkillProgramEffectOp.ResolveWushuangDingan => "draw-target",
            SkillProgramEffectOp.RecoverWushuangChunlao => "chun",
            SkillProgramEffectOp.ResolveWushuangGuzheng => "guzheng-return",
            _ => throw new InvalidOperationException("Unknown Wushuang B instruction.")};
        var previous = op == SkillProgramEffectOp.ResolveWushuangXiaojie ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().LastOrDefault(e => e.SourceSeat == f.OwnerSeat && BOrdinaryTrick(e.CardKind))?.CardKind : null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = new(f.InstructionIndex, op, stage, targetSeat, Before: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, OutputKind: previous) });
        if (op == SkillProgramEffectOp.ResolveWushuangBianyu && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive))
        {
            FinishWushuangB(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveWushuangXiaojie && previous is null)
            return DrawWushuangBFallback(f, 1);
        PublishWushuangBChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice BChoice(ProgramSkillFrame f, string step, string key, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"wushuang-b.{(f.Id)}.{(step)}.{(key)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "wushuang-b", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["step"] = step, ["value"] = key });
    internal IReadOnlyList<PromptChoice> WushuangBChoices(ProgramSkillFrame f)
    {
        var r = f.WushuangB!;
        var list = new List<PromptChoice>();
        if (r.Stage == "health")
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Hp >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp)
                list.Add(BChoice(f, r.Stage, "lose", "令其失去1点体力"));
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Hp <= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp)
                list.Add(BChoice(f, r.Stage, "recover", "令其回复1点体力"));
        }

        if (r.Stage == "target")
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                list.Add(BChoice(f, r.Stage, p.Seat.ToString(CultureInfo.InvariantCulture), p.Name, targets: [p.Seat]));
        if (r.Stage == "color")
        {
            list.Add(BChoice(f, r.Stage, "red", "红色"));
            list.Add(BChoice(f, r.Stage, "black", "黑色"));
        }

        if (r.Stage == "mark")
        {
            var maximum = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Hp);
            if (r.Cards.Count < maximum)
            {
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]);
                for (var i = 0; i < hand.Count; i++)
                    if (!r.Cards.Contains(hand[i].Id))
                        list.Add(BChoice(f, r.Stage, i.ToString(CultureInfo.InvariantCulture), r.TargetSeat == f.OwnerSeat ? $"{(hand[i].DisplayName)} {(hand[i].Suit)}{(hand[i].Rank)}" : $"手牌位置 {(i + 1)}", r.TargetSeat == f.OwnerSeat ? [hand[i].Id] : []));
            }

            list.Add(BChoice(f, r.Stage, "done", "完成选择"));
        }

        if (r.Stage == "chun")
        {
            if (r.Cards.Count < 2)
                foreach (var card in _cardZonesOperations._cardZones.CardsAt(CardLocation.Chunlao(f.OwnerSeat)).Where(c => !r.Cards.Contains(c.Id)))
                    list.Add(BChoice(f, r.Stage, card.Id.ToString(CultureInfo.InvariantCulture), $"{(card.DisplayName)} {(card.Suit)}{(card.Rank)}", [card.Id]));
            list.Add(BChoice(f, r.Stage, "done", "将已选的醇置入手牌"));
        }

        if (r.Stage == "trick")
        {
            if (r.OutputKind is { } kind)
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], kind, Suit.None, enforceUsePermission: true, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []))
                    list.Add(BChoice(f, r.Stage, option.Id.Value, $"视为使用【{(CardCatalog.Get(kind).DisplayName)}】 {(string.Join("、", option.TargetSeats.Select(s => _coreMatchParticipantsPlayersAccess._players[s].Name)))}", targets: option.TargetSeats));
            list.Add(BChoice(f, r.Stage, "draw", "摸一张牌"));
        }

        if (r.Stage == "draw-target")
            foreach (var seat in BDinganDrawTargets(f.OwnerSeat, BTargets(f.WindowContext!.CardUse!)))
                list.Add(BChoice(f, r.Stage, seat.ToString(CultureInfo.InvariantCulture), $"与{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}各摸一张牌", targets: [seat]));
        if (r.Stage == "punish")
        {
            var others = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).ToArray();
            var max = others.Select(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count).DefaultIfEmpty().Max();
            foreach (var p in others.Where(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count == max))
            {
                list.Add(BChoice(f, r.Stage, $"damage-{(p.Seat)}", $"对{(p.Name)}造成1点伤害", targets: [p.Seat]));
                list.Add(BChoice(f, r.Stage, $"discard-{(p.Seat)}", $"令{(p.Name)}弃置手牌中最多的同名牌", targets: [p.Seat]));
            }
        }

        if (r.Stage == "discard-name")
        {
            var groups = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).GroupBy(c => BCanonicalName(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], c))).ToArray();
            var max = groups.Select(g => g.Count()).DefaultIfEmpty().Max();
            foreach (var group in groups.Where(g => g.Count() == max))
                list.Add(BChoice(f, r.Stage, group.Key.ToString(), $"弃置{(group.Count())}张【{(CardCatalog.Get(group.Key).DisplayName)}】", group.Select(c => c.Id).ToArray()));
        }

        _coreWushuangSkillsBAuditOperations.AddWushuangGuzhengChoices(f, list);
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishWushuangBChoice(ProgramSkillFrame f)
    {
        var r = f.WushuangB!;
        var chooser = r.Stage is "color" or "discard-name" ? r.TargetSeat : f.OwnerSeat;
        var choices = WushuangBChoices(f);
        if (choices.Count == 0)
        {
            FinishWushuangB(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("wushuang-b")]
    internal void AnswerWushuangB(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Wushuang choice lost its program.");
        var r = f.WushuangB ?? throw new InvalidOperationException("Wushuang choice lost its pending instruction.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !WushuangBChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(selected.Parameters.OrderBy(p => p.Key))))
            throw new InvalidOperationException("Wushuang choice is not its current published option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!BSourceCurrent(f))
        {
            FinishWushuangB(f);
            return;
        }

        if (_coreWushuangSkillsBAuditOperations.AnswerWushuangGuzheng(f, selected))
            return;
        var value = selected.Parameters["value"];
        if (r.Stage == "health")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangWeiyiUsedEvent(f.OwnerSeat, f.SkillId, r.TargetSeat, f.Id));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "complete" } });
            if (value == "lose")
            {
                var outcome = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, r.TargetSeat, 1);
                if (outcome == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            }
            else
            {
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, r.TargetSeat, 1, null, null);
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            }

            return;
        }

        if (r.Stage == "target")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "color", TargetSeat = selected.Targets.Single() } });
            PublishWushuangBChoice(f);
            return;
        }

        if (r.Stage == "color")
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(hand.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray())));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "duels", Red = value == "red", Cards = hand.Where(c => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c)) == (value == "red")).Select(c => c.Id).ToArray() } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage == "mark")
        {
            if (value != "done")
            {
                var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat])[int.Parse(value, CultureInfo.InvariantCulture)];
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Cards = r.Cards.Append(card.Id).ToArray() } });
                PublishWushuangBChoice(f);
                return;
            }

            foreach (var id in r.Cards)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangBianyuMarkedEvent(f.Id, BSource(f), f.GameplayHash, r.TargetSeat, id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence));
            var allSlash = new[]
            {
                f.OwnerSeat,
                r.TargetSeat
            }.Distinct().Any(seat => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).All(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], c))));
            if (allSlash)
            {
                DrawWushuangBFallback(f, 2);
                return;
            }

            FinishWushuangB(f);
            return;
        }

        if (r.Stage == "trick")
        {
            if (value == "draw")
            {
                DrawWushuangBFallback(f, 1);
                return;
            }

            var option = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], r.OutputKind, Suit.None, enforceUsePermission: true, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).Single(o => o.Id.Value == value && o.TargetSeats.SequenceEqual(selected.Targets));
            IssueWushuangBTrick(f, option, null);
            return;
        }

        if (r.Stage == "chun")
        {
            if (value != "done")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } });
                PublishWushuangBChoice(f);
                return;
            }

            if (r.Cards.Count == 0)
            {
                FinishWushuangB(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "complete", TargetSeat = f.OwnerSeat, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCards(r.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.Chunlao(f.OwnerSeat), CardLocation.Hand(f.OwnerSeat), new("skill-program.chunlao.return"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangB = f.WushuangB!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage == "draw-target")
        {
            var target = selected.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangDinganDrawnEvent(f.OwnerSeat, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.Id));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "draw-children", TargetSeat = target } });
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.dingan.draw-owner"));
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, target, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.dingan.draw-target"));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage == "punish")
        {
            var target = selected.Targets.Single();
            if (value.StartsWith("damage-", StringComparison.Ordinal))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "complete", TargetSeat = target } });
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Damage(f, target, 1);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "discard-name", TargetSeat = target } });
            PublishWushuangBChoice(f);
            return;
        }

        if (r.Stage == "discard-name")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "complete", Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
            foreach (var id in selected.Cards)
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, r.TargetSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(r.TargetSeat), new("skill-program.dingan.discard"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangB = f.WushuangB!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        throw new InvalidOperationException("Unsupported Wushuang pending choice.");
    }

    internal SkillProgramStepOutcome DrawWushuangBFallback(ProgramSkillFrame f, int count)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { WushuangB = f.WushuangB!with { Stage = "complete" } });
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.wushuang-b.draw"));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void FinishWushuangB(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { WushuangB = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 61200)]
    internal bool ContinueWushuangB(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.WushuangB is not { } r)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return true;
        if (r.Stage is "health" or "target" or "color" or "mark" or "chun" or "trick" or "draw-target" or "punish" or "discard-name" or "guzheng-return" or "guzheng-claim")
        {
            PublishWushuangBChoice(f);
            return true;
        }

        if (r.Stage == "using")
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WushuangB!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == "guzheng-returned")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "guzheng-claim" } });
            PublishWushuangBChoice(f);
            return true;
        }

        if (r.Stage == "draw-children")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "punish" } });
            PublishWushuangBChoice(f);
            return true;
        }

        if (r.Stage == "duels")
        {
            if (!BSourceCurrent(f) || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || r.EnteredDying || r.Cards.Count == 0)
            {
                FinishWushuangLiantao(f);
                return true;
            }

            var next = r.Cards.First();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Cards = r.Cards.Skip(1).ToArray() } });
            if (_cardZonesOperations._cardZones.GetLocation(next) != CardLocation.Hand(f.OwnerSeat))
            {
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return true;
            }

            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(next);
            var option = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Duel, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), enforceUsePermission: true, physicalCardIds: [next]).FirstOrDefault(o => o.TargetSeats.SequenceEqual([r.TargetSeat]));
            if (option is null)
            {
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return true;
            }

            IssueWushuangBTrick(f, option, card);
            return true;
        }

        FinishWushuangB(f);
        return true;
    }

    internal void FinishWushuangLiantao(ProgramSkillFrame f)
    {
        var r = f.WushuangB!;
        if (r.Damage == 0)
        {
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().GrantTurnRuleModifier(f, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, 1, []);
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().GrantTurnCardActionProhibition(f, global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds, [CardActionType.Use]);
        }

        DrawWushuangBFallback(f, r.Damage == 0 ? 1 : r.Damage);
    }

    internal void IssueWushuangBTrick(ProgramSkillFrame f, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption option, Card? physical)
    {
        var r = f.WushuangB!;
        var card = physical ?? new Card(0, option.EffectiveCardKind, Suit.None, 0);
        var nextId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "using", ChildUseId = nextId } });
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, option.TargetSeats, option.EffectiveCardKind, physicalCardIds: physical is null ? [] : [physical.Id], conversionSource: physical is null ? null : BSource(f));
        var ret = new WushuangBNativeReturn(f.Id, f.InstructionIndex, BSource(f), f.GameplayHash, useId, option.EffectiveCardKind);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { WushuangBReturn = ret });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangBNativeIssuedEvent(ret));
        if (physical is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(physical, CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, option.TargetSeats, option.ActionKind, option.TargetCardId, option.RequiredCardKind, option.EffectiveCardKind);
    }

    internal bool IsWushuangBZeroUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { WushuangBReturn: not null, CardId: 0, PhysicalCardIds.Count: 0 };
    [NativeUseReturn(61200)]
    internal bool ReturnWushuangBNativeUse(CardUseFrame use)
    {
        if (use.WushuangBReturn is not { } ret)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != ret.ProgramFrameId || f.WushuangB is not { Stage: "using" } r || r.ChildUseId != use.Id || ret.CardUseFrameId != use.Id || ret.Source != BSource(f) || ret.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Wushuang native use lost its exact typed parent return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangBNativeReturnedEvent(ret));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangB = r with { Stage = r.Operation == SkillProgramEffectOp.ResolveWushuangLiantao ? "duels" : "complete", ChildUseId = null } });
        // The native attack cleanup owns its response state until it returns.
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == use.Id)
            return true;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 61200)]
    internal bool ReturnWushuangBNativeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WushuangB: { } r } f || !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangBNativeReturnedEvent>().Any(e => e.Return.ProgramFrameId == f.Id && e.Return.CardUseFrameId == completion.ResolutionId))
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 61200)]
    internal bool ReturnWushuangBMovement(ProgramSkillFrame f)
    {
        if (f.WushuangB is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal WushuangBianyuMarkedEvent? BianyuActiveMark(int seat, int id)
    {
        if (id <= 0 || !HasWushuangBSkills || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(seat))
            return null;
        var mark = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangBianyuMarkedEvent>().LastOrDefault(e => e.TargetSeat == seat && e.CardId == id);
        if (mark is null || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > mark.MovementSequence && m.CardId == id && m.From == CardLocation.Hand(seat)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId > mark.ActionSequence && e.Action.ActorSeat == seat && (e.Action.Type == CardActionType.Use || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(use => use.CardActionId == e.Action.ActionId)) && !BBasic(e.Action.EffectiveKind)))
            return null;
        return mark;
    }

    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangBianyuIdentity(CharacterState owner, Card card)
    {
        if (BianyuActiveMark(owner.Seat, card.Id)is not { } mark)
            return null;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(mark.Source.SkillId).Program!;
        return new(program, new SkillProgramCardIdentity("bianyu-slash", [CardZoneKind.Hand], [], [], CardKind.Slash, new(SkillProgramConditionKind.Always, 0, [])), mark.Source with { BindingId = "bianyu-slash" });
    }

    internal bool HasWushuangBianyuPotential(CharacterState owner) => HasWushuangBSkills && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(c => BianyuActiveMark(owner.Seat, c.Id)is not null);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 61200)]
    internal bool WushuangBianyuQuota(ProgramSlashQuotaQuery q) => q.PhysicalCardIds is [var id] && BianyuActiveMark(q.Actor.Seat, id)is not null || q.PhysicalCard is { } card && BianyuActiveMark(q.Actor.Seat, card.Id)is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveWushuangWeiyi, SkillProgramEffectOp.ResolveWushuangLiantao, SkillProgramEffectOp.ResolveWushuangBianyu, SkillProgramEffectOp.ResolveWushuangXiaojie, SkillProgramEffectOp.ResolveWushuangDingan, SkillProgramEffectOp.RecoverWushuangChunlao, SkillProgramEffectOp.ResolveWushuangGuzheng)]
    internal PromptChoice ChooseWushuangBAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("value") == "done" && (f.WushuangB!.Stage == "chun" ? f.WushuangB.Cards.Count >= 2 : f.WushuangB.Cards.Count >= Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.WushuangB.TargetSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.WushuangB.TargetSeat].Hp))) ?? p.Choices[0];
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 10, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveWushuangChunlao(ProgramTriggerCandidate candidate, CharacterState owner, HpChangeContext change) => candidate.SkillId == BChunlao ? change.Kind == HpChangeKind.Loss && _cardZonesOperations._cardZones.Count(CardLocation.Chunlao(owner.Seat)) > 0 : null;
}
