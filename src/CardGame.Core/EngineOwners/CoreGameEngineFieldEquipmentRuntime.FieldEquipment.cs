#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFieldEquipmentRuntime
{
    internal EquipmentSlot FieldEquipmentSlot(Card card) => _characterWeiYanHappyGodOperations.HappyBoneSlot(card) ?? _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;

    internal IReadOnlyList<Card> GetEquipmentWithPrintedEffects(CharacterState player) =>
        player.SkillGrants.ArePrintedEquipmentEffectsSuppressed ? [] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(player)
            .Where(card => !_characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id)).ToArray();

    internal bool HasHappyWeiYanPotentialCancellation(int seat) => _characterWeiYanHappyGodOperations.HasHappyWeiYanEzhiCapability && _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Any(card => _characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id));
}
