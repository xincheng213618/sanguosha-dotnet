#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFieldEquipmentRuntime
{
    internal EquipmentSlot FieldEquipmentSlot(Card card) => _characterWeiYanHappyGodOperations.HappyBoneSlot(card) ?? _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
    internal bool HasHappyWeiYanPotentialCancellation(int seat) => _characterWeiYanHappyGodOperations.HasHappyWeiYanEzhiCapability && _corePhysicalOwnedCardQueriesOperations.GetEquipment(seat).Any(card => _characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id));
}
