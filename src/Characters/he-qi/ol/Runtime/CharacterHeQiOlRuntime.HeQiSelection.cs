#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiOlRuntime
{
    bool global::CardGame.Core.IHeQiSelectionQueries.HasRoundRange(int sourceSeat, int targetSeat) => _characterSunYuanOlOperations.HasSayingRoundRange(sourceSeat, targetSeat);
    int global::CardGame.Core.IHeQiSelectionQueries.CombatDistance(int sourceSeat, int targetSeat) => _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, targetSeat);
    int global::CardGame.Core.IHeQiSelectionQueries.AttackRange(int seat) => _coreNativeRuleQueriesOperations.GetAttackRange(seat);
    int global::CardGame.Core.IHeQiSelectionQueries.EquipmentSlotCapacity(int seat, EquipmentSlot slot) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot);
    int global::CardGame.Core.IHeQiSelectionQueries.EquipmentCountInSlot(int seat, EquipmentSlot slot) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == slot);
}
