#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouGuoJiaRuntime))]
internal sealed class CharacterGuoJiaMouMouGuoJiaAdapter(global::System.Func<global::CardGame.Core.MouGuoJiaRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouMouGuoJiaOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouGuoJiaRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouGuoJiaRuntime Get() => this._targetFactory();
}
