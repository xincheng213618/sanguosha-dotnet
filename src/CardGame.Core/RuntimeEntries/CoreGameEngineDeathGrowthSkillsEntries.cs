#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime))]
internal sealed partial class CoreGameEngineDeathGrowthSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime> _resolve;
    private CoreGameEngineDeathGrowthSkillsEntries(Func<EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeathGrowthSkillsEntries Create(Func<EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime> resolve) => new(resolve);
}
