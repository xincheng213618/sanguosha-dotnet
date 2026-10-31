#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime))]
internal sealed partial class CoreGameEngineOffTurnUsedCardObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> _resolve;
    private CoreGameEngineOffTurnUsedCardObserversEntries(Func<EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOffTurnUsedCardObserversEntries Create(Func<EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> resolve) => new(resolve);
}
