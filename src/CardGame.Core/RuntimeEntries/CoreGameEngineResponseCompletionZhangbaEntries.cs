#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionZhangbaEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> _resolve;
    private CoreGameEngineResponseCompletionZhangbaEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseCompletionZhangbaEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> resolve) => new(resolve);
}
