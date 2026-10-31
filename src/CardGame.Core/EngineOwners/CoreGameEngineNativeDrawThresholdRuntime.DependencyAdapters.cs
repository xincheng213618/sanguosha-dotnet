#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeDrawThresholdRuntime))]
internal sealed class CoreNativeDrawThresholdNativeDrawThresholdAdapter(global::System.Func<global::CardGame.Core.NativeDrawThresholdRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawThresholdNativeDrawThresholdOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawThresholdRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeDrawThresholdInvoiceRuntime))]
internal sealed class CoreNativeDrawThresholdNativeDrawThresholdInvoiceAdapter(global::System.Func<global::CardGame.Core.NativeDrawThresholdInvoiceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawThresholdNativeDrawThresholdInvoiceOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeDrawThresholdInvoiceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeDrawThresholdInvoiceRuntime Get() => this._targetFactory();
}
