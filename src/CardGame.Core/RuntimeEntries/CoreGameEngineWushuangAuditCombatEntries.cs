#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditCombatEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> _resolve;
    private CoreGameEngineWushuangAuditCombatEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAuditCombatEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditCombatRuntime> resolve) => new(resolve);
}
