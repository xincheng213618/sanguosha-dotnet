#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetResolverRegistry ProgramTargetResolvers { get => this._targetFactory().ProgramTargetResolvers; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}
