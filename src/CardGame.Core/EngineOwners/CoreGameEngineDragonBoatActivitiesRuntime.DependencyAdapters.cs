#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActivityBattleRulesRuntime))]
internal sealed class CoreDragonBoatActivitiesCoreActivityBattleRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityBattleRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDragonBoatActivitiesCoreActivityBattleRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityBattleRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IDragonBoatBattleRulesFeature? DragonBoatRules { get => this._targetFactory().DragonBoatRules; }
}
