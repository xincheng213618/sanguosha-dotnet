#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheGuideSkillsRuntime))]
internal sealed partial class CoreGameEngineShanheGuideSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> _resolve;
    private CoreGameEngineShanheGuideSkillsEntries(Func<EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheGuideSkillsEntries Create(Func<EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> resolve) => new(resolve);
}
