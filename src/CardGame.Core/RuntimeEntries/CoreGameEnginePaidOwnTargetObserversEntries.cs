#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime))]
internal sealed partial class CoreGameEnginePaidOwnTargetObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _resolve;
    private CoreGameEnginePaidOwnTargetObserversEntries(Func<EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidOwnTargetObserversEntries Create(Func<EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> resolve) => new(resolve);
}
