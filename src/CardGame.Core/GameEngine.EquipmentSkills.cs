namespace CardGame.Core;

public sealed partial class GameEngine
{
    private void ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to)
    {
        if (to.Zone == CardZoneKind.Equipment && IsHappyWeiYanBone(card.Id))
            to = CardLocation.Processing;
        ResolveDynamicEquipmentSkillGrants(card, from, to);
        if (EquipmentRules.GetSkillBinding(card.Kind) is not { } binding) return;
        var grantId = binding.CreateGrantId(card.Id);
        if (from is { Zone: CardZoneKind.Equipment, OwnerSeat: { } formerSeat })
            _players[formerSeat].SkillGrants.RemoveGrant(grantId);
        if (to is { Zone: CardZoneKind.Equipment, OwnerSeat: { } newSeat } &&
            _contentRegistry.Skills.ContainsKey(binding.SkillId))
            _players[newSeat].SkillGrants.Grant(new SkillGrant(
                grantId, binding.SkillId, grantId, grantId));
    }

    private string? EquipmentSkillGrantId(Card card) =>
        EquipmentRules.GetSkillBinding(card.Kind)?.CreateGrantId(card.Id);

    private bool IsActiveProgramSourceEquipmentCard(
        int ownerSeat, string skillId, string skillInstanceId, Card card)
    {
        var grantId = EquipmentSkillGrantId(card);
        return grantId is not null && _players[ownerSeat].SkillGrants.Grants.Any(grant =>
            grant.IsEnabled && grant.GrantId == grantId && grant.SkillId == skillId &&
            grant.SkillInstanceId == skillInstanceId);
    }
}
