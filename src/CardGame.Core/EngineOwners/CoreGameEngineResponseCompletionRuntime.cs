#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseCompletionRuntime
{
    private readonly ICoreResponseCompletionResponseCompletionOperations _responseCompletionOperations;
    internal CoreGameEngineResponseCompletionRuntime(ICoreResponseCompletionResponseCompletionOperations responseCompletionOperations)
    {
        this._responseCompletionOperations = responseCompletionOperations;
    }
}
