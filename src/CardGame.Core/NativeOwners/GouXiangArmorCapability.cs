#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GouXiangArmorCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGouXiangArmorEntries> getCoreGameEngineGouXiangArmorRuntime) : IGouXiangArmorHost
{
    public void ObtainAttackerHandCard(ProgramSkillFrame frame) => getCoreGameEngineGouXiangArmorRuntime().ObtainGouXiangAttackerHandCard(frame);
}
