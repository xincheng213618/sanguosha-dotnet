#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternatingSuitAndCompletedTopEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _resolve;
    private CoreGameEngineAlternatingSuitAndCompletedTopEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternatingSuitAndCompletedTopEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> resolve) => new(resolve);
}
