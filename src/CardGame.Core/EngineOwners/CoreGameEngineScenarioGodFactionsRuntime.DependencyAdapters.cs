#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime))]
internal sealed class CoreScenarioGodFactionsCoreModeRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreScenarioGodFactionsCoreModeRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IMatchModeFactionPolicy ModeFactionPolicy { get => this._targetFactory().ModeFactionPolicy; }
}
