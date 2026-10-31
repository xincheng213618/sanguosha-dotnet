#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathGrowthSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime> _resolve;
    private CoreGameEngineDeathGrowthSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeathGrowthSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime> resolve) => new(resolve);
}
