#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeHealthRecoveryRuntime))]
internal sealed class CoreNativeRecoveryDamageNativeHealthRecoveryAdapter(global::System.Func<global::CardGame.Core.NativeHealthRecoveryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeRecoveryDamageNativeHealthRecoveryOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeHealthRecoveryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeHealthRecoveryRuntime Get() => this._targetFactory();
}
