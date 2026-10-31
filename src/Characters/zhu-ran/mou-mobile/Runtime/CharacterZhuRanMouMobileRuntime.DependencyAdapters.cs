#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterZhuRanMouMobileCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhuRanMouMobileCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouZhuRanRuntime))]
internal sealed class CharacterZhuRanMouMobileMouZhuRanAdapter(global::System.Func<global::CardGame.Core.MouZhuRanRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhuRanMouMobileMouZhuRanOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouZhuRanRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouZhuRanRuntime Get() => this._targetFactory();
}
