#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWuJingQueriesRuntime
{
    internal bool IsTurnSlashChoiceUseForbidden(int actorSeat, PromptChoice choice)
    {
        if (choice.Cards is not [var id])
            return _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actorSeat, choice.Cards);
        CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var conversion);
        var kind = choice.Parameters.TryGetValue("effective-kind", out var name) && Enum.TryParse<CardKind>(name, out var parsed)
            ? parsed : CardKind.Slash;
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var conversions = conversion is null ? Array.Empty<CardConversionSource>() : [conversion];
        var suit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actorSeat, card, kind, choice.Cards,
            conversions, _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, choice.Cards, card));
        var turnForbidden = !_characterWuJingOlOperations.HasWuJingLiubing
            ? _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actorSeat, choice.Cards)
            : _coreTurnHandCategoryRestrictionsOperations.IsHandCategoryMaterialRestricted(choice.Cards) ||
                suit is { } value && _coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actorSeat, value);
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, card, conversion);
        var usedSuit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actorSeat, appearance, kind,
            choice.Cards, conversions, _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, choice.Cards, appearance));
        return turnForbidden || _appliedCardActionRestrictions.IsMaterialUseForbidden(actorSeat, choice.Cards, usedSuit);
    }
}
