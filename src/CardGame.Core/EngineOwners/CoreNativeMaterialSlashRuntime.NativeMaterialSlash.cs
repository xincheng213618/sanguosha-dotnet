#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeMaterialSlashRuntime
{
    internal void CompleteNativeMaterialSlash(long id, CharacterState owner, IReadOnlyList<Card> cards, CardKind kind, IReadOnlyList<int> seats, CardConversionSource? conversion, bool armor)
    {
        global::CardGame.Core.GameEngine.NuzhanModifiers nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(id, owner);
        foreach (var card in cards)
            _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(owner, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.AdjustedSlashReturn is { } adjusted)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { AdjustedSlashReturn = adjusted with { PaidMovementLastSequence = _coreTrustedMovementDiagnosticsOperations.CardMovements[^1].Sequence } });
        if (use.HpLossMaterialSlashReturn is { } hp)
        {
            hp = hp with
            {
                PaidLast = _coreSlashTargetPenaltyPaymentsOperations.SlashTargetPenaltySequence
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { HpLossMaterialSlashReturn = hp });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HpLossMaterialSlashIssuedEvent(id, hp.ActionId, hp.PolicySource, hp.PolicyHash, hp.FrozenMaximum, hp.ParentProgramFrameId, hp.IsZhangba));
        }

        if (use.NativeMaterialSlashExtraTargets is { } extra)
        {
            extra = extra with
            {
                PaidLast = _coreSlashTargetPenaltyPaymentsOperations.SlashTargetPenaltySequence
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { NativeMaterialSlashExtraTargets = extra });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeMaterialSlashExtraTargetsIssuedEvent(extra));
        }

        var counted = _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreJudgedRankSlashPoliciesOperations.HasIssuedJudgedRankSlashQuota(id, owner.Seat) && !_characterHuangfuSongWumiaoOperations.HasWuHfOrDixianIssuedUse(id, owner.Seat) && !nuzhan.IgnoresSlashLimit && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(owner, conversion) && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && owner.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat;
        if (counted)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, owner.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(owner.Seat, kind);
        var damage = (1 + owner.AlcoholDamageBonus) + nuzhan.DamageBonus;
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, owner);
        owner.HasAlcoholEffect = false;
        global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id, owner.Seat, cards[0], kind, armor, damage, seats, false, counted, conversion);
        _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = pending;
        if (conversion is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(id, conversion.SkillId, conversion.BindingId, owner.Seat, Array.AsReadOnly(cards.Select(c => c.Id).ToArray()), kind, IsUse: true, seats));
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangbaSerpentSpearConvertedEvent(id, owner.Seat, Array.AsReadOnly(cards.Select(c => c.Id).ToArray()), IsUse: true, seats[0]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(cards[0].Id, kind, owner.Seat, seats[0], IgnoresArmor: armor));
        foreach (var seat in seats)
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(owner, _coreMatchParticipantsPlayersAccess._players[seat]);
        _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(pending);
    }

    internal (IReadOnlyList<int> LegalTargets, int OrdinaryMaximum, IReadOnlyList<ProgramSlashTargetExpansion> Claims) NativeMaterialSlashExpansion(CharacterState owner, IReadOnlyList<Card> cards)
    {
        var ids = cards.Select(c => c.Id).ToArray();
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, cards);
        var rank = _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(owner, cards);
        var legal = Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(target => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, target, CardKind.Slash, suit, color, rank, ids) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, target.Seat, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.Slash, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, target.Seat, suit)).Select(target => target.Seat).ToArray());
        var ordinary = Math.Max(1, global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreRuleQueriesOperations.EvaluateCardTargetCount(owner, CardKind.Slash)));
        return (legal, ordinary, _coreProgramTargetRulesOperations.TargetRules.ExtraSlashTargetClaims(new(owner.Seat, CardKind.Slash, suit ?? Suit.None, legal, color)));
    }

    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 31)]
    internal void AddNativeMaterialSlashEquipmentActions(List<LegalAction> actions, CharacterState owner)
    {
        var index = actions.FindIndex(action => action.Kind == LegalActionKind.UseEquipmentEffect && action.EquipmentKind == CardKind.ZhangbaSerpentSpear);
        if (index < 0)
            return;
        var maximum = actions[index].MaxTargetCount;
        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(owner))
        {
            var expansion = NativeMaterialSlashExpansion(owner, pair);
            if (expansion.Claims.Count == 0)
                continue;
            maximum = Math.Max(maximum, Math.Min(expansion.LegalTargets.Count, expansion.OrdinaryMaximum + expansion.Claims.SelectMany(claim => claim.TargetSeats).Distinct().Count()));
        }

        actions[index] = actions[index] with
        {
            MaxTargetCount = maximum
        };
    }

    internal bool TryGetNativeMaterialSlashExpansion(CharacterState owner, IReadOnlyList<int> ids, IReadOnlyList<int> targets, out int ordinaryMaximum, out IReadOnlyList<ProgramSlashTargetExpansion> claims)
    {
        ordinaryMaximum = 0;
        claims = [];
        if (targets.Count < 2 || targets.Distinct().Count() != targets.Count || !_coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(owner))
            return false;
        var cards = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(owner).FirstOrDefault(pair => pair.Select(c => c.Id).Order().SequenceEqual(ids.Order()));
        if (cards is null || !_characterSunQuanOlMagicOperations.IsMoQuanyuCompleteZhangbaQuotaLegal(owner, targets, cards))
            return false;
        var expansion = NativeMaterialSlashExpansion(owner, cards);
        var extra = expansion.Claims.SelectMany(claim => claim.TargetSeats).ToHashSet();
        if (extra.Count == 0 || targets.Any(target => !expansion.LegalTargets.Contains(target)) || targets.Count(target => !extra.Contains(target)) > expansion.OrdinaryMaximum)
            return false;
        ordinaryMaximum = expansion.OrdinaryMaximum;
        claims = expansion.Claims;
        return true;
    }

    internal void BeginNativeMaterialSlashExpansion(CharacterState owner, IReadOnlyList<Card> cards, IReadOnlyList<int> seats)
    {
        var ids = cards.Select(card => card.Id).ToArray();
        if (!TryGetNativeMaterialSlashExpansion(owner, ids, seats, out var ordinary, out var claims))
            throw new InvalidOperationException("The native material Slash lost its exact expanded target authorization.");
        var armor = _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(owner);
        var id = _coreNativeCardUseOperations.BeginCardUse(cards[0], owner.Seat, seats, CardKind.Slash, ignoresArmor: armor, physicalCardIds: ids, isTrueZhangbaSlash: true);
        var action = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!;
        var origin = new NativeMaterialSlashExtraTargetsOrigin(id, action.ActionId, action.ActorSeat, action.ProviderSeat, ordinary, _coreSlashTargetPenaltyPaymentsOperations.SlashTargetPenaltySequence + 1)
        {
            PhysicalCardIds = ids,
            TargetSeats = seats,
            Claims = claims
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { NativeMaterialSlashExtraTargets = origin, TargetsAdjusted = true });
        CompleteNativeMaterialSlash(id, owner, cards, CardKind.Slash, seats, null, armor);
    }

    internal static bool SameNativeMaterialSlashOrigin(NativeMaterialSlashExtraTargetsOrigin left, NativeMaterialSlashExtraTargetsOrigin right) => left.CardUseFrameId == right.CardUseFrameId && left.ActionId == right.ActionId && left.ActorSeat == right.ActorSeat && left.ProviderSeat == right.ProviderSeat && left.OrdinaryMaximum == right.OrdinaryMaximum && left.PaidFirst == right.PaidFirst && left.PaidLast == right.PaidLast && left.PhysicalCardIds.SequenceEqual(right.PhysicalCardIds) && left.TargetSeats.SequenceEqual(right.TargetSeats) && left.Claims.Count == right.Claims.Count && left.Claims.Zip(right.Claims).All(pair => pair.First.Source == pair.Second.Source && pair.First.GameplayHash == pair.Second.GameplayHash && pair.First.TargetSeats.SequenceEqual(pair.Second.TargetSeats));
    internal bool MatchesNativeMaterialSlashExpansion(CardUseFrame use)
    {
        if (use.NativeMaterialSlashExtraTargets is not { } origin || use.Action is not { Type: CardActionType.Use } action || origin.CardUseFrameId != use.Id || origin.ActionId != action.ActionId || origin.ActorSeat != action.ActorSeat || origin.ProviderSeat != action.ProviderSeat || action.ActorSeat != action.ProviderSeat || use.SourceSeat != action.ActorSeat || origin.PhysicalCardIds is not { Count: 2 } ids || ids.Distinct().Count() != 2 || !ids.SequenceEqual(use.PhysicalCardIds ?? []) || !ids.SequenceEqual(action.PhysicalCards.Select(cost => cost.CardId)) || origin.OrdinaryMaximum < 1 || origin.TargetSeats.Count < 2 || origin.TargetSeats.Distinct().Count() != origin.TargetSeats.Count || origin.Claims.Count == 0 || origin.Claims.Any(claim => claim.Source.OwnerSeat != origin.ActorSeat || !_coreProgramTargetRulesOperations.TargetRules.IsExtraSlashTargetSource(claim.Source, claim.GameplayHash) || claim.TargetSeats.Count == 0 || claim.TargetSeats.Distinct().Count() != claim.TargetSeats.Count) || origin.TargetSeats.Count(target => !origin.Claims.Any(claim => claim.TargetSeats.Contains(target))) > origin.OrdinaryMaximum || origin.PaidFirst <= 0 || origin.PaidLast < origin.PaidFirst || action.PhysicalCards.Any(cost => _coreTrustedMovementDiagnosticsOperations.CardMovements.Count(m => m.Sequence >= origin.PaidFirst && m.Sequence <= origin.PaidLast && m.CardId == cost.CardId && m.From == cost.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) != 1) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NativeMaterialSlashExtraTargetsIssuedEvent>().Count(e => SameNativeMaterialSlashOrigin(e.Origin, origin)) != 1)
            return false;
        var issued = action;
        if (use.CurrentSlashFirePolicy is { Converted: true } fire)
        {
            if (!_coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use))
                return false;
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            issued = fire.OriginalAction;
        }

        return global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && action.EffectiveKind == use.CardKind && issued.EffectiveKind == CardKind.Slash && issued.ConversionChain.Count == 0 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Any(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual(origin.TargetSeats)) && (!use.ProgramUseAccepted || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == origin.ActionId && e.Action.ActorSeat == origin.ActorSeat && e.Action.ProviderSeat == origin.ProviderSeat && e.Action.EffectiveKind == CardKind.Slash && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards) && e.Action.ConversionChain.Count == 0)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangbaSerpentSpearConvertedEvent>().Any(e => e.ResolutionId == use.Id && e.UserSeat == origin.ActorSeat && e.IsUse && e.PhysicalCardIds.SequenceEqual(ids));
    }

    internal bool IsExactNativeMaterialSlashExpansion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { NativeMaterialSlashExtraTargets: not null } use || !MatchesNativeMaterialSlashExpansion(use) || attack.CardUserSeat != use.Action!.ActorSeat || !(attack.SourceSeat == use.SourceSeat && attack.EffectiveCardKind == use.CardKind && use.TargetSeats.Contains(attack.TargetSeat) || _coreNativeSlashTargetDuelOperations.IsNativeSlashTargetDuelAttack(attack)) || !attack.PhysicalCards.Select(card => card.Id).SequenceEqual(use.PhysicalCardIds!))
            return false;
        var retained = use.PhysicalCardIds!.Where(id => !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(use.Id, id)).ToHashSet();
        return processing.All(card => retained.Contains(card.Id)) && retained.All(id => _cardZonesOperations._cardZones.GetLocation(id).Zone is CardZoneKind.Processing or CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 33730)]
    internal void AssertNativeMaterialSlashExpansions()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(use => use.NativeMaterialSlashExtraTargets is not null))
            if (!MatchesNativeMaterialSlashExpansion(use))
                throw new InvalidOperationException("The native material Slash lost its issued target source or real payment.");
    }
}
