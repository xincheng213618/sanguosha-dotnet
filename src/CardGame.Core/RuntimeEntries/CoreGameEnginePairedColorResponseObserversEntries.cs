#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime))]
internal sealed partial class CoreGameEnginePairedColorResponseObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> _resolve;
    private CoreGameEnginePairedColorResponseObserversEntries(Func<EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedColorResponseObserversEntries Create(Func<EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> resolve) => new(resolve);
}
