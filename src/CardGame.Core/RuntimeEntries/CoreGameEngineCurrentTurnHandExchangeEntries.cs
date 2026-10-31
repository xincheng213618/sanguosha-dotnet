#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime))]
internal sealed partial class CoreGameEngineCurrentTurnHandExchangeEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> _resolve;
    private CoreGameEngineCurrentTurnHandExchangeEntries(Func<EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnHandExchangeEntries Create(Func<EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> resolve) => new(resolve);
}
