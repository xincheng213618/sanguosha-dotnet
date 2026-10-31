#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime))]
internal sealed partial class CoreGameEnginePlayPhaseSkillGrantsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> _resolve;
    private CoreGameEnginePlayPhaseSkillGrantsEntries(Func<EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlayPhaseSkillGrantsEntries Create(Func<EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> resolve) => new(resolve);
}
