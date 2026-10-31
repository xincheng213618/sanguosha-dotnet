#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCuiYanNativeQueriesRuntime
{
    internal bool IsCuiYanSingleUseForbidden(CharacterState actor, Card card, CardConversionSource? conversion = null) => _characterCuiYanOlOperations.HasCuiYanCapability && _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [card.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, card, conversion)));
    internal bool IsCuiYanSlashChoiceForbidden(int actorSeat, PromptChoice choice)
    {
        if (!_characterCuiYanOlOperations.HasCuiYanCapability || choice.Cards is not [var id])
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var conversion);
        var kind = choice.Parameters.TryGetValue("effective-kind", out var name) && Enum.TryParse<CardKind>(name, out var parsed) ? parsed : CardKind.Slash;
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), conversion);
        var suit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actorSeat, card, kind, choice.Cards, conversion is null ? [] : [conversion], _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, choice.Cards, card));
        return _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actorSeat, choice.Cards, suit);
    }

    internal bool IsCuiYanFactionSlashUseForbidden(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, CardKind kind, IReadOnlyList<Card> materials, CardConversionSource? conversion = null, IReadOnlyList<CardConversionSource>? additionalConversions = null)
    {
        if (!_characterCuiYanOlOperations.HasCuiYanCapability || !global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(request) || materials.Count != 1)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[request.OwnerSeat];
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, materials[0], conversion);
        var ids = materials.Select(c => c.Id).ToArray();
        var sources = (conversion is null ? Array.Empty<CardConversionSource>() : [conversion]).Concat(additionalConversions ?? []).ToArray();
        var suit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actor.Seat, appearance, kind, ids, sources, _corePlayCardHistoryOperations.CaptureUsedCardSuit(actor.Seat, ids, appearance));
        return _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, ids, suit);
    }

    internal bool IsCuiYanFactionSlashChoiceForbidden(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, PromptChoice choice)
    {
        if (!_characterCuiYanOlOperations.HasCuiYanCapability || !global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(request) || choice.Cards is not [var id])
            return false;
        global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source);
        var kind = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.ReadResponseCardKind(choice) ?? CardKind.Slash;
        return !CanSupplyCuiYanFactionSlashVariant(request, _coreMatchParticipantsPlayersAccess._players[request.CurrentCandidateSeat], kind, [_coreAdvancedLifecycleOperations.GetAdvancedCard(id)], source);
    }

    internal bool CanSupplyCuiYanFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, CharacterState provider, CardKind kind, IReadOnlyList<Card> materials, CardConversionSource? source = null)
    {
        if (!IsCuiYanFactionSlashUseForbidden(request, kind, materials, source))
            return true;
        // The native fan decision belongs to the requester and occurs after the
        // provider has selected its complete material/conversion variant.
        return _characterCuiYanOlOperations.HasCuiYanCapability && (request.IsProgramSkillUse || request.IsBorrowedSwordUse) && kind == CardKind.Slash && materials is [var primary] && (primary.Kind == CardKind.Slash || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(provider, primary)) && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[request.OwnerSeat]) && !IsCuiYanFactionSlashUseForbidden(request, CardKind.FireSlash, materials, source);
    }

    internal bool HasCuiYanAllowedFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, CharacterState provider, Card card)
    {
        if (!_characterCuiYanOlOperations.HasCuiYanCapability || !global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(request))
            return true;
        var kind = _coreCardConversionsOperations.GetFactionSlashEffectiveSlashKind(request, provider, card);
        return _coreCardConversionsOperations.CreateConversionChoiceVariants(provider, card, kind, false, "faction-suit-query", "", [card.Id], [], new Dictionary<string, string> { ["response-card-kind"] = kind.ToString() }).Any(choice => !IsCuiYanFactionSlashChoiceForbidden(request, choice));
    }

    [ProgramCardLegality(ProgramCardLegalityStage.DyingConversionBan, 10)]
    internal bool CuiYanDyingConversionForbidden(ProgramViewAsSourceQuery query) => query.DyingUse && IsCuiYanSingleUseForbidden(query.Owner, query.Card, query.Source);
}
