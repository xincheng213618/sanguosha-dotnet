#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CoreNativeTargetLegalityCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTargetLegalityCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime))]
internal sealed class CoreNativeTargetLegalityCoreProgramDistanceRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTargetLegalityCoreProgramDistanceRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}
