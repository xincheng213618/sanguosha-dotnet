#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime))]
internal sealed partial class CoreGameEngineWushuangCombatTacticsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> _resolve;
    private CoreGameEngineWushuangCombatTacticsEntries(Func<EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangCombatTacticsEntries Create(Func<EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> resolve) => new(resolve);
}
