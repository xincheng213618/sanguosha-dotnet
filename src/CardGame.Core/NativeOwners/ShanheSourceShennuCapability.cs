#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceShennuCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceShennuEntries> getCoreGameEngineShanheSourceShennuRuntime) : IShanheSourceShennuHost
{
    public void ReceiveDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceShennuRuntime().ReceiveShanheSourceShennuDamage(frame);
}
