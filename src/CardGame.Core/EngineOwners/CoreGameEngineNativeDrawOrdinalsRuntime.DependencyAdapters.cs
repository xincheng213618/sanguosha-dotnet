#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeDrawOrdinalTriggerRuntime))]
internal sealed class CoreNativeDrawOrdinalsNativeDrawOrdinalTriggerAdapter(global::System.Func<global::CardGame.Core.NativeDrawOrdinalTriggerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawOrdinalsNativeDrawOrdinalTriggerOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeDrawOrdinalTriggerRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawOrdinalTriggerRuntime Get() => this._targetFactory();
}
