#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNativeLegalActionsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreNativeLegalActionsCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramActionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramActionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}
