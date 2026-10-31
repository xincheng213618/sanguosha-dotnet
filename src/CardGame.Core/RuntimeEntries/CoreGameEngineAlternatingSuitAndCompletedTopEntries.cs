#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed partial class CoreGameEngineAlternatingSuitAndCompletedTopEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _resolve;
    private CoreGameEngineAlternatingSuitAndCompletedTopEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternatingSuitAndCompletedTopEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> resolve) => new(resolve);
}
