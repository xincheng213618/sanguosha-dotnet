#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangSkillsBRuntime))]
internal sealed partial class CoreGameEngineWushuangSkillsBEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangSkillsBRuntime> _resolve;
    private CoreGameEngineWushuangSkillsBEntries(Func<EngineOwners.CoreGameEngineWushuangSkillsBRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangSkillsBEntries Create(Func<EngineOwners.CoreGameEngineWushuangSkillsBRuntime> resolve) => new(resolve);
}
