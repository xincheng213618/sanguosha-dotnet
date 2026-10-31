#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangCombatTacticsRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 61400)]
    internal void RecordWushuangCombatUse(CardUseDeclaredEvent e)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatUseFactEvent>().Any(f => f.CardUseFrameId == e.ResolutionId))
            return;
        var action = _coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)?.Action;
        var kind = action?.EffectiveKind ?? e.CardKind;
        var ordinal = CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? 1 + _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatUseFactEvent>().Count(f => f.OwnerSeat == e.SourceSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(f.Kind)) : 0;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangCombatUseFactEvent(e.ResolutionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, e.SourceSeat, kind, action?.EffectiveSuit ?? e.PublicCard?.Suit ?? Suit.None, ordinal, _coreMatchParticipantsPlayersAccess._players[e.SourceSeat].HasAlcoholEffect));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 61401)]
    internal void RecordWushuangCombatDamage(DamageAppliedEvent e)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault()is not { } f || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatDamageFactEvent>().Any(previous => previous.DamageFrameId == f.Id))
            return;
        var request = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageRequestedEvent>().LastOrDefault(r => r.ResolutionId == f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangCombatDamageFactEvent(f.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, e.SourceSeat, e.TargetSeat, e.Amount, request?.SourceCard, e.Nature, e.SourceLess));
    }

    internal bool IsWushuangActualDraw(ProgramSkillWindowContext? c, int owner, out CardMovementRecord? movement)
    {
        movement = c?.MovementBatch is { } batch && c.MovementIndex is { } index && index >= 0 && index < batch.Movements.Count ? batch.Movements[index] : null;
        if (movement is not { From.Zone: CardZoneKind.DrawPile } m || m.To != CardLocation.Hand(owner) || m.Reason == CardMoveReasons.InitialDeal)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Any(draw => draw.OwnerSeat == owner && draw.SequenceBefore < m.Sequence && draw.SequenceAfter >= m.Sequence && draw.Reason == m.Reason);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 61400)]
    internal bool AllowsWushuangCombatTrigger(ProgramTriggerLegalityContext gate)
    {
        if (!gate.SkillId.StartsWith("wushuang:tactic-", StringComparison.Ordinal) || !int.TryParse(gate.SkillId.AsSpan("wushuang:tactic-".Length), out var id))
            return true;
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode)
            return false;
        if (gate.WindowContext is not { } c)
            return true;
        if (c.Window == SkillProgramTriggerWindow.SkillsChanged)
            return true; // The acquisition bridge checks its exact paid parent.
        if (id is 2104 or 2143 or 2367)
        {
            if (!IsWushuangActualDraw(c, gate.OwnerSeat, out var movement))
                return false;
            if (id == 2367)
            {
                var lucky = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangLuckyNumberEvent>().LastOrDefault(e => e.OwnerSeat == gate.OwnerSeat);
                return lucky is not null && _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(movement!.CardId)).Single(card => card.Id == movement.CardId).Rank == lucky.Number;
            }

            return true;
        }

        if (id is 2120 or 2121 or 2122)
            return _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != gate.OwnerSeat && c.CardUse is { } use && use.ActorSeat != gate.OwnerSeat && use.DesignatedTargetSeats is [var only] && only == gate.OwnerSeat && (id == 2120 || id == 2121 && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.EffectiveKind) == SkillProgramCardCategory.Basic || id == 2122 && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.EffectiveKind) == SkillProgramCardCategory.Trick);
        if (id == 2116)
            return c.SourceSeat is { } source && source != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[source].IsAlive;
        if (id is 2174 or 2175)
            return _coreWushuangDyingExitOperations.IsWushuangDyingExitContext(gate.Candidate, c);
        if (id == 2130)
            return c.Amount >= 3 && c.TargetSeat is { } t && t != gate.OwnerSeat;
        if (id == 2131)
            return _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != gate.OwnerSeat && c.Amount >= 3 && c.SourceSeat is { } s && s != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
        if (id is 2245 or 2246 or 2247)
            return _coreActualRoundProgressRoundNumberAccess._roundNumber == (id == 2247 ? 2 : 3);
        if (id == 2337)
            return c.CardUse is { } original && _coreCardUseLifecycleOperations.LifecycleCardUse(original.ParentCardUseFrameId)is { ExtraCardEffect: null, CardKind: CardKind.Snatch };
        return true;
    }

    [ProgramTargetRule(ProgramTargetRulePoint.TargetCountContributions, 61400)]
    internal RuleQueryContribution? WushuangCombatExtraTargets(ProgramTargetCountQuery q)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode)
            return null;
        if (q.Kind == CardKind.IronChain && _coreWushuangTacticRulesOperations.HasWushuangTactic(q.Actor.Seat, 2007))
            return new FiniteRuleQueryContribution("wushuang:iron-chain-all", SkillRuleOperation.Add, Math.Max(0, _coreMatchDefinitionPlayerCountAccess._playerCount - 2));
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatUseFactEvent>().Any(f => f.OwnerSeat == q.Actor.Seat && f.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && CoreNativeCardClassificationsRuntime.IsSlashCard(f.Kind)))
            return null;
        var amount = (_coreWushuangTacticRulesOperations.HasWushuangTactic(q.Actor.Seat, 2033) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(q.Actor.Seat, 2034) ? 2 : 0);
        return amount == 0 ? null : new FiniteRuleQueryContribution("wushuang:first-round-slash-targets", SkillRuleOperation.Add, amount);
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 61400)]
    internal int WushuangCombatDamageAmount(GameEngine.IDamageAttempt attack)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode)
            return 0;
        var bonus = 0;
        var source = attack.SourceSeat;
        var target = attack.TargetSeat;
        var facts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatDamageFactEvent>();
        if (!attack.IsSourceLess && !attack.IsChainPropagation)
        {
            if (attack.EffectiveCardKind is null)
                bonus += (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2171) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2172) ? 2 : 0);
            if (!attack.IsChainPropagation && attack.EffectiveCardKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            {
                var use = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatUseFactEvent>().LastOrDefault(f => f.CardUseFrameId == attack.ResolutionId);
                if (use?.SlashOrdinal % 3 == 0)
                    bonus += (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2042) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2043) ? 2 : 0);
                if (use?.Suit == Suit.Heart && _coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2045))
                    bonus += 2;
                if (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2115) && source == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
                {
                    var previous = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().LastOrDefault(t => t.ActorSeat == source && t.TurnNumber < _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
                    if (previous is not null && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatUseFactEvent>().Any(f => f.OwnerSeat == source && f.Turn == previous.TurnNumber && CoreNativeCardClassificationsRuntime.IsSlashCard(f.Kind)))
                        bonus++;
                }

                if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } borrowedAttack } borrowed && borrowedAttack.ResolutionId == attack.ResolutionId && _coreWushuangTacticRulesOperations.HasWushuangTactic(borrowed.SourceSeat, 2244))
                    bonus++;
            }

            if (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2127))
            {
                var tail = facts.SkipWhile(f => !ReferenceEquals(f, facts.LastOrDefault(x => x.SourceSeat == source && !x.SourceLess))).Skip(facts.Any(f => f.SourceSeat == source && !f.SourceLess) ? 1 : 0);
                if (!facts.Any(f => f.SourceSeat == source && !f.SourceLess))
                    tail = facts;
                bonus += tail.Count(f => f.TargetSeat == source && f.Amount > 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().LastOrDefault(t => t.TurnNumber == f.Turn)?.ActorSeat != source);
            }

            if (_coreActualRoundProgressRoundNumberAccess._roundNumber > 1)
            {
                var previousRoundDamage = facts.Where(f => f.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber - 1 && f.SourceSeat == source && !f.SourceLess).Sum(f => f.Amount);
                bonus += _coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2188) && previousRoundDamage == 0 ? 1 : 0;
                bonus += _coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2189) && previousRoundDamage < 3 ? 1 : 0;
            }

            if (source == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !facts.Any(f => f.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && f.SourceSeat == source && !f.SourceLess))
                bonus += (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2333) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2334) ? 2 : 0);
            if (attack.EffectiveCardKind is null || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(attack.EffectiveCardKind.Value) == SkillProgramCardCategory.Trick)
                bonus += (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2357) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 2 ? 2 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2358) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 4 ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2359) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 3 ? 1 : 0);
        }

        if (target != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !facts.Any(f => f.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && f.TargetSeat == target))
            bonus += (_coreWushuangTacticRulesOperations.HasWushuangTactic(target, 2333) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(target, 2334) ? 2 : 0);
        var received = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangIncomingReductionIssuedEvent>().Count(f => f.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && f.TargetSeat == target);
        if (_coreWushuangTacticRulesOperations.HasWushuangTactic(target, 2196) || _coreWushuangTacticRulesOperations.HasWushuangTactic(target, 2197))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangIncomingReductionIssuedEvent(attack.ResolutionId, target, _coreActualRoundProgressRoundNumberAccess._roundNumber, received + 1));
        if (_coreWushuangTacticRulesOperations.HasWushuangTactic(target, 2196) && received < 2)
            bonus--;
        if (_coreWushuangTacticRulesOperations.HasWushuangTactic(target, 2197) && received < 3)
            bonus -= 2;
        return bonus;
    }

    internal bool WushuangFixedDamage(GameEngine.IDamageAttempt attack) => !attack.IsSourceLess && _coreWushuangTacticRulesOperations.HasWushuangTactic(attack.SourceSeat, 2210);
    [ProgramDamageHook(ProgramDamagePhase.AfterEquipmentChoices, 61400)]
    internal ProgramDamageInterception WushuangNormalizeFixedDamage(GameEngine.IDamageAttempt attack, int amount, DamageNature nature)
    {
        if (!WushuangFixedDamage(attack))
            return ProgramDamageInterception.Continue;
        if (attack.DamageAmount > 2)
            attack.ReduceFinalizedDamageAmount(attack.DamageAmount - 2);
        if (attack.DamageAmount < 2)
            attack.IncreaseFinalizedDamageAmount(2 - attack.DamageAmount);
        return ProgramDamageInterception.Continue;
    }

    internal bool WushuangArmorBypass(GameEngine.IDamageAttempt attack) => !attack.IsSourceLess && _coreWushuangTacticRulesOperations.HasWushuangTactic(attack.SourceSeat, 2177);
    internal int WushuangArmorDamageMultiplier(GameEngine.IDamageAttempt attack) => !attack.IsSourceLess && _coreWushuangTacticRulesOperations.HasWushuangTactic(attack.SourceSeat, 2193) ? 2 : 1;
    internal bool WushuangDrunkenSlashUnrespondable(NativeOwners.CardAttackHandle attack) => _coreWushuangTacticRulesOperations.HasWushuangTactic(attack.CardUserSeat, 2001) && (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ConsumedAlcoholBoost == true || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCombatUseFactEvent>().LastOrDefault(e => e.CardUseFrameId == attack.ResolutionId)is { Drunken: true });
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 61400)]
    internal void WushuangExtraAlcoholQuota(JianjiOrdinaryAlcoholPaidEvent e)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || !e.CountsQuota)
            return;
        var extra = (_coreWushuangTacticRulesOperations.HasWushuangTactic(e.ActorSeat, 2108) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(e.ActorSeat, 2109) ? 2 : 0);
        if (extra > 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JianjiOrdinaryAlcoholPaidEvent>().Count(p => p.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.ActorSeat == e.ActorSeat && p.CountsQuota) < 1 + extra)
            _coreMatchParticipantsPlayersAccess._players[e.ActorSeat].UsedPlayPhaseAlcoholThisTurn = false;
    }

    internal int WushuangDrawTwoCount(int sourceSeat) => _coreWushuangTacticRulesOperations.HasWushuangTactic(sourceSeat, 2030) ? 3 : 2;
    internal bool WushuangHasVisibleTargetHand(int source, CardKind kind) => kind == CardKind.Dismantlement && _coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2005) || kind == CardKind.Snatch && _coreWushuangTacticRulesOperations.HasWushuangTactic(source, 2006) || _coreTargetHandVisibilityOperations.Value.HasProgramVisibleTargetHandDuringTrick(source, kind);
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 61400)]
    internal IEnumerable<CardSnapshot> WushuangVisibleTargetHand(int viewer) => _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is { } selection && selection.SourceSeat == viewer && WushuangHasVisibleTargetHand(viewer, selection.EffectCardKind) ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[selection.TargetSeat]).Select(_coreSnapshotValueProjectionOperations.ToSnapshot) : [];
}
