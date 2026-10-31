#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAdvancedLifecycleRuntime
{
    internal int GetWeaponAttackRange(CharacterState owner, Card weapon) => weapon.Kind == CardKind.RedBloodBlade && !_coreNationalSkillsOperations.EnabledContentSkillIds(owner).Contains("ol:shenyu", StringComparer.Ordinal) ? 0 : weapon.PrintedAttackRange ?? _coreEquipmentContentOperations.EquipmentRules.Get(weapon.Kind).WeaponAttackRange ?? 1;
    internal SkillProgramStepOutcome ExecuteAdvancedLifecycle(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        switch (effect.Op)
        {
            case SkillProgramEffectOp.BalanceHandAttackTricks:
                var slashes = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(AdvancedEffectiveHandKind(owner, card))).ToArray();
                var tricks = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => AdvancedIsOrdinaryTrick(AdvancedEffectiveHandKind(owner, card))).ToArray();
                if (slashes.Length == tricks.Length)
                {
                    _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true);
                    return SkillProgramStepOutcome.Continue;
                }

                var balancing = (slashes.Length > tricks.Length ? slashes : tricks).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, CardLocation.Hand(owner.Seat), OwnedCardMoveIntent.Discard)).ToArray();
                if (balancing.Length < Math.Abs(slashes.Length - tricks.Length))
                    return SkillProgramStepOutcome.Continue;
                return BeginAdvancedSelection(frame, effect, balancing.Select(card => card.Id.ToString()).ToArray(), Math.Abs(slashes.Length - tricks.Length));
            case SkillProgramEffectOp.ReplaceJudgmentPhase:
                _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= GameEngine.DelayedTurnEffects.SkipJudgmentPhase;
                var polarity = _skillRuntimeStateOperations._skillRuntimeState.GetConversionState(owner.Seat, frame.SkillId);
                _skillRuntimeStateOperations._skillRuntimeState.ToggleConversionState(owner.Seat, frame.SkillId);
                return _coreProgramLifecycleOperations.ScheduleProgramPhase(frame, polarity == SkillPolarity.Yang ? TurnPhase.Draw : TurnPhase.Play, SkillProgramPhaseContinuation.BeforeNormalPreparation);
            case SkillProgramEffectOp.AlterEquipmentSlots:
                foreach (var slot in effect.EquipmentSlots)
                    SetEquipmentSlotCapacity(target, slot, effect.Amount);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.AbolishRandomEquipmentSlot:
                var available = Enum.GetValues<EquipmentSlot>().Where(slot => target.EquipmentSlotCapacity(slot) > 0).ToArray();
                if (available.Length > 0)
                    SetEquipmentSlotCapacity(target, available[_randomOperations._random.Next(available.Length)], 0);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.SampleFactionSkills:
                return BeginAdvancedSelection(frame, effect, _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).Select(card => card.Id.ToString()).ToArray());
            case SkillProgramEffectOp.ExpireSampledSkills:
                var source = $"acquired:{(effect.StateId ?? frame.SkillId)}:sample:";
                var grants = owner.SkillGrants.Grants.Where(grant => grant.SourceId.StartsWith(source, StringComparison.Ordinal)).ToArray();
                foreach (var grant in grants)
                    owner.SkillGrants.RemoveGrant(grant.GrantId);
                _corePhysicalDrawFacadesOperations.DrawCards(owner, grants.Select(grant => grant.SkillId).Distinct(StringComparer.Ordinal).Count(), true);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.ReplaceSkillsOnAwakening:
                if (!_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(owner.Seat, frame.SkillId, frame.TriggerId) && _coreLordSkillProjectionsOperations.AdvancedOwnedSkillIds(owner).Count <= owner.MaxHp || !_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(owner.Seat, frame.SkillId, "awakening", SkillUsageScope.Game, 1))
                    return SkillProgramStepOutcome.Continue;
                _coreProgramLifecycleOperations.ChangeProgramMaximumHp(frame, -1);
                return BeginAdvancedSelection(frame, effect, _coreLordSkillProjectionsOperations.AdvancedOwnedSkillIds(owner).Where(id => id != frame.SkillId).ToArray());
            case SkillProgramEffectOp.AccumulateCardRank:
                var context = frame.WindowContext?.CardUse;
                var action = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()?.Action ?? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(item => item.Action?.ActionId == context?.CardActionId)?.Action;
                var rank = action?.EffectiveRank ?? (action?.PhysicalCards.Count == 1 ? GetAdvancedCard(action.PhysicalCards[0].CardId).Rank : 0);
                if (effect.Marker is not { } marker || rank <= 0)
                    return SkillProgramStepOutcome.Continue;
                var before = owner.Markers.GetValueOrDefault(marker);
                _coreStrategicProgramsOperations.SetStrategicMarker(owner.Seat, marker, owner.Seat, checked(before + rank), frame);
                var tens = owner.Markers[marker] / 10 % 10;
                if (before / 10 % 10 != tens)
                {
                    var matches = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => card.Rank == tens).ToArray();
                    if (matches.Length > 0)
                        ObtainAdvancedCards(frame, [matches[_randomOperations._random.Next(matches.Length)]]);
                }

                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.ObtainDeckRankSum:
                if (effect.Marker is not { } sumMarker || owner.Markers.GetValueOrDefault(sumMarker) <= _cardZonesOperations._cardZones.Count(CardLocation.DrawPile))
                    return SkillProgramStepOutcome.Continue;
                var deck = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
                if (!RankSubsetSearch.CanComplete(deck.Select(card => card.Rank), effect.MaximumRankSum))
                    return SkillProgramStepOutcome.Continue;
                _coreStrategicProgramsOperations.SetStrategicMarker(owner.Seat, sumMarker, owner.Seat, 0, frame);
                return BeginAdvancedSelection(frame, effect, deck.Select(card => card.Id.ToString()).ToArray(), effect.MaximumRankSum);
            case SkillProgramEffectOp.DamageAfterDeckShuffle:
                if (!_corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent).OfType<CardMovedEvent>().Any(item => item.Reason == CardMoveReasons.Reshuffle))
                    return SkillProgramStepOutcome.Continue;
                return BeginAdvancedSelection(frame, effect, _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).Select(player => player.Seat.ToString()).ToArray());
            case SkillProgramEffectOp.EquipSampledGenerals:
                var selectedGeneralIds = _coreMatchParticipantsPlayersAccess._players.SelectMany(player => new[] { player.General.Id, player.SecondaryGeneral?.Id }).OfType<string>().ToHashSet(StringComparer.Ordinal);
                var generals = _coreMatchDefinitionGeneralPoolAccess._generalPool.Select(general => general.Id).Where(id => !selectedGeneralIds.Contains(id) && !_coreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess._usedGeneralWeaponSources.Contains(id)).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToList();
                _randomOperations._random.Shuffle(generals);
                return BeginAdvancedSelection(frame, effect, generals.Take(Math.Max(1, effect.Amount)).ToArray());
            case SkillProgramEffectOp.DamageFarthestCharacter:
                if (frame.SelectedTargetSeats.Count == 0)
                    return BeginAdvancedSelection(frame, effect, _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat && IsFarthestInRange(owner, player) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, frame.SkillId, $"target:{(player.Seat)}", SkillUsageScope.Turn) == 0).Select(player => player.Seat.ToString()).ToArray());
                if (frame.SelectedTargetSeats.Count != 1 || !IsFarthestInRange(owner, target) || !_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(owner.Seat, frame.SkillId, $"target:{(targetSeat)}", SkillUsageScope.Turn, 1))
                    throw new InvalidOperationException("The damage recipient is not an unused farthest character in attack range.");
                return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, targetSeat, 1);
            case SkillProgramEffectOp.PlaceNamedWeapon:
                _coreAdvancedWeaponsOperations.PlaceAdvancedWeapon(frame, targetSeat, effect.OutputKind!.Value);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.ReclaimNamedWeapon:
                return _coreAdvancedWeaponsOperations.BeginWeaponReclaim(frame, effect);
            case SkillProgramEffectOp.InheritWeapon:
                var weapons = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(_corePhysicalOwnedCardQueriesOperations.GetEquipment).Concat(_corePhysicalOwnedCardQueriesOperations.GetHand(owner)).Where(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon).DistinctBy(card => card.Id).Select(card => card.Id.ToString()).ToArray();
                return BeginAdvancedSelection(frame, effect, weapons);
            default:
                throw new InvalidOperationException("Unsupported advanced lifecycle operation.");
        }
    }

    internal Card GetAdvancedCard(int id) => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id);
    internal CardKind AdvancedEffectiveHandKind(CharacterState owner, Card card) => _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card).FirstOrDefault()?.Identity.OutputKind ?? card.Kind;
    internal static bool AdvancedIsOrdinaryTrick(CardKind kind) => RuntimeSkills.RuntimeRules_Core_AdvancedLifecycle.AdvancedIsOrdinaryTrick(kind);
    internal void SetEquipmentSlotCapacity(CharacterState player, EquipmentSlot slot, int capacity)
    {
        if (capacity < 0 || capacity > 5)
            throw new InvalidOperationException("Equipment slot capacity must be between zero and five.");
        var previousCapacity = player.EquipmentSlotCapacity(slot);
        player.EquipmentSlotCapacities[slot] = capacity;
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == slot).Skip(capacity).ToArray())
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Equipment(player.Seat), CardLocation.DiscardPile, new("equipment.slot-abolished"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotCapacityChangedEvent(player.Seat, slot, capacity));
        if (previousCapacity > 0 && capacity == 0 && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame program)
            _characterLiuHongOlOperations.ResolveTuxingAfterSlotAbolish(program, player.Seat, 1);
    }

    internal bool RestoreEquipmentSlot(CharacterState player, EquipmentSlot slot)
    {
        if (player.EquipmentSlotCapacity(slot) > 0)
            return false;
        if (player.EquipmentAreaAbolished)
        {
            // Preserve the other disabled slots when converting the legacy whole-area
            // flag into the native per-slot representation.
            foreach (var equipmentSlot in Enum.GetValues<EquipmentSlot>())
                player.EquipmentSlotCapacities[equipmentSlot] = 0;
            player.EquipmentAreaAbolished = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerAreasAbolishedEvent(player.Seat, false, player.JudgmentAreaAbolished));
        }

        SetEquipmentSlotCapacity(player, slot, 1);
        return true;
    }

    internal bool IsFarthestInRange(CharacterState owner, CharacterState target)
    {
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(owner.Seat);
        var legal = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner.Seat, player.Seat)).ToArray();
        return legal.Contains(target) && _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, target.Seat) == legal.Max(player => _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, player.Seat));
    }

    internal void ObtainAdvancedCards(ProgramSkillFrame frame, IReadOnlyList<Card> cards)
    {
        if (cards.Count == 0)
            return;
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.DrawPile, CardLocation.Hand(frame.OwnerSeat), new("skill.deck-search.obtain"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeckRankCardsObtainedEvent(frame.OwnerSeat, frame.SkillId, cards.Select(card => card.Id).ToArray()));
    }

    internal SkillProgramStepOutcome BeginAdvancedSelection(ProgramSkillFrame frame, SkillProgramEffect effect, IReadOnlyList<string> candidates, int rankSum = 0)
    {
        if (candidates.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { AdvancedSelection = new(effect.Op, candidates, [], rankSum) });
        _coreAdvancedLifecycleChoicesOperations.PromptAdvancedSelection(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }
}
