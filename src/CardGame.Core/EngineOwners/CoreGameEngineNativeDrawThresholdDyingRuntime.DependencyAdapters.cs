#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeDrawThresholdRuntime))]
internal sealed class CoreNativeDrawThresholdDyingNativeDrawThresholdAdapter(global::System.Func<global::CardGame.Core.NativeDrawThresholdRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawThresholdDyingNativeDrawThresholdOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawThresholdRuntime Get() => this._targetFactory();
}
