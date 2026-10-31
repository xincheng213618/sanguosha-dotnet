#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.HealthContinuationRuntime))]
internal sealed class CoreTypedRecoveryDamageCursorHealthContinuationAdapter(global::System.Func<global::CardGame.Core.HealthContinuationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTypedRecoveryDamageCursorHealthContinuationOperations
{
    private readonly global::System.Func<global::CardGame.Core.HealthContinuationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HealthContinuationRuntime Get() => this._targetFactory();
}
