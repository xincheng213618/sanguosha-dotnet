#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangAuditCombatRuntime))]
internal sealed partial class CoreGameEngineWushuangAuditCombatEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> _resolve;
    private CoreGameEngineWushuangAuditCombatEntries(Func<EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAuditCombatEntries Create(Func<EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> resolve) => new(resolve);
}
