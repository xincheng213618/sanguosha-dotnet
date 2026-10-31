#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterSimaYiMouCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouSimaYiRuntime))]
internal sealed class CharacterSimaYiMouMouSimaYiAdapter(global::System.Func<global::CardGame.Core.MouSimaYiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouMouSimaYiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouSimaYiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouSimaYiRuntime Get() => this._targetFactory();
}
