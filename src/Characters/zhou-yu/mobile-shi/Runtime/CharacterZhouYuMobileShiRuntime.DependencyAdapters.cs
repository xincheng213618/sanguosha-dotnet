#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ShiZhouYuRuntime))]
internal sealed class CharacterZhouYuMobileShiShiZhouYuAdapter(global::System.Func<global::CardGame.Core.ShiZhouYuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMobileShiShiZhouYuOperations
{
    private readonly global::System.Func<global::CardGame.Core.ShiZhouYuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ShiZhouYuRuntime Get() => this._targetFactory();
}
