#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime))]
internal sealed class CoreRecoveryDamageEdgesCoreNativeRecoveryDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryDamageEdgesCoreNativeRecoveryDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeHealthRecoveryRuntime NativeHealthRecovery { get => this._targetFactory().NativeHealthRecovery; }
}
