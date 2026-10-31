#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouKongRongRuntime))]
internal sealed class CharacterKongRongMouOlMouKongRongAdapter(global::System.Func<global::CardGame.Core.MouKongRongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKongRongMouOlMouKongRongOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouKongRongRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouKongRongRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterKongRongMouOlCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKongRongMouOlCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
