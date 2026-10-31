#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseCompletionHealthRuntime
{
    private readonly ICoreResponseCompletionHealthResponseCompletionOperations _responseCompletionOperations;
    internal CoreGameEngineResponseCompletionHealthRuntime(ICoreResponseCompletionHealthResponseCompletionOperations responseCompletionOperations)
    {
        this._responseCompletionOperations = responseCompletionOperations;
    }
}
