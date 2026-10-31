#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseUseCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _resolve;
    private CoreGameEngineResponseUseCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseUseCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> resolve) => new(resolve);
}
