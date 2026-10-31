#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime))]
internal sealed partial class CoreGameEngineDeathGrowthSkillObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> _resolve;
    private CoreGameEngineDeathGrowthSkillObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeathGrowthSkillObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime> resolve) => new(resolve);
}
