#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCardPoliciesRuntime
{
    internal IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> CardPolicies(CharacterState owner, SkillProgramCardPolicyKind kind, CardKind? effectiveKind = null, CardKind? requiredKind = null, bool? eventTargetIsFemale = null, bool? eventSourceIsFemale = null)
    {
        // Binding shards already preserve SkillId/instance order. Most queries have
        // no matching policy, so do not build a gameplay context for those reads.
        PlayerSkillContext? context = null;
        foreach (var(instance, policy)in _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(kind))
        {
            if ((policy.Kind != SkillProgramCardPolicyKind.IgnoreUseDistanceBeforeDealingDamage || _coreLeastHandMarkerDrawProgramsOperations.IsBeforeActualDamageDistancePolicyQualified(owner)) && (policy.OwnerRole is null || _coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, instance.SkillId, instance.SkillInstanceId, policy.OwnerRole.Value)) && (policy.CardKinds.Count == 0 || effectiveKind is { } card && ProgramCardKindMatch.Contains(policy.CardKinds, card)) && (policy.RequiredCardKinds.Count == 0 || requiredKind is { } response && policy.RequiredCardKinds.Contains(response)) && policy.Condition.Evaluate(context ??= _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner), null, eventTargetIsFemale, eventSourceIsFemale))
                yield return (instance, policy);
        }
    }

    internal bool HasCardPolicy(CharacterState owner, SkillProgramCardPolicyKind kind, CardKind? effectiveKind = null) => kind == SkillProgramCardPolicyKind.UnlimitedAlcoholUse && _coreProgramAlcoholQuotaOperations.HasAdditionalAlcoholUse(owner) || (kind is SkillProgramCardPolicyKind.IgnoreUseDistance or SkillProgramCardPolicyKind.UnlimitedAlcoholUse) && _characterHanSongAnniversaryOperations.HasAnniversaryHanSongUniqueHand(owner) || (kind is SkillProgramCardPolicyKind.IgnoreUseDistance or SkillProgramCardPolicyKind.UnlimitedAlcoholUse) && _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(owner) || kind == SkillProgramCardPolicyKind.UnlimitedAlcoholUse && _characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(owner) || _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(kind).Count != 0 && CardPolicies(owner, kind, effectiveKind).Any();
    internal bool HasOutsideAttackRangeSlashQuota(CharacterState owner, CharacterState target, CardKind kind, EquipmentSlot? excludedEquipmentSlot = null) => owner.IsAlive && target.IsAlive && owner.Seat != target.Seat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && !_characterSunYuanOlOperations.HasSayingRoundRange(owner.Seat, target.Seat) && HasCardPolicy(owner, SkillProgramCardPolicyKind.BypassSlashLimitAgainstOutsideAttackRange, kind) && (excludedEquipmentSlot is null ? _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, target.Seat) > _coreNativeRuleQueriesOperations.GetAttackRange(owner.Seat) : global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(owner, target, excludedEquipmentSlot: excludedEquipmentSlot)) > global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(owner, excludedEquipmentSlot: excludedEquipmentSlot)));
    internal bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats)
    {
        if (_coreShanheTrickResponseOperations.IsShanheTrickResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_coreShanheS5QuyiOperations.IsShanheS5FuqiResponseProhibited(sourceSeat, responderSeat))
            return true;
        if (_characterXiahouHuiAnniversaryOperations.AnniversaryXiahouHuiCannotRespond(sourceSeat, responderSeat))
            return true;
        if (_characterZhangFeiAnniversaryGodOperations.ShenZfCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_characterSunCeMobileGodOperations.ShenSunCeCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_coreWushuangSkillsBAuditOperations.WushuangFuqiCannotRespond(sourceSeat, responderSeat))
            return true;
        if (_characterCaoPiMouOperations.MouCaoPiCannotRespondTo(responderSeat, sourceSeat))
            return true;
        if (_characterLuZhiMouOlOperations.MouLzCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_characterZhongHuiMobileShiOperations.ShiZhHuiKeChangCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_characterXiahouBaMobileShiOperations.ShiXbPoxiCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_characterSunShaoMobileShiOperations.ShiSsGanjueCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats))
            return true;
        if (_characterLvYiMobileShiOperations.ShiLyShishiCannotRespond(sourceSeat, responderSeat))
            return true;
        if ((!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind) && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(incomingKind)) || sourceSeat == responderSeat || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[responderSeat].IsAlive)
            return false;
        return CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], SkillProgramCardPolicyKind.ProhibitNearbyTargetResponse, incomingKind).Any(item => (item.Policy.IncludeNonTargetResponders == true || targetSeats.Contains(responderSeat)) && (item.Policy.UseIncomingResponseDistance == true ? _coreNativeRuleQueriesOperations.GetCombatDistance(responderSeat, sourceSeat) : _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, responderSeat)) <= item.Policy.Value);
    }

    internal bool IsSuitSlashResponseProhibited(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || attack.CardUserSeat == attack.TargetSeat || attack.PhysicalCards.Count == 0)
            return false;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat];
        return CardPolicies(source, SkillProgramCardPolicyKind.ProhibitTargetSlashResponseBySuit, kind).Any(item => attack.PhysicalCards.Any(card => _coreNativeCardAppearanceOperations.EffectiveSuit(source, card) == item.Policy.InputSuit));
    }

    internal int GetProgramRequiredResponseCount(CharacterState owner, int sourceSeat, int responderSeat, CardKind incomingKind, CardKind requiredKind)
    {
        // Roulin's two directions share the response-count query: an attacker-side
        // MinimumResponseCount policy keys on the attack target's gender while a
        // defender-side MinimumResponseCountAsTarget policy keys on the attacker's.
        // The max (never a product) matches the Wushuang aggregation precedent.
        var attackerSide = sourceSeat == owner.Seat ? CardPolicies(owner, SkillProgramCardPolicyKind.MinimumResponseCount, incomingKind, requiredKind, eventTargetIsFemale: _coreMatchParticipantsPlayersAccess._players[responderSeat].Gender == GeneralGender.Female, eventSourceIsFemale: _coreMatchParticipantsPlayersAccess._players[sourceSeat].Gender == GeneralGender.Female).Select(item => item.Policy.Value).DefaultIfEmpty(1).Max() : 1;
        var targetSide = CardPolicies(_coreMatchParticipantsPlayersAccess._players[responderSeat], SkillProgramCardPolicyKind.MinimumResponseCountAsTarget, incomingKind, requiredKind, eventTargetIsFemale: _coreMatchParticipantsPlayersAccess._players[responderSeat].Gender == GeneralGender.Female, eventSourceIsFemale: _coreMatchParticipantsPlayersAccess._players[sourceSeat].Gender == GeneralGender.Female).Select(item => item.Policy.Value).DefaultIfEmpty(0).Max();
        var shortRange = sourceSeat == owner.Seat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind) && requiredKind == CardKind.Dodge && _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, responderSeat) == 1 ? CardPolicies(owner, SkillProgramCardPolicyKind.MinimumSlashResponseAtDistanceOne, incomingKind, requiredKind).Select(item => item.Policy.Value).DefaultIfEmpty(1).Max() : 1;
        return Math.Max(_coreShanheBossProgramsOperations.GetBattleSlashCountResponseRequirement(owner, sourceSeat, incomingKind, requiredKind), Math.Max(shortRange, Math.Max(attackerSide, targetSide)));
    }

    internal Suit GetProgramEffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation = null)
    {
        if (_characterZhangFeiAnniversaryGodOperations.ShenZfIdentitySuit(owner, card, identityLocation)is { } shenZfSuit)
            return shenZfSuit;
        if (_characterXiaoQiaoBoundaryMobileOperations.BoundaryXiaoQiaoEffectiveSuit(owner, card, identityLocation)is { } mobileSuit)
            return mobileSuit;
        if (_coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(SkillProgramCardPolicyKind.RewriteSuit).Count == 0)
            return card.Suit;
        var suit = card.Suit;
        foreach (var item in CardPolicies(owner, SkillProgramCardPolicyKind.RewriteSuit))
            if (item.Policy.InputSuit == suit)
                suit = item.Policy.OutputSuit!.Value;
        return suit;
    }

    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 30)]
    internal bool OutsideAttackRangeSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasOutsideAttackRangeSlashQuota(query.Actor, query.Target, query.Kind, query.ExcludedEquipmentSlot);
}
