#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime))]
internal sealed partial class CoreGameEngineWushuangOriginalSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillsEntries(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillsEntries Create(Func<EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> resolve) => new(resolve);
}
