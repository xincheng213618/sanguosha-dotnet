#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CharacterJuShouMouCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouJuShouRuntime))]
internal sealed class CharacterJuShouMouMouJuShouAdapter(global::System.Func<global::CardGame.Core.MouJuShouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouMouJuShouOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouJuShouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouJuShouRuntime Get() => this._targetFactory();
}
