#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterYangFuOlCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterYangFuOlCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.YangFuRuntime))]
internal sealed class CharacterYangFuOlYangFuAdapter(global::System.Func<global::CardGame.Core.YangFuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterYangFuOlYangFuOperations
{
    private readonly global::System.Func<global::CardGame.Core.YangFuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.YangFuRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterYangFuOlCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterYangFuOlCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
