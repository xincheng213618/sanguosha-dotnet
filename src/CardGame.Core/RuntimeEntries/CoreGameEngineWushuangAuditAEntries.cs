#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditAEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangAuditARuntime> _resolve;
    private CoreGameEngineWushuangAuditAEntries(Func<EngineOwners.CoreGameEngineWushuangAuditARuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangAuditAEntries Create(Func<EngineOwners.CoreGameEngineWushuangAuditARuntime> resolve) => new(resolve);
}
