#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime))]
internal sealed partial class CoreGameEngineActualTurnSkillGrantsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> _resolve;
    private CoreGameEngineActualTurnSkillGrantsEntries(Func<EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnSkillGrantsEntries Create(Func<EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> resolve) => new(resolve);
}
