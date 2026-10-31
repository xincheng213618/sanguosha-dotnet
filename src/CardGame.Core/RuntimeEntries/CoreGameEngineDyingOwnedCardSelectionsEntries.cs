#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingOwnedCardSelectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> _resolve;
    private CoreGameEngineDyingOwnedCardSelectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingOwnedCardSelectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> resolve) => new(resolve);
}
