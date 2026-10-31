#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChaoAnniversaryGodRuntime
{
    internal bool HasHengwuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawHengwuFieldSuitEquipment);

    internal bool ShouliIgnoresSlashLimit(CharacterState owner, CardConversionSource? source) => source is { SkillId: ShenMaChaoComposition.Shouli, BindingId: ShenMaChaoComposition.Slash } && source.OwnerSeat == owner.Seat && _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetSingleCardConversions(CardKind.Slash).Any(binding => binding.Source.SkillId == source.SkillId && binding.Source.SkillInstanceId == source.SkillInstanceId && binding.Rule.Id == source.BindingId && binding.Rule.FieldMountConversion == true);
    internal static CardKind? ShouliMountOutput(Card card) => !EquipmentCatalog.IsEquipment(card.Kind) ? null : EquipmentCatalog.Get(card.Kind).Slot switch
    {
        EquipmentSlot.OffensiveHorse => CardKind.Slash,
        EquipmentSlot.DefensiveHorse => CardKind.Dodge,
        _ => null
    };
    internal bool IsShouliFieldMaterial(CharacterState actor, Card card, out CardLocation location)
    {
        location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        return actor.IsAlive && ShouliMountOutput(card)is not null && location.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && location.OwnerSeat is { } owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _cardZonesOperations._cardZones.CardsAt(location).Any(c => c.Id == card.Id && c.Kind == card.Kind);
    }

    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule)> ShouliSources(CharacterState actor, Card card, bool response)
    {
        if (!IsShouliFieldMaterial(actor, card, out _) || _coreResponseEntityExchangeOperations.IsResponseEntityRestricted(actor.Seat, card.Id))
            yield break;
        var output = ShouliMountOutput(card)!.Value;
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, output, response ? CardActionType.Response : CardActionType.Use) || _coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(actor.Seat, card.Id) || _coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(actor.Seat, output, [card.Id], response))
            yield break;
        foreach (var(instance, rule)in _coreNationalSkillsOperations.GetSkillBindingShard(actor).GetSingleCardConversions(output))
        {
            if (instance.SkillId != ShenMaChaoComposition.Shouli || rule.FieldMountConversion != true || !rule.InputKinds.Contains(card.Kind) || !(response ? rule.ForResponse : rule.ForPlay) || !rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor)))
                continue;
            var source = new CardConversionSource(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId);
            if (_coreProgramCardLegalityOperations.IsProgramViewAsAllowed(actor, output, [card.Id], source, response) && !_coreProgramCardLegalityOperations.CardLegality.IsConversionSourceForbidden(ProgramCardLegalityStage.ConversionSourceBan, new(actor, card, source, response, false)))
                yield return (source, rule);
        }
    }

    internal IReadOnlyList<CardConversionSource> ShouliFieldConversions(CharacterState actor, Card card, CardKind output, bool response, bool dyingUse)
    {
        if (dyingUse || !IsShouliFieldMaterial(actor, card, out var from) || from == CardLocation.Equipment(actor.Seat) || ShouliMountOutput(card) != output)
            return[];
        return Array.AsReadOnly(ShouliSources(actor, card, response).Select(x => x.Source).ToArray());
    }

    internal IEnumerable<Card> ShouliFieldCards(CharacterState actor) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(p))).Where(c => ShouliMountOutput(c)is not null).DistinctBy(c => c.Id);
    internal IReadOnlyList<Card> ShouliFieldResponseCards(CharacterState actor, CardKind requiredKind) => requiredKind is not (CardKind.Slash or CardKind.Dodge) ? [] : Array.AsReadOnly(ShouliFieldCards(actor).Where(c => ShouliMountOutput(c) == requiredKind && ShouliSources(actor, c, true).Any()).ToArray());
    internal Card? FindShouliFieldCard(CharacterState actor, int? id) => id is { } actual ? ShouliFieldCards(actor).SingleOrDefault(c => c.Id == actual && ShouliMountOutput(c) == CardKind.Slash && ShouliSources(actor, c, false).Any()) : null;
    internal bool IsAcceptedShouliMaterial(CharacterState actor, Card card, CardConversionSource? source, bool response) => source is { SkillId: ShenMaChaoComposition.Shouli } && source.OwnerSeat == actor.Seat && ShouliSources(actor, card, response).Any(s => s.Source == source);
    internal bool TryShouliFieldLocation(CharacterState actor, Card card, out CardLocation location)
    {
        if (!IsShouliFieldMaterial(actor, card, out location))
            return false;
        if (IsAcceptedShouliMaterial(actor, card, _coreGameEngineCardConversionsSelectedUseConversionAccess._selectedUseConversion, false))
            return true;
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action;
        return action is not null && action.ProviderSeat == actor.Seat && action.PhysicalCards is [var cost] && cost.CardId == card.Id && cost.From == location && action.ConversionChain.Any(s => IsAcceptedShouliMaterial(actor, card, s, false));
    }

    internal CardLocation FindOwnedCardLocation(CharacterState actor, Card card, CardConversionSource? acceptedConversion) => IsAcceptedShouliMaterial(actor, card, acceptedConversion, true) && IsShouliFieldMaterial(actor, card, out var from) ? from : _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card);
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 39000)]
    internal void AddShouliForeignSlashActions(List<LegalAction> actions, CharacterState actor, int? selectedPhysicalCardId) => actions.AddRange(ShouliForeignSlashActions(actor).Where(action => selectedPhysicalCardId is null || action.CardId == selectedPhysicalCardId));
    internal IEnumerable<LegalAction> ShouliForeignSlashActions(CharacterState actor)
    {
        if (actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !actor.IsAlive)
            yield break;
        foreach (var card in ShouliFieldCards(actor).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != CardLocation.Equipment(actor.Seat) && ShouliMountOutput(c) == CardKind.Slash))
            foreach (var(source, _)in ShouliSources(actor, card, false))
                foreach (var variant in _coreProgramCardUseRulesOperations.GetSlashUseVariants(actor, card, CardKind.Slash, source))
                    foreach (var target in _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: variant.EffectiveKind, ignoresSlashLimit: true))
                        yield return new(LegalActionKind.Slash, card.Id, target.Seat, $"【狩骊】将 {(_coreMatchParticipantsPlayersAccess._players[_cardZonesOperations._cardZones.GetLocation(card.Id).OwnerSeat!.Value].Name)} 场上的【{(card.DisplayName)}】当【{(CardCatalog.Get(variant.EffectiveKind).DisplayName)}】使用", variant.EffectiveKind)
                        {
                            ConversionSource = source,
                            AdditionalConversionSources = variant.UsesZhuqueFan ? [] : null
                        };
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AcceptedConversionCost, 39000)]
    internal void IssueShouliBorrowedMountDebt(CardActionContext action)
    {
        if (action.PhysicalCards is not [var cost] || cost.From.Zone is not (CardZoneKind.Equipment or CardZoneKind.Judgment) || cost.From.OwnerSeat is not { } originalOwner || action.ConversionChain.SingleOrDefault(s => s.SkillId == ShenMaChaoComposition.Shouli)is not { } source || originalOwner == source.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShouliBorrowedMountEvent>().Any(e => e.ActionId == action.ActionId && e.Source == source))
            return;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(originalOwner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || _corePlayCardHistoryOperations.ViewAsRule(source)is not { FieldMountConversion: true } rule || !rule.InputKinds.Contains(cost.CardKind) || (rule.OutputKind == CardKind.Slash ? !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) : action.EffectiveKind != CardKind.Dodge) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cost.CardId && m.CardKind == cost.CardKind && m.From == cost.From && m.To == CardLocation.Processing && m.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && m.Reason == (action.Type == CardActionType.Use ? CardMoveReasons.Use : CardMoveReasons.Respond)))
            throw new InvalidOperationException("Shouli debt requires its exact accepted real field-mount conversion payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShouliBorrowedMountEvent(action.ActionId, source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, cost.CardId, cost.From));
        if (!_coreMatchParticipantsPlayersAccess._players[originalOwner].IsAlive ||
            _coreMatchParticipantsPlayersAccess._players[originalOwner].SkillGrants.Grants.Any(g => g.SkillState is
                { Action: SkillStateAction.Suppress, Scope: SkillStateScope.NonLocked, Duration: SkillStateDuration.CurrentTurn,
                    NamedSkill: null, PrintedOnly: false, CharacterOnly: false } state &&
                state.IssuedTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber &&
                state.EndSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat))
            return;
        var parentId = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == action.ActionId)?.Id ?? _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId ?? throw new InvalidOperationException("Shouli's paid suppression lost its native original owner.");
        _coreProgramSkillChangesOperations.IssueSkillState(new(parentId, 0,
            new(source.SkillId, source.BindingId, source.OwnerSeat, source.SkillInstanceId), parentId, action.ActionId), originalOwner,
            new(SkillStateAction.Suppress, SkillStateScope.NonLocked, SkillStateDuration.CurrentTurn) { CharacterOnly = false }, []);
    }

    internal bool HasShouliDamageDebt(int target) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DealShouliStartingMounts) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShouliBorrowedMountEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && (e.Source.OwnerSeat == target || e.From.OwnerSeat == target));
    internal DamageNature? ShouliThunderNature(GameEngine.IDamageAttempt attack) => HasShouliDamageDebt(attack.TargetSeat) ? DamageNature.Thunder : null;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 39000)]
    internal int ShouliReceivedDamageBonus(GameEngine.IDamageAttempt attack) => HasShouliDamageDebt(attack.TargetSeat) ? 1 : 0;
}
