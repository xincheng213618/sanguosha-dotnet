#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MobileSimaRenjieRuntime))]
internal sealed class CharacterSimaYiMobileGodMobileSimaRenjieAdapter(global::System.Func<global::CardGame.Core.MobileSimaRenjieRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMobileGodMobileSimaRenjieOperations
{
    private readonly global::System.Func<global::CardGame.Core.MobileSimaRenjieRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MobileSimaRenjieRuntime Get() => this._targetFactory();
}
