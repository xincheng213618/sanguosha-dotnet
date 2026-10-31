#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed partial class CoreGameEngineAdvancedWeaponsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _resolve;
    private CoreGameEngineAdvancedWeaponsEntries(Func<EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedWeaponsEntries Create(Func<EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> resolve) => new(resolve);
}
