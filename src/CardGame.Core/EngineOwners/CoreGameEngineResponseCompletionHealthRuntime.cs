#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseCompletionHealthRuntime
{
    private readonly ICoreResponseCompletionHealthResponseCompletionOperations _responseCompletionOperations;
    internal CoreGameEngineResponseCompletionHealthRuntime(ICoreResponseCompletionHealthResponseCompletionOperations responseCompletionOperations)
    {
        this._responseCompletionOperations = responseCompletionOperations;
    }
}
