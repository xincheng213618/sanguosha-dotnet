#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime))]
internal sealed partial class CoreGameEngineSharedPhaseSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> _resolve;
    private CoreGameEngineSharedPhaseSkillsEntries(Func<EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSharedPhaseSkillsEntries Create(Func<EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> resolve) => new(resolve);
}
