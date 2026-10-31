#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MoSimaYiRuntime))]
internal sealed class CharacterSimaYiOlMagicMoSimaYiAdapter(global::System.Func<global::CardGame.Core.MoSimaYiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicMoSimaYiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MoSimaYiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MoSimaYiRuntime Get() => this._targetFactory();
}
