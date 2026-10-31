#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime))]
internal sealed partial class CoreGameEngineOffTurnUsedCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> _resolve;
    private CoreGameEngineOffTurnUsedCardsEntries(Func<EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOffTurnUsedCardsEntries Create(Func<EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> resolve) => new(resolve);
}
