#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangTiaoxinExpansionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangAuditAEntries> getCoreGameEngineWushuangAuditARuntime) : IWushuangTiaoxinExpansionHost
{
    public void Record(ProgramSkillFrame frame) => getCoreGameEngineWushuangAuditARuntime().RecordWushuangTiaoxinExpansion(frame);
}
