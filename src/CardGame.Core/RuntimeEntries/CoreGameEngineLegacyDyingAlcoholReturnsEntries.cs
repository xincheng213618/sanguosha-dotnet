#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLegacyDyingAlcoholReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> _resolve;
    private CoreGameEngineLegacyDyingAlcoholReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLegacyDyingAlcoholReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> resolve) => new(resolve);
}
