#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangQuanMouCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanMouCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.BoundaryYiJiRuntime))]
internal sealed class CharacterHuangQuanMouBoundaryYiJiAdapter(global::System.Func<global::CardGame.Core.BoundaryYiJiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanMouBoundaryYiJiOperations
{
    private readonly global::System.Func<global::CardGame.Core.BoundaryYiJiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.BoundaryYiJiRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouHuangQuanRuntime))]
internal sealed class CharacterHuangQuanMouMouHuangQuanAdapter(global::System.Func<global::CardGame.Core.MouHuangQuanRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanMouMouHuangQuanOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouHuangQuanRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouHuangQuanRuntime Get() => this._targetFactory();
}
