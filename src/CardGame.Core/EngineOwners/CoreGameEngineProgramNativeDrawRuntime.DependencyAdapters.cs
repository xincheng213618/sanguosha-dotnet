#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeDrawProducerRuntime))]
internal sealed class CoreProgramNativeDrawNativeDrawProducerAdapter(global::System.Func<global::CardGame.Core.NativeDrawProducerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramNativeDrawNativeDrawProducerOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeDrawProducerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawProducerRuntime Get() => this._targetFactory();
}
