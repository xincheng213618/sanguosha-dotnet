#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.AtomicCardMovementRuntime))]
internal sealed class CoreAtomicCardMovementAtomicCardMovementAdapter(global::System.Func<global::CardGame.Core.AtomicCardMovementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementAtomicCardMovementOperations
{
    private readonly global::System.Func<global::CardGame.Core.AtomicCardMovementRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AtomicCardMovementRuntime Get() => this._targetFactory();
}
