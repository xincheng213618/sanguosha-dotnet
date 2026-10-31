#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNextCardTargetAdjustmentsRuntime
{
    internal bool HasRemainingAdjustedSimpleTargets(long frameId) => (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId)?.AdjustedSimpleContinuation is not null) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == frameId && frame.TargetIndex + 1 < frame.TargetSeats.Count);
    internal bool TryContinueAdjustedSimpleCardUse(long frameId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId)?.AdjustedSimpleContinuation is not { } continuation)
            return false;
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == frameId);
        if (frame.TargetIndex + 1 >= frame.TargetSeats.Count || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(frameId, use => use with { AdjustedSimpleContinuation = null });
            return false;
        }

        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(frameId, frame.TargetIndex + 1);
        _coreCardUseLifecycleOperations.ContinueSimpleCardUse(frameId, continuation);
        return true;
    }

    internal bool HasRemainingAdjustedBorrowedSwordTargets(long frameId) => !_characterJiangJiMouOperations.IsTargetScopedMouJjEffect(frameId) && !_characterDaQiaoLeOperations.IsTargetScopedZixiEffectRound(frameId) && _coreMatchOutcomeWinnerAccess._winner == Winner.None && (_coreCardUseLifecycleOperations.LifecycleCardUse(frameId)?.TargetsAdjusted == true) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == frameId && frame.CardKind == CardKind.BorrowedSword && frame.TargetSeats.Count >= 4 && frame.TargetSeats.Count % 2 == 0 && frame.TargetIndex + 2 < frame.TargetSeats.Count);
    internal bool TryContinueAdjustedBorrowedSwordUse(long frameId)
    {
        if (!HasRemainingAdjustedBorrowedSwordTargets(frameId))
            return false;
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == frameId);
        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(frameId, frame.TargetIndex + 2);
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(frameId, frame.CardId, requireProcessing: true);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(frameId, card, frame.SourceSeat, frame.TargetSeats.Skip(frame.TargetIndex + 2).Take(2).ToArray(), LegalActionKind.BorrowedSword);
        return true;
    }

    void INextCardTargetAdjustmentProgramHost.GrantNextCardTargetAdjustment(ProgramSkillFrame frame) => GrantNextCardTargetAdjustment(frame);
    internal void GrantNextCardTargetAdjustment(ProgramSkillFrame frame)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        _coreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners[frame.OwnerSeat] = new(frame.SkillId, "next-card-target-adjustment", frame.OwnerSeat, frame.SkillInstanceId);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 210)]
    internal void ObserveNextCardTargetAdjustmentEvent(IGameEvent payload)
    {
        if (payload is TurnStartedEvent)
        {
            _coreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners.Clear();
            _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants.Clear();
        }
        else if (payload is CardUseDeclaredEvent used && (!_coreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners.TryGetValue(used.SourceSeat, out var source) || source.SkillId != "classic:qiaoshui" || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(used.CardKind) == SkillProgramCardCategory.Basic || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(used.CardKind)))
            _coreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners.Remove(used.SourceSeat);
    }

    internal bool HasLegacyNextCardTargetAdjustment(CharacterState owner) => _coreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners.TryGetValue(owner.Seat, out var source) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId);
    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 10)]
    internal void AddNextCardTargetAdjustmentActions(List<LegalAction> actions, CharacterState actor)
    {
        if (!HasLegacyNextCardTargetAdjustment(actor))
            return;
        var source = _coreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess._nextCardTargetAdjustmentOwners[actor.Seat];
        var ordinary = actions.ToArray();
        foreach (var action in ordinary.Where(item => item.CardId is not null && item.Kind != LegalActionKind.Recast && (item.CardId != 0 || item.ProgramActivationId is null)))
        {
            var normal = action.Kind is LegalActionKind.DrawTwo or LegalActionKind.Peach or LegalActionKind.Alcohol ? new[]
            {
                actor.Seat
            }

            : _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, action.TargetSeats).ToArray();
            if (normal.Length == 0)
                continue;
            var kind = action.PlayedCardKind ?? _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)!.Kind;
            if (kind == CardKind.BorrowedSword)
            {
                foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && player.Seat != action.TargetSeats[0] && _coreNativeBorrowedSwordOperations.GetWeapon(player)is not null && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, kind, action.CardId == 0 ? Suit.None : _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)!.Suit, action.CardId == 0 ? null : CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)!, action.ConversionSource))))))
                    foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(player => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(owner, player)))
                        Add([..action.TargetSeats, owner.Seat, victim.Seat]);
                continue;
            }

            // Placement cards retain their legal target contract. Classic
            // Qiaoshui waits for the next basic or ordinary trick to consume it.
            if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) || kind is CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning or CardKind.BorrowedSword)
                continue;
            var possible = action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains ? normal : _coreMatchParticipantsPlayersAccess._players.Where(player => CanBeExtraNextCardTarget(actor, player, action, kind)).Select(player => player.Seat).ToArray();
            foreach (var seat in possible.Except(normal))
                Add(normal.Append(seat).ToArray());
            if (normal.Length > 1 && !CharacterDuKuiAnniversaryRuntime.IsDuKuiFanyinTargetGrant(source))
                foreach (var seat in normal)
                    Add(normal.Where(item => item != seat).ToArray());
            void Add(IReadOnlyList<int> targets)
            {
                if (targets.Count == 0 || actions.Any(item => item.CardId == action.CardId && item.Kind == action.Kind && item.PlayedCardKind == action.PlayedCardKind && item.ConversionSource == action.ConversionSource && item.TargetCardId == action.TargetCardId && item.TargetSeats.SequenceEqual(targets)))
                    return;
                actions.Add(action with { TargetSeat = targets[0], TargetSeats = targets, ProgramSkillId = source.SkillId, ProgramActivationId = source.BindingId, Description = action.Description + (CharacterDuKuiAnniversaryRuntime.IsDuKuiFanyinTargetGrant(source) ? "（下一张牌额外选择一个目标）" : "（下一张牌增减一个目标）") });
            }
        }
    }

    internal bool CanBeExtraNextCardTarget(CharacterState actor, CharacterState target, LegalAction action, CardKind kind, Suit? effectiveUseSuit = null)
    {
        if (action.CardId == 0)
            return _coreNextActualUseTargetAdjustmentsOperations.CanBeExtraTargetAdjustmentTarget(actor, target, action, kind, Suit.None);
        var physical = _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)!;
        if (!target.IsAlive || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, effectiveUseSuit ?? physical.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(effectiveUseSuit ?? _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, physical, action.ConversionSource)))) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, effectiveUseSuit ?? _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, physical, action.ConversionSource))))
            return false;
        return action.Kind switch
        {
            LegalActionKind.Slash => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, physical, action.ConversionSource, kind, ignoreDistance: true),
            LegalActionKind.Peach => _coreTreatedWoundedOperations.IsRuleWounded(target),
            LegalActionKind.Alcohol or LegalActionKind.DrawTwo or LegalActionKind.IronChain => true,
            LegalActionKind.WumiaoFlood => true,
            LegalActionKind.Duel or LegalActionKind.QizhengXiangsheng => target.Seat != actor.Seat,
            LegalActionKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            LegalActionKind.UnexpectedAssault => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            LegalActionKind.Dismantlement or LegalActionKind.Snatch => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Count > 0,
            _ => false
        };
    }
}
