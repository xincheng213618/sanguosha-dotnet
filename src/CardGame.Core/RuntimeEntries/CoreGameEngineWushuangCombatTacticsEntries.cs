#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangCombatTacticsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> _resolve;
    private CoreGameEngineWushuangCombatTacticsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangCombatTacticsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> resolve) => new(resolve);
}
