#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ManualDiscardRuntime))]
internal sealed class CoreDiscardManualDiscardAdapter(global::System.Func<global::CardGame.Core.ManualDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardManualDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ManualDiscardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ManualDiscardRuntime Get() => this._targetFactory();
}
