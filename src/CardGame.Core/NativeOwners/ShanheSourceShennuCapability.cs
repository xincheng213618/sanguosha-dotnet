#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceShennuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceShennuEntries> getCoreGameEngineShanheSourceShennuRuntime) : IShanheSourceShennuHost
{
    public void ReceiveDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceShennuRuntime().ReceiveShanheSourceShennuDamage(frame);
}
