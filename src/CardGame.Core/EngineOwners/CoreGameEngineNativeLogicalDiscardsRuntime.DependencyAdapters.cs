#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeLogicalDiscardRuntime))]
internal sealed class CoreNativeLogicalDiscardsNativeLogicalDiscardAdapter(global::System.Func<global::CardGame.Core.NativeLogicalDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLogicalDiscardsNativeLogicalDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeLogicalDiscardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeLogicalDiscardRuntime Get() => this._targetFactory();
}
