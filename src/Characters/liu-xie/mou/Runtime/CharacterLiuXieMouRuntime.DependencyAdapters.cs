#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouLiuXieRuntime))]
internal sealed class CharacterLiuXieMouMouLiuXieAdapter(global::System.Func<global::CardGame.Core.MouLiuXieRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterLiuXieMouMouLiuXieOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouLiuXieRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouLiuXieRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterLiuXieMouCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterLiuXieMouCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
