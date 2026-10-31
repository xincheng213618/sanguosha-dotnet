#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GouXiangArmorCapability(Func<RuntimeEntries.CoreGameEngineGouXiangArmorEntries> getCoreGameEngineGouXiangArmorRuntime) : IGouXiangArmorHost
{
    public void ObtainAttackerHandCard(ProgramSkillFrame frame) => getCoreGameEngineGouXiangArmorRuntime().ObtainGouXiangAttackerHandCard(frame);
}
