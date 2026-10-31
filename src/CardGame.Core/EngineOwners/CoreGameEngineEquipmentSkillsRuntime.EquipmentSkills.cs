#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentSkillsRuntime
{
    internal void ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to)
    {
        if (to.Zone == CardZoneKind.Equipment && _characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id))
            to = CardLocation.Processing;
        _coreAdvancedWeaponsOperations.ResolveDynamicEquipmentSkillGrants(card, from, to);
        if (_coreEquipmentContentOperations.EquipmentRules.GetSkillBinding(card.Kind)is not { } binding)
            return;
        var grantId = binding.CreateGrantId(card.Id);
        if (from is { Zone: CardZoneKind.Equipment, OwnerSeat: { } formerSeat })
            _coreMatchParticipantsPlayersAccess._players[formerSeat].SkillGrants.RemoveGrant(grantId);
        if (to is { Zone: CardZoneKind.Equipment, OwnerSeat: { } newSeat } && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(binding.SkillId))
            _coreMatchParticipantsPlayersAccess._players[newSeat].SkillGrants.Grant(new SkillGrant(grantId, binding.SkillId, grantId, grantId));
    }

    internal string? EquipmentSkillGrantId(Card card) => _coreEquipmentContentOperations.EquipmentRules.GetSkillBinding(card.Kind)?.CreateGrantId(card.Id);
    internal bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, Card card)
    {
        var grantId = EquipmentSkillGrantId(card);
        return grantId is not null && _coreMatchParticipantsPlayersAccess._players[ownerSeat].SkillGrants.Grants.Any(grant => grant.IsEnabled && grant.GrantId == grantId && grant.SkillId == skillId && grant.SkillInstanceId == skillInstanceId);
    }
}
