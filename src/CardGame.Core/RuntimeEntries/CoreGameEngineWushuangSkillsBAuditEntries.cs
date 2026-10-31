#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangSkillsBAuditEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> _resolve;
    private CoreGameEngineWushuangSkillsBAuditEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangSkillsBAuditEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> resolve) => new(resolve);
}
