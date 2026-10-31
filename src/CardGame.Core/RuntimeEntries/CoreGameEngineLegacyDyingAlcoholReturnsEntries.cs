#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime))]
internal sealed partial class CoreGameEngineLegacyDyingAlcoholReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> _resolve;
    private CoreGameEngineLegacyDyingAlcoholReturnsEntries(Func<EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLegacyDyingAlcoholReturnsEntries Create(Func<EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> resolve) => new(resolve);
}
