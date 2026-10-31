#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuWumiaoRuntime
{
    internal bool HasWuGuanYuJuewuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRankTwoDamageUse;

    internal bool IsJuewuSource(CardConversionSource? source) => source is { SkillId: "wumiao:juewu" } && _corePlayCardHistoryOperations.ViewAsRule(source)is { RankTwoDamageUse: true } rule && source.BindingId == JuewuComposition.Binding(rule.OutputKind);
    internal CardMovementRecord? JuewuForeignHandEntry(int ownerSeat, int cardId, long beforeSequence = long.MaxValue)
    {
        var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cardId && m.Sequence < beforeSequence && m.To == CardLocation.Hand(ownerSeat));
        if (entry is null)
            return null;
        var origin = _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(entry);
        return origin.OwnerSeat is { } foreign && foreign != ownerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(foreign) ? entry : null;
    }

    internal int? JuewuForeignHandRank(CharacterState owner, Card card) => HasWuGuanYuJuewuCapability && _coreNationalSkillsOperations.HasRuntimeSkill(owner, "wumiao:juewu") && card.Id > 0 && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner.Seat) && JuewuForeignHandEntry(owner.Seat, card.Id)is not null ? 2 : null;
    internal Card ApplyJuewuOwnedAppearance(CharacterState owner, Card card) => JuewuForeignHandRank(owner, card)is { } rank ? card with
    {
        Rank = rank
    }

    : _coreCardDeclarationsOperations.LiveDeclarationPayment(card.Id)is { JuewuEffectiveRank: 2 } paid && paid.ProviderSeat == owner.Seat ? card with
    {
        Rank = 2
    }

    : card;
    internal int? CaptureJuewuActionRank(int providerSeat, IReadOnlyList<int> ids)
    {
        if (!HasWuGuanYuJuewuCapability || ids is not [var id])
            return null;
        if (_coreCardDeclarationsOperations.LiveDeclarationPayment(id)is { JuewuEffectiveRank: 2 } declared && declared.ProviderSeat == providerSeat)
            return 2;
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
        return JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[providerSeat], card);
    }

    internal int? CaptureJuewuResponseRank(CharacterState provider, IReadOnlyList<CardActionCost> costs)
    {
        if (!HasWuGuanYuJuewuCapability || !_coreNationalSkillsOperations.HasRuntimeSkill(provider, "wumiao:juewu") || costs is not [var cost] || cost.From != CardLocation.Hand(provider.Seat))
            return null;
        var departure = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId && m.From == cost.From);
        return departure is not null && departure.From == cost.From && departure.CardKind == cost.CardKind && departure.To == CardLocation.Processing && (departure.Reason == CardMoveReasons.Respond || departure.Reason == CardMoveReasons.Nullification) && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cost.CardId && m.Sequence > departure.Sequence && m.To == CardLocation.Hand(provider.Seat)) && JuewuForeignHandEntry(provider.Seat, cost.CardId, departure.Sequence)is not null ? 2 : null;
    }

    internal bool JuewuPureProviderFrontier(int actorSeat)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long id) => top?.Id == id || top is ResponseWindowFrame response && response.ParentFrameId == id;
        return _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } slash && slash.CurrentCandidateSeat == actorSeat && At(slash.OwnerFrameId) || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense && defense.CurrentCandidateSeat == actorSeat && At(defense.Attack.ResolutionId);
    }

    internal bool IsJuewuTrueSlashUse(CharacterState owner)
    {
        if (!owner.IsAlive || JuewuPureProviderFrontier(owner.Seat))
            return false;
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long id) => top?.Id == id || top is ResponseWindowFrame response && response.ParentFrameId == id;
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: null, AwaitingSlashChoice: true } borrowed && borrowed.WeaponOwnerSeat == owner.Seat && At(borrowed.ResolutionId))
            return true;
        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && qinglong.Attack.SourceSeat == owner.Seat && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglong.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && At(qinglong.Attack.ResolutionId))
            return true;
        if (top is not ProgramSkillFrame program || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(program)is not { } effect)
            return false;
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(program))
            return program.LvKuangLvXiang!.Actor == owner.Seat;
        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(program) || _characterWuJingOlOperations.IsWuJingHejiRequest(program))
            return program.OwnerSeat == owner.Seat;
        return _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(program, effect.Op, owner.Seat);
    }

    internal bool CanUseJuewuConversion(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse, bool dyingUse = false, int? requestedUseTarget = null)
    {
        if (!rule.RankTwoDamageUse)
            return true;
        var source = new CardConversionSource(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId);
        var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        return HasWuGuanYuJuewuCapability && IsJuewuSource(source) && owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId) && location.OwnerSeat == owner.Seat && (location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment || location.Zone == CardZoneKind.WoodenOxGrain && _coreNativeDamageHandlesOperations.UsesFormalWoodenOx && _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(c => c.Kind == CardKind.WoodenOx)) && _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(owner, card) == 2 && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) && !dyingUse && requestedUseTarget is null && !JuewuPureProviderFrontier(owner.Seat) && _coreActivityOrdinaryTricksOperations.IsActivityOrdinaryTrickNameAvailable(rule.OutputKind) && (!forResponse || JuewuComposition.IsSlash(rule.OutputKind) && IsJuewuTrueSlashUse(owner));
    }

    internal bool JuewuSlashAliasAllowed(CharacterState owner, bool forResponse) => !forResponse || IsJuewuTrueSlashUse(owner);
    internal bool JuewuNativeConversionMatchesOutput(CharacterState owner, CardConversionSource source, CardKind kind) => !IsJuewuSource(source) || !JuewuExplicitForcedSlashFrontier(owner) || _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind;
    internal bool JuewuExplicitForcedSlashFrontier(CharacterState owner)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long id) => top?.Id == id || top is ResponseWindowFrame response && response.ParentFrameId == id;
        return _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: null, AwaitingSlashChoice: true } borrowed && borrowed.WeaponOwnerSeat == owner.Seat && At(borrowed.ResolutionId) || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && qinglong.Attack.SourceSeat == owner.Seat && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglong.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && At(qinglong.Attack.ResolutionId);
    }

    internal IReadOnlyList<SlashUseVariant> JuewuPhysicalSlashVariants(CharacterState owner, Card card) => global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Where(kind => _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, kind, false).Any(source => IsJuewuSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind)).Select(kind => new SlashUseVariant(kind, AdditionalConversionSource: null, UsesZhuqueFan: false)).ToArray();
    internal CardKind JuewuSelectedSlashKind(CharacterState owner, Card card, CardKind fallback, CardConversionSource? explicitSource = null)
    {
        var selected = explicitSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        return IsJuewuSource(selected) && _corePlayCardHistoryOperations.ViewAsRule(selected!)is { } rule && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) && _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, rule.OutputKind, false).Contains(selected!) ? rule.OutputKind : fallback;
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 50)]
    internal bool JuewuConversionUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseJuewuConversion(query.Owner, query.Card, query.Instance, query.Rule, query.ForResponse, query.DyingUse, query.RequestedUseTarget);
}
