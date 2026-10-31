#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseEntityExchangeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _resolve;
    private CoreGameEngineResponseEntityExchangeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseEntityExchangeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> resolve) => new(resolve);
}
