#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangGaiMouCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangGaiMouCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouHuangGaiRuntime))]
internal sealed class CharacterHuangGaiMouMouHuangGaiAdapter(global::System.Func<global::CardGame.Core.MouHuangGaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangGaiMouMouHuangGaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouHuangGaiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouHuangGaiRuntime Get() => this._targetFactory();
}
