#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime))]
internal sealed partial class CoreGameEngineEndRefillNextTurnObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime> _resolve;
    private CoreGameEngineEndRefillNextTurnObserversEntries(Func<EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndRefillNextTurnObserversEntries Create(Func<EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime> resolve) => new(resolve);
}
