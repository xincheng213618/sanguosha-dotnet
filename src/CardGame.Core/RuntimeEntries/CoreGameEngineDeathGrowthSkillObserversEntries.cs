#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime))]
internal sealed partial class CoreGameEngineDeathGrowthSkillObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> _resolve;
    private CoreGameEngineDeathGrowthSkillObserversEntries(Func<EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeathGrowthSkillObserversEntries Create(Func<EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> resolve) => new(resolve);
}
