#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOldHandDispositionRuntime))]
internal sealed partial class CoreGameEngineOldHandDispositionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOldHandDispositionRuntime> _resolve;
    private CoreGameEngineOldHandDispositionEntries(Func<EngineOwners.CoreGameEngineOldHandDispositionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOldHandDispositionEntries Create(Func<EngineOwners.CoreGameEngineOldHandDispositionRuntime> resolve) => new(resolve);
}
