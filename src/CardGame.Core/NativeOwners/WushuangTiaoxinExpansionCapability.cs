#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangTiaoxinExpansionCapability(Func<RuntimeEntries.CoreGameEngineWushuangAuditAEntries> getCoreGameEngineWushuangAuditARuntime) : IWushuangTiaoxinExpansionHost
{
    public void Record(ProgramSkillFrame frame) => getCoreGameEngineWushuangAuditARuntime().RecordWushuangTiaoxinExpansion(frame);
}
