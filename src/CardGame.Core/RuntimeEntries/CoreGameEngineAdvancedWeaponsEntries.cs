#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed partial class CoreGameEngineAdvancedWeaponsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _resolve;
    private CoreGameEngineAdvancedWeaponsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedWeaponsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> resolve) => new(resolve);
}
