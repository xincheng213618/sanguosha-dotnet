#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime))]
internal sealed partial class CoreGameEngineNextActualTurnStartSkillGrantsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime> _resolve;
    private CoreGameEngineNextActualTurnStartSkillGrantsEntries(Func<EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextActualTurnStartSkillGrantsEntries Create(Func<EngineOwners.CoreGameEngineNextActualTurnStartSkillGrantsRuntime> resolve) => new(resolve);
}
