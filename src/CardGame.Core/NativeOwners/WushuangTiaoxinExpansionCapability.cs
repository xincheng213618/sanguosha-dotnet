#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangTiaoxinExpansionCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineWushuangAuditARuntime>> getCoreGameEngineWushuangAuditARuntime) : IWushuangTiaoxinExpansionHost
{
    public void Record(ProgramSkillFrame frame) => getCoreGameEngineWushuangAuditARuntime().Value.RecordWushuangTiaoxinExpansion(frame);
}
