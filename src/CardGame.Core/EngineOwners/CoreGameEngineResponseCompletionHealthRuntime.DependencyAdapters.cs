#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ResponseCompletionRuntime))]
internal sealed class CoreResponseCompletionHealthResponseCompletionAdapter(global::System.Func<global::CardGame.Core.ResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionHealthResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ResponseCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ResponseCompletionRuntime Get() => this._targetFactory();
}
