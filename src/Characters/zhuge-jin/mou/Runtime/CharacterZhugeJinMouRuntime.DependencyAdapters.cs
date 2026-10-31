#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouZhugeJinRuntime))]
internal sealed class CharacterZhugeJinMouMouZhugeJinAdapter(global::System.Func<global::CardGame.Core.MouZhugeJinRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouMouZhugeJinOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouZhugeJinRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouZhugeJinRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterZhugeJinMouCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
