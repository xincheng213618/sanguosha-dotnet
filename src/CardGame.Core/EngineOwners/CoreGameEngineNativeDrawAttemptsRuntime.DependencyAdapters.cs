#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeDrawAttemptRuntime))]
internal sealed class CoreNativeDrawAttemptsNativeDrawAttemptAdapter(global::System.Func<global::CardGame.Core.NativeDrawAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawAttemptsNativeDrawAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeDrawAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawAttemptRuntime Get() => this._targetFactory();
}
