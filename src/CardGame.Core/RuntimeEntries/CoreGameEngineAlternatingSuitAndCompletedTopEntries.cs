#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed partial class CoreGameEngineAlternatingSuitAndCompletedTopEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _resolve;
    private CoreGameEngineAlternatingSuitAndCompletedTopEntries(Func<EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternatingSuitAndCompletedTopEntries Create(Func<EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> resolve) => new(resolve);
}
