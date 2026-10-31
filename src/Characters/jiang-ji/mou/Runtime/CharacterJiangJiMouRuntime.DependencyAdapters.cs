#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouJiangJiRuntime))]
internal sealed class CharacterJiangJiMouMouJiangJiAdapter(global::System.Func<global::CardGame.Core.MouJiangJiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterJiangJiMouMouJiangJiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouJiangJiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouJiangJiRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CharacterJiangJiMouCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterJiangJiMouCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
