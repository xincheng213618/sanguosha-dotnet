#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActionQueriesRuntime
{
    internal NativeOwners.ActionQueryScope BeginActionQuery()
    {
        if (_coreGameEngineActionQueriesActionQueryAccess._actionQuery is not null)
            return default;
        _coreGameEngineActionQueriesActionQueryAccess._actionQuery = new NativeOwners.ActionQuery(_coreMatchParticipantsPlayersAccess._players.Count);
        return _kernelCoreGameEngineActionQueriesOperations.CreateActionQueryScope();
    }

    internal IReadOnlyList<GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(CharacterState owner, Card card)
    {
        if (_coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return _coreCardConversionsOperations.FindProgramCardIdentityMatches(owner, card);
        if (query.PrintedTargetTemplate is { } printed && printed.Seat == owner.Seat && printed.Card == card)
            return[];
        var key = (owner.Seat, card);
        if (!query.Identities.TryGetValue(key, out var result))
            query.Identities[key] = result = _coreCardConversionsOperations.FindProgramCardIdentityMatches(owner, card);
        return result;
    }

    internal IReadOnlyList<LegalAction> BuildMobileWumouPrintedTargetActions(CharacterState actor, Card card)
    {
        var previous = _coreGameEngineActionQueriesActionQueryAccess._actionQuery;
        _coreGameEngineActionQueriesActionQueryAccess._actionQuery = new NativeOwners.ActionQuery(_coreMatchParticipantsPlayersAccess._players.Count, (actor.Seat, card));
        using var scope = _kernelCoreGameEngineActionQueriesOperations.CreateActionQueryScope(previous);
        return Array.AsReadOnly(_coreNativeLegalActionsOperations.BuildLegalActionsCore(actor, includeProgramActions: false, selectedPhysicalCardId: card.Id, nativeOriginalCard: card).Where(action => action.CardId == card.Id && action.ConversionSource is null && (action.AdditionalConversionSources is null || action.AdditionalConversionSources.Count == 0) && action.Kind is not (LegalActionKind.Recast or LegalActionKind.EndPlay or LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill or LegalActionKind.RevealGeneral) && action.MaxCardCount <= 1 && (action.PlayedCardKind ?? card.Kind) == card.Kind).ToArray());
    }

    internal IReadOnlyList<CardConversionSource> GetProgramViewAsConversions(CharacterState owner, Card card, CardKind outputKind, bool forResponse, bool dyingUse = false)
    {
        if (_coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return _coreCardConversionsOperations.FindProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
        var key = (owner.Seat, card, outputKind, forResponse, dyingUse);
        if (!query.Conversions.TryGetValue(key, out var result))
            query.Conversions[key] = result = _coreCardConversionsOperations.FindProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
        return result;
    }

    internal RuleQueryEvaluation EvaluateDistance(CharacterState source, CharacterState target, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null)
    {
        if (excludedEquipmentSlot is not null || _coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return _coreRuleQueriesOperations.CalculateDistance(source, target, excludedEquipmentId, excludedEquipmentSlot);
        var key = (source.Seat, target.Seat, excludedEquipmentId);
        if (!query.Distances.TryGetValue(key, out var result))
            query.Distances[key] = result = _coreRuleQueriesOperations.CalculateDistance(source, target, excludedEquipmentId);
        return result;
    }

    internal RuleQueryEvaluation EvaluateAttackRange(CharacterState player, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null)
    {
        if (excludedEquipmentSlot is not null || _coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return _coreRuleQueriesOperations.CalculateAttackRange(player, excludedEquipmentId, excludedEquipmentSlot);
        var key = (player.Seat, excludedEquipmentId);
        if (!query.AttackRanges.TryGetValue(key, out var result))
            query.AttackRanges[key] = result = _coreRuleQueriesOperations.CalculateAttackRange(player, excludedEquipmentId);
        return result;
    }

    internal RuleQueryEvaluation EvaluateSlashUseLimit(CharacterState player, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null)
    {
        if (_coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return _coreRuleQueriesOperations.CalculateSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
        var key = (player.Seat, excludedEquipmentId, excludedEquipmentSlot);
        if (!query.SlashUseLimits.TryGetValue(key, out var result))
            query.SlashUseLimits[key] = result = _coreRuleQueriesOperations.CalculateSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
        return result;
    }

    internal IReadOnlyList<Card> GetSlashUseCards(CharacterState owner, bool ignoreSuitUseProhibition = false)
    {
        if (_coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return _coreCardConversionsOperations.FindSlashUseCards(owner, ignoreSuitUseProhibition);
        var key = (owner.Seat, ignoreSuitUseProhibition);
        if (!query.SlashCards.TryGetValue(key, out var result))
            query.SlashCards[key] = result = _coreCardConversionsOperations.FindSlashUseCards(owner, ignoreSuitUseProhibition);
        return result;
    }
}
