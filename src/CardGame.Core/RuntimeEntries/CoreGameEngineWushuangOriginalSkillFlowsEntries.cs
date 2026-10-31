#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime))]
internal sealed partial class CoreGameEngineWushuangOriginalSkillFlowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillFlowsEntries(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillFlowsEntries Create(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> resolve) => new(resolve);
}
