#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrepPoisonObserversRuntime))]
internal sealed partial class CoreGameEnginePrepPoisonObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepPoisonObserversRuntime> _resolve;
    private CoreGameEnginePrepPoisonObserversEntries(Func<EngineOwners.CoreGameEnginePrepPoisonObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepPoisonObserversEntries Create(Func<EngineOwners.CoreGameEnginePrepPoisonObserversRuntime> resolve) => new(resolve);
}
