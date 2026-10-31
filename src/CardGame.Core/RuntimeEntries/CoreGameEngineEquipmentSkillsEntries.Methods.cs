#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSkillsEntries
{
    internal string? EquipmentSkillGrantId(Card card) => _resolve().EquipmentSkillGrantId(card);
    internal bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, Card card) => _resolve().IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, skillInstanceId, card);
    internal void ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to) => _resolve().ResolveEquipmentSkillGrant(card, from, to);
}
