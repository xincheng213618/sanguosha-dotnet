#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CharacterHeQiOlCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHeQiOlCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IProgramSelectionQueries ProgramSelectionQueries { get => this._targetFactory().ProgramSelectionQueries; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterHeQiOlCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHeQiOlCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterHeQiOlCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHeQiOlCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
