#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _resolve;
    private CoreGameEngineResponseCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> resolve) => new(resolve);
}
