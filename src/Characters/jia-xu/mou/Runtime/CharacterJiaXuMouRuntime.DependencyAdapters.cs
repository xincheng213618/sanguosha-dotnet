#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CharacterJiaXuMouCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouJiaXuRuntime))]
internal sealed class CharacterJiaXuMouMouJiaXuAdapter(global::System.Func<global::CardGame.Core.MouJiaXuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouMouJiaXuOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouJiaXuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouJiaXuRuntime Get() => this._targetFactory();
}
