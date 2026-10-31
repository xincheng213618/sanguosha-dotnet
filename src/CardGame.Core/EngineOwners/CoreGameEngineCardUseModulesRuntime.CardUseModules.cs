#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardUseModulesRuntime
{
    internal bool IsTurnHandCardRestricted(CharacterState player, Card card)
    {
        if (_coreProgramCardLegalityOperations.IsProgramHandPhysicalRestricted(player.Seat, card.Id) || _coreTurnHandCategoryRestrictionsOperations.IsTurnHandCategoryRestricted(player, card) || _coreResponseEntityExchangeOperations.IsResponseEntityRestricted(player.Seat, card.Id))
            return true;
        if (_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(player, card))
            return true;
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(player.Seat))
            return false;
        if (!_characterSimaYiMouOperations.IsMouSmPlayedActor(player) && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { ProhibitsTargetHandResponses: true } attack && attack.TargetSeat == player.Seat && attack.CardUserSeat != player.Seat)
            return true;
        if (_turnCardUseEffectsOperations._turnCardUseEffects.HandColorRestrictions.Count == 0)
            return false;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(player, card);
        return _turnCardUseEffectsOperations._turnCardUseEffects.IsHandColorRestricted(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, player.Seat, CoreNativeCardClassificationsRuntime.IsRedSuit(suit), isColorless: suit == Suit.None);
    }

    internal bool IsCardUseForbidden(int actorSeat, CardKind effectiveKind, CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = null) => !_modeRulesOperations._modeRules.Cards.IsCardAllowed(effectiveKind) || _coreProgramCardLegalityOperations.CardLegality.IsUseForbidden(ProgramCardLegalityStage.BeforeIntrinsicUseBan, new(actorSeat, effectiveKind, actionType, existingUseFrameId)) || effectiveKind == CardKind.Poison || effectiveKind == CardKind.Peach && actionType == CardActionType.Use && !_characterSimaYiHappyGodOperations.AllowsHappySimaPeach(actorSeat) || _coreProgramCardLegalityOperations.CardLegality.IsUseForbidden(ProgramCardLegalityStage.BeforeTurnUseBan, new(actorSeat, effectiveKind, actionType, existingUseFrameId)) || !ignoreIssuedPlayBan && (actionType == CardActionType.Use || actionType == CardActionType.Response && (effectiveKind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actorSeat], effectiveKind))) && _coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(actorSeat) || _turnCardUseEffectsOperations._turnCardUseEffects.IsCardUseForbidden(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, actorSeat, effectiveKind, actionType) || _coreDyingWindowSkillPoliciesOperations.IsExclusiveTurnPeachUseForbidden(actorSeat, effectiveKind, actionType) || _coreNextActualUseTargetAdjustmentsOperations.IsNextActualUseCounterspellForbidden(actorSeat, effectiveKind, actionType) || _coreAlcoholIdentityPoliciesOperations.IsForeignTurnAlcoholUseForbidden(actorSeat, effectiveKind, actionType) || _corePhaseStartControlPoliciesOperations.IsOtherPlayRestrictionUseForbidden(actorSeat, effectiveKind, actionType) || _characterYuanTanYuanShangOlOperations.IsNeifaUseForbidden(actorSeat, effectiveKind, actionType) || _characterZhangLingOlOperations.IsCategoryTalismanUseForbidden(actorSeat, effectiveKind, actionType);
    internal IReadOnlyList<LegalAction> FilterTurnCardUseRestrictions(CharacterState actor, IEnumerable<LegalAction> actions, Card? nativeOriginalCard = null) => actions.Select(action => _coreProgramCardLegalityOperations.CardLegality.ProjectAction(new(actor, action, null, nativeOriginalCard))).Where(action => action is not null).Select(action => action!).Where(action =>
    {
        if (HasTurnCardTargetRestriction(actor.Seat, SkillProgramCardTargetRestriction.SelfOnly))
        {
            if (action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage)
                return false;
            if (action.Kind == LegalActionKind.UseEquipmentEffect && action.MaxTargetCount > 0)
                return false;
            if (action.TargetSeats.Any(targetSeat => targetSeat != actor.Seat))
                return false;
        }

        if (_coreProgramCardLegalityOperations.CardLegality.IsActionForbidden(ProgramCardLegalityStage.BeforeLegalActionKind, new(actor, action, null, nativeOriginalCard)))
            return false;
        if (!TryGetLegalActionEffectiveCardKind(actor, action, out var effectiveKind, nativeOriginalCard))
            return true;
        if (_coreProgramCardLegalityOperations.CardLegality.IsActionForbidden(ProgramCardLegalityStage.AfterLegalActionKind, new(actor, action, effectiveKind, nativeOriginalCard)))
            return false;
        if (_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, effectiveKind, action.Kind == LegalActionKind.BorrowedSword ? action.TargetSeats : _coreHandComparisonOperations.GetSelfProhibitionPolicyTargets(actor, action)))
            return false;
        // Zero identifies a genuine material-free action, so only real entities have a physical color.
        return !(action.CardId is { } id && id != 0 && _coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(actor, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id))) && !IsCardUseForbidden(actor.Seat, effectiveKind, CardActionType.Use) && _coreHandComparisonOperations.GetSelfProhibitionPolicyTargets(actor, action).All(targetSeat => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, targetSeat, effectiveKind));
    }).ToArray();
    internal bool HasTurnCardTargetRestriction(int actorSeat, SkillProgramCardTargetRestriction restriction, int? targetSeat = null) => restriction == SkillProgramCardTargetRestriction.SelfOnly && _characterSunQuanHappyGodOperations.HasHappyLearningPlaySelfOnly(actorSeat) || _turnCardUseEffectsOperations._turnCardUseEffects.HasTargetRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, actorSeat, restriction, targetSeat);
    internal IReadOnlyList<int> ApplyTurnCardGroupTargetRestrictions(CharacterState source, IReadOnlyList<int> targets) => HasTurnCardTargetRestriction(source.Seat, SkillProgramCardTargetRestriction.SelfOnly) ? targets.Where(seat => seat == source.Seat).ToArray() : targets;
    internal bool HasUnlimitedTurnRuleModifier(int actorSeat, SkillRuleQuery query, CardKind? effectiveCardKind = null) => _turnCardUseEffectsOperations._turnCardUseEffects.GetRuleModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, actorSeat, query, effectiveCardKind).Any(item => item.Operation == SkillRuleOperation.Unlimited);
    internal int GetAdditiveTurnRuleModifier(int actorSeat, SkillRuleQuery query) => _turnCardUseEffectsOperations._turnCardUseEffects.GetRuleModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, actorSeat, query).Where(item => item.Operation == SkillRuleOperation.Add).Sum(item => item.Amount);
    internal bool TryGetLegalActionEffectiveCardKind(CharacterState actor, LegalAction action, out CardKind effectiveKind, Card? nativeOriginalCard = null)
    {
        effectiveKind = default;
        if (action.Kind is LegalActionKind.EndPlay or LegalActionKind.Recast || action.CardId is not { } cardId || action.Kind is not (LegalActionKind.Slash or LegalActionKind.Peach or LegalActionKind.Duel or LegalActionKind.DrawTwo or LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains or LegalActionKind.WumiaoFlood or LegalActionKind.QizhengXiangsheng or LegalActionKind.MantianGuohai or LegalActionKind.Dismantlement or LegalActionKind.Snatch or LegalActionKind.FireAttack or LegalActionKind.UnexpectedAssault or LegalActionKind.Alcohol or LegalActionKind.Equip or LegalActionKind.IronChain or LegalActionKind.Indulgence or LegalActionKind.SupplyShortage or LegalActionKind.Lightning or LegalActionKind.BorrowedSword or LegalActionKind.StarShift or LegalActionKind.BeamSwap or LegalActionKind.HandRedistribution or LegalActionKind.WenheChaos or LegalActionKind.Zong or LegalActionKind.RealgarWine or LegalActionKind.SameBoat or LegalActionKind.StriveUpstream or LegalActionKind.Intoxication or LegalActionKind.StoreGrain or LegalActionKind.Reinforcements or LegalActionKind.UnitedEffort or LegalActionKind.AgainstCurrent))
        {
            return false;
        }

        effectiveKind = action.PlayedCardKind ?? (nativeOriginalCard is { } original && original.Id == cardId ? original.Kind : _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Single(card => card.Id == cardId).Kind);
        return true;
    }

    internal void ExpireTurnCardUseEffects(int turnNumber, int turnSeat)
    {
        _coreProgramBooleanStateExpirationOperations.ExpireProgramBooleanStateLeases2(turnNumber, turnSeat);
        _coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess._currentTurnHeartSlashBonuses.RemoveAll(b => b.TurnNumber == turnNumber && b.TurnOwnerSeat == turnSeat);
        _coreCurrentTurnSkillSuppressionsOperations.ExpireCurrentTurnNonLockedSkillSuppressions(turnNumber, turnSeat);
        _coreAlcoholSlashSuppressionsOperations.ExpireCurrentTurnOwnSkillSuppressions(turnNumber, turnSeat);
        _coreNextSlashProgramsOperations.ExpireNextSlashDamage(turnNumber, turnSeat);
        _coreDirectedTurnCardPoliciesOperations.ExpireDirectedTurnCardPolicies(turnNumber, turnSeat);
        _coreOriginalTargetAdditionsOperations.ExpireOriginalTargetAdditionGrants(turnNumber, turnSeat);
        _coreFixedDistanceOneDebtOperations.ExpireFixedDistanceOneGrants(turnNumber, turnSeat);
        _coreBoundDiscardSlashBenefitsOperations.ResolveFirstRoundGameUsageRefunds(turnNumber, turnSeat);
        var expired = _turnCardUseEffectsOperations._turnCardUseEffects.ExpireTurn(turnNumber, turnSeat);
        if (expired.Count == 0)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnCardUseEffectsExpiredEvent(turnNumber, turnSeat, expired));
    }

    internal void ExpireOwnerTurnStartDamageModifiers(int ownerSeat)
    {
        _coreParticipantAndReserveProgramsOperations.ExpireAttributedNatureMarkers(ownerSeat);
        _coreProgramBooleanStateExpirationOperations.ExpireProgramBooleanStateLeasesAtOwnerTurnStart(ownerSeat);
        var expired = _turnCardUseEffectsOperations._turnCardUseEffects.ExpireDamageModifiersAtOwnerTurnStart(ownerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber).Concat(_turnCardUseEffectsOperations._turnCardUseEffects.ExpireRecoveryAndRuleModifiersAtOwnerTurnStart(ownerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)).Order().ToArray();
        if (expired.Length > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnCardUseEffectsExpiredEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, ownerSeat, expired));
    }

    internal void ExpireDeadOwnerDamageModifiers(int ownerSeat)
    {
        _coreProgramBooleanStateExpirationOperations.ExpireProgramBooleanStateLeasesAtOwnerTurnStart(ownerSeat, ownerDied: true);
        var expired = _turnCardUseEffectsOperations._turnCardUseEffects.ExpireDamageModifiersOnDeath(ownerSeat).Concat(_turnCardUseEffectsOperations._turnCardUseEffects.ExpireRecoveryAndRuleModifiersOnDeath(ownerSeat)).Order().ToArray();
        if (expired.Length > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnCardUseEffectsExpiredEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, expired));
    }

    internal void GrantProgramTurnCardDamageModifier(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, int amount, SkillProgramDamageModifierExpiration expiration, SkillProgramDamageModifierSourceScope sourceScope)
    {
        ValidateProgramTurnEffectGrant(frame);
        if (amount <= 0 || cardKinds.Count == 0 || !Enum.IsDefined(expiration) || !Enum.IsDefined(sourceScope))
        {
            throw new InvalidOperationException("A turn card-damage modifier requires positive damage and card kinds.");
        }

        var source = CreateProgramTurnEffectSource(frame);
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantDamageModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, source, cardKinds, amount, expiration, sourceScope);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardDamageModifierGrantedEvent(granted));
    }

    internal void GrantProgramTurnCardActionProhibition(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, IReadOnlyList<CardActionType> actionTypes)
    {
        ValidateProgramTurnEffectGrant(frame);
        if (cardKinds.Count == 0 || actionTypes.Count == 0)
            throw new InvalidOperationException("A turn card-action prohibition requires kinds and actions.");
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantActionProhibition(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CreateProgramTurnEffectSource(frame), cardKinds, actionTypes);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionProhibitionGrantedEvent(granted));
    }

    internal void GrantProgramTurnHandColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat, bool useFrozenSuit = false)
    {
        ValidateProgramTurnEffectGrant(frame);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            throw new InvalidOperationException("A hand-color restriction requires one living other character.");
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, sourceBind);
        if (binding.CardIds.Count != 1 || binding.SourceLocations.Count != 1 || (!useFrozenSuit && _cardZonesOperations._cardZones.GetLocation(binding.CardIds[0]) != binding.SourceLocations[0]))
            throw new InvalidOperationException("A hand-color restriction requires one stable bound card.");
        var effectiveSuit = useFrozenSuit ? binding.FrozenRevealedSuit ?? throw new InvalidOperationException("A frozen hand-color restriction requires its captured suit.") : _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _cardZonesOperations._cardZones.CardsAt(binding.SourceLocations[0]).Single(item => item.Id == binding.CardIds[0]));
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandColorRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CreateProgramTurnEffectSource(frame), targetSeat, CoreNativeCardClassificationsRuntime.IsRedSuit(effectiveSuit));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HandCardColorRestrictionGrantedEvent(granted));
    }

    internal void GrantProgramTurnHandCardProhibition(ProgramSkillFrame frame, int targetSeat)
    {
        ValidateProgramTurnEffectGrant(frame);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            throw new InvalidOperationException("A hand-card prohibition requires one living other character.");
        var effectIndex = frame.InstructionIndex - 1;
        foreach (var isRed in new[]
        {
            false,
            true
        }

        )
        {
            var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandColorRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, -2 * effectIndex - (isRed ? 2 : 1), CreateProgramTurnEffectSource(frame), targetSeat, isRed);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HandCardColorRestrictionGrantedEvent(granted));
        }
    }

    internal void GrantProgramTurnRuleModifier(ProgramSkillFrame frame, SkillRuleQuery query, SkillRuleOperation operation, int amount, IReadOnlyList<CardKind> cardKinds, PlayerMarkerKind? amountFromMarker = null, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd)
    {
        ValidateProgramTurnEffectGrant(frame);
        if (amountFromMarker is { } marker)
            amount = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Markers.GetValueOrDefault(marker);
        var valid = query == SkillRuleQuery.SlashLimit && operation == SkillRuleOperation.Add && (amount > 0 || amount is >= -20 and <= -1) || query == SkillRuleQuery.HandLimit && operation == SkillRuleOperation.Add && amount is >= -20 and <= 20 && amount != 0 || query == SkillRuleQuery.OutgoingDistance && operation == SkillRuleOperation.Add && amount is >= -20 and <= 20 && amount != 0 || (query is SkillRuleQuery.SlashLimit or SkillRuleQuery.CardUseDistanceLimit or SkillRuleQuery.SlashDistanceLimit or SkillRuleQuery.AttackRange) && operation == SkillRuleOperation.Unlimited && amount == 0 || query == SkillRuleQuery.CardTargetCount && operation == SkillRuleOperation.Add && amount > 0 && cardKinds.Count > 0;
        if (query != SkillRuleQuery.CardTargetCount && query != SkillRuleQuery.CardUseDistanceLimit && cardKinds.Count > 0)
            throw new InvalidOperationException("Only card target-count and card-use distance-limit modifiers accept card kinds.");
        if (query == SkillRuleQuery.CardUseDistanceLimit && cardKinds.Count > 0 && cardKinds.Distinct().Count() != cardKinds.Count)
            throw new InvalidOperationException("A card-use distance-limit modifier requires distinct card kinds.");
        if (!valid || !Enum.IsDefined(expiration))
            throw new InvalidOperationException("The turn rule modifier is unsupported.");
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CreateProgramTurnEffectSource(frame), query, operation, amount, cardKinds, expiration: expiration);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(granted));
    }

    internal void GrantProgramTurnCardTargetRestriction(ProgramSkillFrame frame, SkillProgramCardTargetRestriction restriction, int targetSeat)
    {
        ValidateProgramTurnEffectGrant(frame);
        var targetScoped = restriction != SkillProgramCardTargetRestriction.SelfOnly;
        // A self-only restriction binds the character named by the instruction, which lets a skill
        // pin another character to self-targeting instead of only its own owner.
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetScoped && targetSeat == frame.OwnerSeat)
            throw new InvalidOperationException("The turn card-target restriction has an invalid subject.");
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CreateProgramTurnEffectSource(frame), restriction, targetScoped ? targetSeat : null, targetScoped || targetSeat == frame.OwnerSeat ? null : targetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(granted));
    }

    internal void GrantProgramTurnCardConversion(ProgramSkillFrame frame, string sourceBind, SkillProgramCardColorRelation colorRelation, CardKind outputKind)
    {
        ValidateProgramTurnEffectGrant(frame);
        if (colorRelation != SkillProgramCardColorRelation.OppositeBoundCard || outputKind != CardKind.Duel)
            throw new InvalidOperationException("The turn card conversion is unsupported.");
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, sourceBind);
        if (binding.CardIds.Count == 0)
            return;
        if (binding.CardIds.Count != 1 || binding.SourceLocations.Count != 1 || _cardZonesOperations._cardZones.GetLocation(binding.CardIds[0]) != binding.SourceLocations[0])
            throw new InvalidOperationException("A turn card conversion requires one stable bound card.");
        var card = _cardZonesOperations._cardZones.CardsAt(binding.SourceLocations[0]).Single(item => item.Id == binding.CardIds[0]);
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantConversion(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CreateProgramTurnEffectSource(frame), sourceBind, colorRelation, CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card)), outputKind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardConversionGrantedEvent(granted));
    }

    internal void ValidateProgramTurnEffectGrant(ProgramSkillFrame frame)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id || frame.OwnerSeat < 0 || frame.OwnerSeat >= _coreMatchParticipantsPlayersAccess._players.Count || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || (frame.TriggerId is null && string.IsNullOrWhiteSpace(frame.ActivationId)))
            throw new InvalidOperationException("A turn effect requires an active program frame in an ongoing turn.");
    }

    internal static CardUseEffectSource CreateProgramTurnEffectSource(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_Core_CardUseModules.CreateProgramTurnEffectSource(frame);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 130)]
    internal bool DistanceCardUseExemption130HasUnlimitedTurnRuleModifier(ProgramCardDistanceQuery q) => HasUnlimitedTurnRuleModifier(q.Actor.Seat, SkillRuleQuery.CardUseDistanceLimit, q.Kind);
}
