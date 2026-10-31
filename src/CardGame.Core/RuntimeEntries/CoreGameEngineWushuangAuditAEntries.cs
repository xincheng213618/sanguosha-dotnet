#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditAEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditARuntime> _resolve;
    private CoreGameEngineWushuangAuditAEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditARuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAuditAEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAuditARuntime> resolve) => new(resolve);
}
