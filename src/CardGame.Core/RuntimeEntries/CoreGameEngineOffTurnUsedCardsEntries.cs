#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime))]
internal sealed partial class CoreGameEngineOffTurnUsedCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> _resolve;
    private CoreGameEngineOffTurnUsedCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOffTurnUsedCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> resolve) => new(resolve);
}
