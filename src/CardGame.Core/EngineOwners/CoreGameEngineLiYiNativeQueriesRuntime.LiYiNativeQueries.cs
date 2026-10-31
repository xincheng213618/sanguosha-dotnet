#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLiYiNativeQueriesRuntime
{
    internal int? LiYiPaidSlashWeapon(CharacterState actor, IReadOnlyList<int> materialIds) => actor.IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.EmptyWeaponStoneAxe) ? _corePhysicalOwnedCardQueriesOperations.GetEquipment(actor).Where(card => materialIds.Contains(card.Id) && _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon).Select(card => (int? )card.Id).SingleOrDefault() : null;
    internal bool HasLiYiWeaponPaymentRange(CharacterState actor, int targetSeat, IReadOnlyList<Card> cards) => LiYiPaidSlashWeapon(actor, cards.Select(card => card.Id).ToArray())is { } weaponId && _characterLiYiOlOperations.HasLiYiVirtualStoneAxe(actor, weaponId) && global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], weaponId)) <= global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(actor, weaponId));
    internal bool HasLiYiProspectiveWeaponSlashTarget(CharacterState actor, CharacterState target)
    {
        if (!actor.IsAlive || !_coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.EmptyWeaponStoneAxe))
            return false;
        foreach (var weapon in _corePhysicalOwnedCardQueriesOperations.GetEquipment(actor).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon))
            foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, weapon, kind, forResponse: false))
                    if (_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, weapon, source, kind, physicalCardIds: [weapon.Id], excludedEquipmentId: weapon.Id))
                        return true;
        return false;
    }
}
