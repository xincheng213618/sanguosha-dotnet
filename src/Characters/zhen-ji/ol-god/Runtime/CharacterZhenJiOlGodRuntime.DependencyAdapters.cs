#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.OlShenZhenJiRuntime))]
internal sealed class CharacterZhenJiOlGodOlShenZhenJiAdapter(global::System.Func<global::CardGame.Core.OlShenZhenJiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhenJiOlGodOlShenZhenJiOperations
{
    private readonly global::System.Func<global::CardGame.Core.OlShenZhenJiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.OlShenZhenJiRuntime Get() => this._targetFactory();
}
