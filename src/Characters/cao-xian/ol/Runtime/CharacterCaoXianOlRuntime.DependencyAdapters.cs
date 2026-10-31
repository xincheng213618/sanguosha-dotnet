#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterCaoXianOlCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoXianOlCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CaoXianRuntime))]
internal sealed class CharacterCaoXianOlCaoXianAdapter(global::System.Func<global::CardGame.Core.CaoXianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoXianOlCaoXianOperations
{
    private readonly global::System.Func<global::CardGame.Core.CaoXianRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CaoXianRuntime Get() => this._targetFactory();
}
