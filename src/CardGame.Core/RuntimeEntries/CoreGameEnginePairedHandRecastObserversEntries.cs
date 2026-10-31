#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime))]
internal sealed partial class CoreGameEnginePairedHandRecastObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime> _resolve;
    private CoreGameEnginePairedHandRecastObserversEntries(Func<EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedHandRecastObserversEntries Create(Func<EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime> resolve) => new(resolve);
}
