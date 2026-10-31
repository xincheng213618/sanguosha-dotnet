#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSkillsEntries
{
    internal string? EquipmentSkillGrantId(global::CardGame.Core.Card card) => _resolve().EquipmentSkillGrantId(card);
    internal bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card) => _resolve().IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, skillInstanceId, card);
    internal void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().ResolveEquipmentSkillGrant(card, from, to);
}
