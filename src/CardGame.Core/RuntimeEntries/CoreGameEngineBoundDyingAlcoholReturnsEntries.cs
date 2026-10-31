#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> _resolve;
    private CoreGameEngineBoundDyingAlcoholReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundDyingAlcoholReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> resolve) => new(resolve);
}
