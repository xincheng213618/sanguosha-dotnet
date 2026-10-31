#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangRenMobileShiRuntime
{
    internal bool HasShiZrCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiZrFuan);

    internal GameEngine.ShiZrFuanRecordInfo? ShiZrFuanRecord(int seat)
    {
        GameEngine.ShiZrFuanRecordInfo? record = null;
        foreach (var payload in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (payload is ShiZrFuanRecordedEvent { OwnerSeat: var owner } rec && owner == seat)
                record = new(rec.Suit, rec.ViaRelaunch);
            else if (payload is ShiZrFuanClearedEvent { OwnerSeat: var cleared } && cleared == seat)
                record = null;
        }

        return record;
    }

    internal int ShiZrYinXianBonus(int seat)
    {
        var bonus = 0;
        foreach (var payload in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (payload is ShiZrFuanArmedEvent { OwnerSeat: var owner } && owner == seat)
                bonus = 0;
            else if (payload is ShiZrYinXianBonusGainedEvent { OwnerSeat: var gained } gain && gained == seat)
                bonus = gain.Bonus;
        }

        return bonus;
    }

    internal bool ShiZrFuanZeroed(int seat) => HasShiZrCapability && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShiZrFuanArmedEvent>().Any(armed => armed.OwnerSeat == seat);
    internal int ShiZrYinXianRangeBonus(CharacterState player) => HasShiZrCapability ? ShiZrYinXianBonus(player.Seat) : 0;
    internal bool ShiZrFuanBaseZeroed(CharacterState player) => HasShiZrCapability && ShiZrFuanZeroed(player.Seat);
    internal bool ShiZrYinXianFarthest(int user, int target)
    {
        if (!HasShiZrCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[user].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[user], ShiZhangRenComposition.YinXianSkill))
            return false;
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(user);
        if (range <= 0)
            return false;
        var farthest = -1;
        for (var seat = 0; seat < _coreMatchParticipantsPlayersAccess._players.Count; seat++)
        {
            if (seat == user || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                continue;
            var distance = _coreNativeRuleQueriesOperations.GetCombatDistance(user, seat);
            if (distance >= 1 && distance <= range && distance > farthest)
                farthest = distance;
        }

        return farthest >= 1 && _coreNativeRuleQueriesOperations.GetCombatDistance(user, target) == farthest;
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 68200)]
    internal int ShiZrYinXianDamageBonus(GameEngine.IDamageAttempt attack) => attack is NativeOwners.CardAttackHandle card && card.CardUserSeat is { } user && user >= 0 && attack.EffectiveCardKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.TargetSeat is { } target && target != user && ShiZrYinXianFarthest(user, target) ? 1 : 0;
    internal bool IsShiZrFuanStrikeUse(long useFrameId) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanStrikeIssuedEvent>().Any(entry => entry.UseId == useFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 68200)]
    internal bool ShiZrFuanUseDistance(ProgramCardDistanceQuery query) => query.CardUseFrameId is { } id && IsShiZrFuanStrikeUse(id);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 68200)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 68200)]
    internal bool ShiZrFuanSlashDistance(ProgramSlashDistanceQuery query) => query.ExistingUseFrameId is { } id && IsShiZrFuanStrikeUse(id);
    // Observes completed engine facts only: discards during an armed record arm
    // the foreign turn-ending strike.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68200)]
    internal void ObserveShiZrFuanDiscards(IGameEvent payload)
    {
        if (!HasShiZrCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || payload is not CardMovedEvent { CardId: var cardId, To: { Zone: CardZoneKind.DiscardPile } })
            return;
        var suit = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).Suit;
        for (var seat = 0; seat < _coreMatchParticipantsPlayersAccess._players.Count; seat++)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || ShiZrFuanRecord(seat)is not { } record || record.Suit != suit)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanSuitDiscardedEvent(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, seat, cardId, suit));
        }
    }

    internal bool ShiZrFuanSealActive(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanSealedEvent>().Any(stamped => stamped.TargetSeat == target && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanSealExpiredEvent>().Any(expired => expired.FrameId == stamped.FrameId));
    internal bool ShiZrFuanGrantQualified(CharacterState owner, SkillGrant grant) => !HasShiZrCapability || grant.IsModeRule || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) || !ShiZrFuanSealActive(owner.Seat);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68201)]
    internal void ExpireShiZrFuanSeals(IGameEvent payload)
    {
        if (!HasShiZrCapability || payload is not TurnEndedEvent turn)
            return;
        foreach (var sealedEntry in _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanSealedEvent>().Where(entry => entry.TargetSeat == turn.ActorSeat && turn.TurnNumber > entry.GrantedTurn && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanSealExpiredEvent>().Any(expired => expired.FrameId == entry.FrameId)).ToArray())
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanSealExpiredEvent(sealedEntry.FrameId, sealedEntry.TargetSeat, turn.TurnNumber));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        }
    }
}
