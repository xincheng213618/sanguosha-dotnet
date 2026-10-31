#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouZhouYuRuntime))]
internal sealed class CharacterZhouYuMouMouZhouYuAdapter(global::System.Func<global::CardGame.Core.MouZhouYuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouMouZhouYuOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouZhouYuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouZhouYuRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CharacterZhouYuMouCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
