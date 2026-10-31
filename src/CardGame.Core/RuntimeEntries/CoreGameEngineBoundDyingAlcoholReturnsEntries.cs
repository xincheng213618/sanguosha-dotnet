#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime))]
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> _resolve;
    private CoreGameEngineBoundDyingAlcoholReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundDyingAlcoholReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> resolve) => new(resolve);
}
