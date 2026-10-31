#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandExchangesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandExchangesRuntime> _resolve;
    private CoreGameEngineProgramHandExchangesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandExchangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHandExchangesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandExchangesRuntime> resolve) => new(resolve);
}
