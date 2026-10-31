#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduGeneralBenefitsRuntime
{
    internal void ContinueGuanduShicai(ProgramSkillFrame f)
    {
        if (f.GuanduGeneral!.Stage == "begin")
        {
            if (!_coreGuanduGeneralsOperations.GuanduShicaiReady(f.OwnerSeat, f.SkillInstanceId) || _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) == 0)
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduStage(f, "shicai-cost");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, f.OwnerSeat, "discard", discard: true));
            return;
        }

        if (f.GuanduGeneral.Stage == "shicai-cost")
        {
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, f.OwnerSeat, "discard", discard: true));
            return;
        }

        if (f.GuanduGeneral.Stage != "shicai-obtain")
            throw new InvalidOperationException("Shicai lost its paid discard.");
        var top = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault();
        f = _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
        if (top is not null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(top, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new("activity.guandu.shicai.obtain"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduShicaiObtainedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, top.Id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
    }

    internal void ContinueGuanduZezhu(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage == "begin")
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r = r with { Stage = "zezhu-take-next", Seats = CoreGameEngineGuanduGeneralsRuntime.FreezeGuanduSeats(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreGuanduActivitiesOperations.IsGuanduCommander(p.Seat)).Select(p => p.Seat)), Index = 0 });
        if (r.Stage == "zezhu-take")
        {
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduForeignGainChoices(f, r.Seats![r.Index]));
            return;
        }

        if (r.Stage == "zezhu-take-next")
        {
            if (r.Index == r.Seats!.Count)
            {
                f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Stage = "zezhu-give-next", Index = 0 });
                _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
                return;
            }

            var target = r.Seats[r.Index];
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Index = r.Index + 1 });
                _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
                return;
            }

            if (_coreGuanduGeneralsOperations.GuanduOwnedCards(target, judgment: true).Length == 0)
            {
                f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Index = r.Index + 1 });
                _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, f.OwnerSeat, 1, "zezhu-take-next");
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduStage(f, "zezhu-take");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduForeignGainChoices(f, target));
            return;
        }

        if (r.Stage is "zezhu-give-next" or "zezhu-give")
        {
            while (r.Index < r.Seats!.Count && !_coreMatchParticipantsPlayersAccess._players[r.Seats[r.Index]].IsAlive)
                f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r = r with { Index = r.Index + 1 });
            if (r.Index == r.Seats.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreGuanduGeneralsOperations.GuanduOwnedCards(f.OwnerSeat).Length == 0)
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduStage(f, "zezhu-give");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, f.OwnerSeat, "give", allowSkip: false));
            return;
        }

        throw new InvalidOperationException("Zezhu changed its commander sequence.");
    }

    internal void ContinueGuanduJieliang(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage == "begin")
        {
            var target = f.WindowContext?.SourceSeat ?? -1;
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == f.OwnerSeat)
                throw new InvalidOperationException("Jieliang lost its actual Draw owner.");
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { TargetSeat = target, Stage = "jieliang-cost" });
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, f.OwnerSeat, "discard", discard: true));
            return;
        }

        if (r.Stage == "jieliang-cost")
        {
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, f.OwnerSeat, "discard", discard: true));
            return;
        }

        if (r.Stage != "jieliang-paid")
            throw new InvalidOperationException("Jieliang lost its payment.");
        f = _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
        _coreProgramLifecycleOperations.AdjustProgramNormalDraw(f, -1);
        GuanduHandLimitPenalty(f, r.TargetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduJieliangPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
    }

    internal GuanduDiscardFactEvent[] GuanduClaimableDiscards(int target)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActorSeat: var actor } current && actor == target ? current : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualDiscardPhaseExecutionConfirmedEvent>().LastOrDefault(e => e.Phase.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Phase.ActorSeat == target)?.Phase;
        if (phase is null)
            return[];
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduDiscardFactEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.PhaseActor == target && e.PhaseToken == phase.Token && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == e.CardId && m.Sequence > e.MovementSequence && m.From == CardLocation.DiscardPile)).DistinctBy(e => e.CardId).ToArray();
    }

    internal void ContinueGuanduJieliangClaim(ProgramSkillFrame f)
    {
        var target = f.WindowContext?.SourceSeat ?? -1;
        f = _coreGuanduGeneralsOperations.GuanduUpdate(f, f.GuanduGeneral!with { TargetSeat = target, Stage = "jieliang-claim" });
        var choices = GuanduClaimableDiscards(target).Select(e => _coreGuanduGeneralChoicesOperations.GuanduChoice(f, "claim", $"获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).DisplayName)}】", [e.CardId])).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "skip", "不获得")).ToArray();
        _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, choices);
    }

    internal void GuanduHandLimitPenalty(ProgramSkillFrame f, int target)
    {
        var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, f.Id, 0, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, affectedSeat: target);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
    }

    internal void ContinueGuanduCangchu(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage == "begin")
        {
            var current = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.GuanduGrain);
            if (f.WindowContext!.Window == SkillProgramTriggerWindow.GameStarting)
            {
                _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.GuanduGrain, f.OwnerSeat, 3, f);
                _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
                _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
                return;
            }

            if (current == 0)
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
                return;
            }

            _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.GuanduGrain, f.OwnerSeat, current - 1, f);
            if (current > 1 || !_coreGuanduGeneralsOperations.GuanduSkillEnabled(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], "guandu:liangying"))
            {
                _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
                _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Stage = "liangying-enemies", Index = 0, Seats = CoreGameEngineGuanduGeneralsRuntime.FreezeGuanduSeats(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !_coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, p.Seat)).Select(p => p.Seat)) });
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1, "guandu:liangying");
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage != "liangying-enemies")
            throw new InvalidOperationException("Liangying lost its once-paid collapse.");
        if (r.Index == r.Seats!.Count)
        {
            _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
            return;
        }

        var target = r.Seats[r.Index];
        f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Index = r.Index + 1 });
        _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, target, 2, "liangying-enemies");
    }

    internal void ContinueGuanduSushou(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage == "begin")
        {
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Seats = CoreGameEngineGuanduGeneralsRuntime.FreezeGuanduSeats(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, p.Seat)).Select(p => p.Seat)) });
            _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.GuanduGrain) + 1, "sushou-give");
            return;
        }

        var choices = r.Seats!.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).SelectMany(s => _coreGuanduGeneralsOperations.GuanduOwnedCards(f.OwnerSeat).Select(c => _coreGuanduGeneralChoicesOperations.GuanduChoice(f, "give", $"将【{(c.DisplayName)}】交给 {(_coreMatchParticipantsPlayersAccess._players[s].Name)}", [c.Id], [s]))).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "skip", "分配结束")).ToArray();
        _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, choices);
    }

    internal void ContinueGuanduHengjiang(ProgramSkillFrame f)
    {
        var context = f.WindowContext!;
        if (context.DamageFrameId is not { } damage || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber < 1)
        {
            _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
            return;
        }

        _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
        GuanduHandLimitPenalty(f, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduHengjiangPaidEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, damage, context.OccurrenceIndex));
        _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
    }

    internal void ContinueGuanduHengjiangReward(ProgramSkillFrame f)
    {
        var count = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduHengjiangPaidEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        if (count == 0 || _coreGuanduGeneralsOperations.GuanduTurnDiscarded(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat))
        {
            _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
            return;
        }

        _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, f.OwnerSeat, count, "finish");
    }

    internal void ContinueGuanduXiying(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage is "begin" or "xiying-cost")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "xiying-cost");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, f.OwnerSeat, "discard", discard: true));
            return;
        }

        if (r.Stage is not ("xiying-target" or "xiying-answer"))
            throw new InvalidOperationException("Xiying lost its paid target sequence.");
        while (r.Index < r.Seats!.Count && !_coreMatchParticipantsPlayersAccess._players[r.Seats[r.Index]].IsAlive)
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r = r with { Index = r.Index + 1 });
        if (r.Index == r.Seats.Count)
        {
            _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
            return;
        }

        f = _coreGuanduGeneralsOperations.GuanduStage(f, "xiying-answer");
        var target = r.Seats[r.Index];
        var choices = _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, target, "discard", discard: true, allowSkip: false).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "ban", "本回合不能使用或打出任何牌")).ToArray();
        _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, target, choices);
    }

    internal void ContinueGuanduBeizhan(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage is "begin" or "beizhan-target")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "beizhan-target");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => _coreGuanduGeneralChoicesOperations.GuanduChoice(f, "target", $"令 {(p.Name)} 补至体力上限张手牌", targets: [p.Seat])).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "skip", "不执行")).ToArray());
            return;
        }

        if (r.Stage != "beizhan-draw")
            throw new InvalidOperationException("Beizhan lost its target before drawing.");
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduBeizhanArmedEvent(f.Id, f.OwnerSeat, r.TargetSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f)));
        _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, target.Seat, Math.Max(0, target.MaxHp - _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count), "finish");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 44001)]
    internal void ConsumeGuanduBeizhan(IGameEvent payload)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || payload is not TurnStartedEvent turn)
            return;
        var consumed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduBeizhanConsumedEvent>().Select(e => e.ProducerFrameId).ToHashSet();
        foreach (var armed in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduBeizhanArmedEvent>().Where(e => e.TargetSeat == turn.ActorSeat && e.CreatedTurn < _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && !consumed.Contains(e.FrameId)).ToArray())
        {
            var player = _coreMatchParticipantsPlayersAccess._players[turn.ActorSeat];
            var restricted = player.IsAlive && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduBeizhanConsumedEvent(armed.FrameId, player.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, restricted));
            if (!restricted)
                continue;
            var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, player.Seat, armed.FrameId, 0, armed.Source, SkillProgramCardTargetRestriction.SelfOnly, null, player.Seat == armed.OwnerSeat ? null : player.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(grant));
        }
    }

    internal void ContinueGuanduZhongjie(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage is "begin" or "zhongjie-target")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "zhongjie-target");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => _coreGuanduGeneralChoicesOperations.GuanduChoice(f, "target", $"令 {(p.Name)} 增加体力上限、回复并摸一张牌", targets: [p.Seat])).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "skip", "不执行")).ToArray());
            return;
        }

        if (r.Stage == "zhongjie-grow")
        {
            var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "zhongjie-recover");
            target.MaxHp = checked(target.MaxHp + 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(target.Seat, 1, target.MaxHp, f.SkillId));
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "zhongjie-recover")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "zhongjie-draw");
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, r.TargetSeat, 1, null, null);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage != "zhongjie-draw")
            throw new InvalidOperationException("Zhongjie changed its native ordered effects.");
        _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, r.TargetSeat, 1, "finish");
    }
}
