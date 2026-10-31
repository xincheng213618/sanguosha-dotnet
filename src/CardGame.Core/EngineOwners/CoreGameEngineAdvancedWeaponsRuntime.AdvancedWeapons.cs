#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAdvancedWeaponsRuntime
{
    internal bool MoveProcessingCardUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null)
    {
        if ((card.IsGeneralWeapon || _characterDiaoChanLeOperations.IsDiouCarrier(card.Id) || _coreWushuangShopCardsOperations.IsWushuangShopEquipment(card.Id) || _coreShanheStartingResourcesOperations.IsShanheStartingResource(card.Id) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheStartingResourceDestroyedEvent>().Any(e => e.CardId == card.Id)) && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.OutsideGame)
            return false;
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, destination, reason, beforeFact);
        return true;
    }

    internal void ResolveDynamicEquipmentSkillGrants(Card card, CardLocation from, CardLocation to)
    {
        var source = $"equipment:dynamic:{(card.Id)}";
        if (from is { Zone: CardZoneKind.Equipment, OwnerSeat: { } oldSeat })
            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[oldSeat].SkillGrants.Grants.Where(grant => grant.SourceId == source).ToArray())
                _coreMatchParticipantsPlayersAccess._players[oldSeat].SkillGrants.RemoveGrant(grant.GrantId);
        if (to is { Zone: CardZoneKind.Equipment, OwnerSeat: { } newSeat })
            foreach (var id in card.GrantedSkillIds.Concat(_coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._copiedWeaponSkills.GetValueOrDefault(card.Id) ?? []).Distinct(StringComparer.Ordinal))
            {
                var grant = $"{(source)}:{(id)}";
                _coreMatchParticipantsPlayersAccess._players[newSeat].SkillGrants.Grant(new(grant, id, grant, source));
            }
    }

    internal string[] GetGeneralWeaponSkillIds(ContentGeneralDefinition general) => general.SkillIds.Select(_contentRegistryOperations._contentRegistry.GetSkill).Where(skill => skill.Description.Contains("【杀】", StringComparison.Ordinal) && (skill.Tags & (SkillTag.Awakening | SkillTag.Limited | SkillTag.Conversion | SkillTag.Lord)) == SkillTag.None).Select(skill => skill.Id).ToArray();
    internal string DescribeGeneralWeaponChoice(string generalId)
    {
        var general = _contentRegistryOperations._contentRegistry.Generals[generalId];
        var names = GetGeneralWeaponSkillIds(general).Select(id => _contentRegistryOperations._contentRegistry.GetSkill(id).Name).ToArray();
        return $"{(general.Name)} · 攻击范围{(general.BaseHp)} · " + (names.Length == 0 ? "无可复制技能" : string.Join(" / ", names));
    }

    internal void EquipGeneralWeapon(ProgramSkillFrame frame, ContentGeneralDefinition general)
    {
        _coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._usedGeneralWeaponSources.Add(general.Id);
        var skills = GetGeneralWeaponSkillIds(general);
        var id = _coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(item => item.CardId).DefaultIfEmpty(0).Max() + 1;
        var card = new Card(id, CardKind.GeneralWeapon, Suit.None, 0)
        {
            PrintedName = general.Name,
            PrintedAttackRange = general.BaseHp,
            GrantedSkillIds = skills,
            IsGeneralWeapon = true
        };
        _cardZonesOperations._cardZones.AddGeneratedCard(card);
        _coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._generatedPhysicalCardIds.Add(card.Id);
        PlaceAdvancedEquipment(card, frame.OwnerSeat);
    }

    internal void PlaceAdvancedWeapon(ProgramSkillFrame frame, int targetSeat, CardKind kind)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            return;
        var found = _coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Where(item => item.CardKind == kind && !_coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._permanentlyRemovedWeapons.Contains(item.CardId) && !_characterDiaoChanLeOperations.IsDiouCarrier(item.CardId)).ToArray();
        var card = found.Length > 0 ? _coreAdvancedLifecycleOperations.GetAdvancedCard(found[0].CardId) : null;
        if (card is null)
        {
            if (_coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._permanentlyRemovedWeapons.Any(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == kind))
                return;
            card = new(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(item => item.CardId).DefaultIfEmpty(0).Max() + 1, kind, Suit.Heart, 13);
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._generatedPhysicalCardIds.Add(card.Id);
        }

        PlaceAdvancedEquipment(card, targetSeat);
    }

    internal void PlaceAdvancedEquipment(Card card, int targetSeat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
        if (target.EquipmentSlotCapacity(slot) == 0)
            return;
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (from == CardLocation.Equipment(targetSeat))
            return;
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Where(item => _coreFieldEquipmentOperations.FieldEquipmentSlot(item) == slot).ToArray();
        if (equipped.Length >= target.EquipmentSlotCapacity(slot))
        {
            var replaced = equipped[0];
            CopyFirstReplacedWeapon(card, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(targetSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(targetSeat), CardMoveReasons.EquipmentEnter);
    }

    internal void CopyFirstReplacedWeapon(Card card, Card replaced)
    {
        if (card.Kind != CardKind.RedBloodBlade || _coreFieldEquipmentOperations.FieldEquipmentSlot(replaced) != EquipmentSlot.Weapon || !_coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._weaponCopyConsumed.Add(card.Id))
            return;
        _coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._copiedWeaponAbilities[card.Id] = GetWeaponAbilityKinds(replaced).ToArray();
        var skills = replaced.GrantedSkillIds.Concat(replaced.Kind == CardKind.XingtianAxe ? new[] { "special:xingtian-axe-effect" } : []).ToArray();
        _coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._copiedWeaponSkills[card.Id] = skills;
    }

    internal IEnumerable<CardKind> GetWeaponAbilityKinds(Card card) => new[]
    {
        card.Kind
    }.Concat(_coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._copiedWeaponAbilities.GetValueOrDefault(card.Id) ?? []);
    internal bool HasWeaponAbility(CharacterState player, CardKind kind) => _characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => !card.IsGeneralWeapon && GetWeaponAbilityKinds(card).Contains(kind)) || (_coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._inheritedWeaponAbilities.GetValueOrDefault(player.Seat) ?? []).Contains(kind) || kind == CardKind.StoneAxe && _characterLiYiOlOperations.HasLiYiVirtualStoneAxe(player) || _coreProgramPipelineHooksOperations.ProgramVirtualEquipment(player.Seat, EquipmentSlot.Weapon) == kind;
    internal IReadOnlyList<Card> GetEquipmentRuleCards(CharacterState player, EquipmentSlot? excludedEquipmentSlot = null)
    {
        IReadOnlyList<Card> physical = _characterHuCheErMouOperations.KongwuEffectiveEquipment(player);
        if (excludedEquipmentSlot is not null)
            physical = physical.Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) != excludedEquipmentSlot).ToArray();
        var kinds = physical.Where(card => !card.IsGeneralWeapon).SelectMany(GetWeaponAbilityKinds).Concat(_coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._inheritedWeaponAbilities.GetValueOrDefault(player.Seat) ?? []).Concat(_coreProgramPipelineHooksOperations.ProgramVirtualEquipment(player.Seat, EquipmentSlot.Weapon)is { } virtualWeapon ? new[] { virtualWeapon } : []).Distinct().ToArray();
        return physical.Where(card => !card.IsGeneralWeapon).Concat(kinds.Where(kind => !physical.Any(card => card.Kind == kind)).Select((kind, index) => new Card(-500 - index, kind, Suit.None, 0))).ToArray();
    }

    internal void CommitWeaponInheritance(ProgramSkillFrame frame, int cardId)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        if (_coreFieldEquipmentOperations.FieldEquipmentSlot(card) != EquipmentSlot.Weapon || location.Zone != CardZoneKind.Equipment && location != CardLocation.Hand(frame.OwnerSeat))
            throw new InvalidOperationException("Only an equipped weapon or owner's hand weapon can be inherited.");
        var range = location.OwnerSeat is { } seat ? _coreAdvancedLifecycleOperations.GetWeaponAttackRange(_coreMatchParticipantsPlayersAccess._players[seat], card) : _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).WeaponAttackRange ?? 1;
        _coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._inheritedWeaponAbilities[frame.OwnerSeat] = (_coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._inheritedWeaponAbilities.GetValueOrDefault(frame.OwnerSeat) ?? []).Concat(GetWeaponAbilityKinds(card)).Distinct().ToArray();
        var skills = card.GrantedSkillIds.Concat(_coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._copiedWeaponSkills.GetValueOrDefault(card.Id) ?? []).Concat(card.Kind == CardKind.XingtianAxe ? new[] { "special:xingtian-axe-effect" } : []).ToArray();
        _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, skills);
        _coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._permanentlyRemovedWeapons.Add(card.Id);
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.OutsideGame, new("equipment.permanent-removal"));
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], range, true);
    }

    internal SkillProgramStepOutcome BeginWeaponReclaim(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var turns = _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent).ToArray();
        var damaged = turns.OfType<DamageAppliedEvent>().Any(item => !item.SourceLess && item.SourceSeat == frame.OwnerSeat && item.Amount > 0);
        var uses = turns.OfType<CardUseDeclaredEvent>().Where(item => item.SourceSeat == frame.OwnerSeat).Count(item => item.CardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash or CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage);
        if (!damaged && uses < 2)
            return SkillProgramStepOutcome.Continue;
        var blade = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat).SelectMany(_corePhysicalOwnedCardQueriesOperations.GetEquipment).FirstOrDefault(card => card.Kind == effect.OutputKind);
        if (blade is null)
            return SkillProgramStepOutcome.Continue;
        var former = _cardZonesOperations._cardZones.GetLocation(blade.Id).OwnerSeat!.Value;
        var affected = AliveSeatsBetweenInclusive(former, frame.OwnerSeat);
        PlaceAdvancedEquipment(blade, frame.OwnerSeat);
        if (_cardZonesOperations._cardZones.GetLocation(blade.Id) != CardLocation.Equipment(frame.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        foreach (var seat in affected)
        {
            var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat)).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[seat])).ToArray();
            if (candidates.Length == 0)
                continue;
            var card = candidates[_randomOperations._random.Next(candidates.Length)];
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Hand(frame.OwnerSeat), new("skill.weapon-reclaim.obtain"));
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<int> AliveSeatsBetweenInclusive(int start, int end)
    {
        var clockwise = new List<int>();
        var counterclockwise = new List<int>();
        for (var seat = start; seat != end; seat = _coreTableOrderOperations.TableStepSeat(seat, 1))
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                clockwise.Add(seat);
        for (var seat = start; seat != end; seat = _coreTableOrderOperations.TableStepSeat(seat, -1))
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                counterclockwise.Add(seat);
        return clockwise.Count <= counterclockwise.Count ? clockwise : counterclockwise;
    }
}
