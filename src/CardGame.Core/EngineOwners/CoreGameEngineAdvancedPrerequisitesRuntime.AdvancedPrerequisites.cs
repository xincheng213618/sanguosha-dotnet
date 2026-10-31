#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAdvancedPrerequisitesRuntime
{
    internal bool AdvancedTriggerPrerequisites(CharacterState owner, string skillId, SkillProgramTrigger trigger)
    {
        foreach (var effect in trigger.Effects)
        {
            if (effect.Op == SkillProgramEffectOp.ReplaceSkillsOnAwakening && (!_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(owner.Seat, skillId, trigger.Id) && _coreLordSkillProjectionsOperations.AdvancedOwnedSkillIds(owner).Count <= owner.MaxHp || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, "awakening", SkillUsageScope.Game) > 0))
                return false;
            if (effect.Op == SkillProgramEffectOp.ObtainDeckRankSum && (effect.Marker is not { } marker || owner.Markers.GetValueOrDefault(marker) <= _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) || !RankSubsetSearch.CanComplete(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(card => card.Rank), effect.MaximumRankSum)))
                return false;
            if (effect.Op == SkillProgramEffectOp.DamageAfterDeckShuffle && !_corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent).OfType<CardMovedEvent>().Any(item => item.Reason == CardMoveReasons.Reshuffle))
                return false;
            if (effect.Op == SkillProgramEffectOp.InheritWeapon && !_coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(_corePhysicalOwnedCardQueriesOperations.GetEquipment).Concat(_corePhysicalOwnedCardQueriesOperations.GetHand(owner)).Any(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == EquipmentSlot.Weapon))
                return false;
            if (effect.Op == SkillProgramEffectOp.ReclaimNamedWeapon && !CanReclaimNamedWeapon(owner, effect))
                return false;
        }

        return true;
    }

    internal bool CanReclaimNamedWeapon(CharacterState owner, SkillProgramEffect effect)
    {
        var turns = _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent).ToArray();
        return (turns.OfType<DamageAppliedEvent>().Any(item => !item.SourceLess && item.SourceSeat == owner.Seat && item.Amount > 0) || turns.OfType<CardUseDeclaredEvent>().Count(item => item.SourceSeat == owner.Seat && item.CardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash or CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage) >= 2) && _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).SelectMany(_corePhysicalOwnedCardQueriesOperations.GetEquipment).Any(card => card.Kind == effect.OutputKind);
    }
}
