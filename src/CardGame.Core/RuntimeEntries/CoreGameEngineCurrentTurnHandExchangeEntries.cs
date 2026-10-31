#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnHandExchangeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> _resolve;
    private CoreGameEngineCurrentTurnHandExchangeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnHandExchangeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> resolve) => new(resolve);
}
