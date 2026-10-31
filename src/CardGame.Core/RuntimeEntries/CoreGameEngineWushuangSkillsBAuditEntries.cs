#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime))]
internal sealed partial class CoreGameEngineWushuangSkillsBAuditEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> _resolve;
    private CoreGameEngineWushuangSkillsBAuditEntries(Func<EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangSkillsBAuditEntries Create(Func<EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> resolve) => new(resolve);
}
