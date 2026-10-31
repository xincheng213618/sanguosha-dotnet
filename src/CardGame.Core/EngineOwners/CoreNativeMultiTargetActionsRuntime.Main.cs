#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeMultiTargetActionsRuntime
{
    internal IReadOnlyList<CharacterState> GetFangtianOrderedSlashTargets(CharacterState actor, Card slash, CardConversionSource? conversionSource = null, bool ignoresSlashLimit = false, CardKind effectiveKind = CardKind.Slash)
    {
        slash = _characterWuJingOlOperations.WuJingLiubingUseCard(actor, slash, effectiveKind, conversionSource);
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, effectiveKind, CardActionType.Use))
        {
            return[];
        }

        return _coreMatchParticipantsPlayersAccess._players.Where(player => _coreRuleQueriesOperations.CanSpendSlashUse(actor, player, ignoresSlashLimit || _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, conversionSource), effectiveKind, slash) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, player, slash, conversionSource, effectiveKind)).ToArray();
    }

    internal void AddFangtianHalberdSlashActions(ICollection<LegalAction> actions, CharacterState actor, Card physicalCard, IReadOnlyList<CharacterState> legalTargets, string slashName, CardKind? playedCardKind, CardConversionSource? conversionSource = null, IReadOnlyList<CardConversionSource>? additionalConversionSources = null)
    {
        if (!_coreNativeDamageHandlesOperations.Value.UsesFormalFangtianHalberd || _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count != 1 || _corePhysicalOwnedCardQueriesOperations.GetHand(actor)[0].Id != physicalCard.Id || !_coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.FangtianHalberd) || legalTargets.Count < 2)
        {
            return;
        }

        var orderedTargets = legalTargets.OrderBy(player => _coreTableOrderOperations.RelativeTableOrder(actor.Seat, player.Seat)).ToArray();
        var maximumTargets = Math.Min(3, orderedTargets.Length);
        for (var targetCount = 2; targetCount <= maximumTargets; targetCount++)
        {
            AddCombinations(0, []);
            void AddCombinations(int startIndex, IReadOnlyList<CharacterState> selected)
            {
                if (selected.Count == targetCount)
                {
                    AddFangtianHalberdSlashAction(actions, physicalCard, slashName, playedCardKind, selected, conversionSource, additionalConversionSources);
                    return;
                }

                for (var index = startIndex; index <= orderedTargets.Length - (targetCount - selected.Count); index++)
                {
                    AddCombinations(index + 1, [..selected, orderedTargets[index]]);
                }
            }
        }
    }

    internal void AddFangtianHalberdSlashAction(ICollection<LegalAction> actions, Card physicalCard, string slashName, CardKind? playedCardKind, IReadOnlyList<CharacterState> targets, CardConversionSource? conversionSource, IReadOnlyList<CardConversionSource>? additionalConversionSources)
    {
        var targetSeats = Array.AsReadOnly(targets.Select(target => target.Seat).ToArray());
        actions.Add(new LegalAction(LegalActionKind.Slash, physicalCard.Id, targetSeats[0], _coreCardConversionsOperations.DescribeConversion(conversionSource, $"发动【方天画戟】，对 {(string.Join("、", targets.Select(target => target.Name)))} 使用【{(slashName)}】"), PlayedCardKind: playedCardKind, TargetSeats: targetSeats) { ConversionSource = conversionSource, AdditionalConversionSources = additionalConversionSources, });
    }

    internal void AddTargetCardActions(ICollection<LegalAction> actions, int actorSeat, LegalActionKind actionKind, Card effectCard, CharacterState target, string effectName, CardKind? playedCardKind = null, CardConversionSource? conversionSource = null)
    {
        var useDescription = playedCardKind is null ? $"对 {(target.Name)} 使用【{(effectName)}】" : $"将【{(effectCard.DisplayName)}】当作【{(effectName)}】对 {(target.Name)} 使用";
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0)
        {
            actions.Add(new LegalAction(actionKind, effectCard.Id, target.Seat, useDescription, PlayedCardKind: playedCardKind) { ConversionSource = conversionSource });
        }

        foreach (var equipment in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target))
        {
            if (actionKind == LegalActionKind.Dismantlement && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actorSeat, equipment, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard))
                continue;
            actions.Add(new LegalAction(actionKind, effectCard.Id, target.Seat, $"{(useDescription)}，选择其装备【{(equipment.DisplayName)}】", PlayedCardKind: playedCardKind, TargetCardId: equipment.Id) { ConversionSource = conversionSource });
        }

        foreach (var judgment in _corePhysicalOwnedCardQueriesOperations.GetJudgment(target))
        {
            actions.Add(new LegalAction(actionKind, effectCard.Id, target.Seat, $"{(useDescription)}，选择其判定区【{(judgment.DisplayName)}】", PlayedCardKind: playedCardKind, TargetCardId: judgment.Id) { ConversionSource = conversionSource });
        }
    }
}
