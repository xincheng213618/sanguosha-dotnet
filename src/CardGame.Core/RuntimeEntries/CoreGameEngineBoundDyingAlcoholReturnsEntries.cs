#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime))]
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> _resolve;
    private CoreGameEngineBoundDyingAlcoholReturnsEntries(Func<EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundDyingAlcoholReturnsEntries Create(Func<EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> resolve) => new(resolve);
}
