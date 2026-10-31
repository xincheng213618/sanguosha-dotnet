#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterXuShengMouCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouXuShengRuntime))]
internal sealed class CharacterXuShengMouMouXuShengAdapter(global::System.Func<global::CardGame.Core.MouXuShengRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouMouXuShengOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouXuShengRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouXuShengRuntime Get() => this._targetFactory();
}
