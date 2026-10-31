#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime))]
internal sealed class CoreWushuangTacticRulesCoreWushuangAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangTacticRulesCoreWushuangOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IWushuangModeFeature RequireWushuangFeature { get => this._targetFactory().RequireWushuangFeature; }
    public global::CardGame.Core.IWushuangModeFeature? WushuangFeature { get => this._targetFactory().WushuangFeature; }
}
