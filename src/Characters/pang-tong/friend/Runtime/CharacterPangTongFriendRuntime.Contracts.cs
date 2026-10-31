#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterPangTongFriendCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterPangTongFriendCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetSeatDistance(int sourceSeat, int targetSeat);
}
