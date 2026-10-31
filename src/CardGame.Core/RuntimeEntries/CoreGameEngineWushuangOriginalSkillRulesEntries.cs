#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime))]
internal sealed partial class CoreGameEngineWushuangOriginalSkillRulesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillRulesEntries(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillRulesEntries Create(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> resolve) => new(resolve);
}
