#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterPangTongFriendCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterPangTongFriendCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongFriendCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetSeatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetSeatDistance(sourceSeat, targetSeat);
}
