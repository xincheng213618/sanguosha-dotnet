#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime))]
internal sealed partial class CoreGameEngineWushuangOriginalSkillFlowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillFlowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillFlowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> resolve) => new(resolve);
}
