#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GouXiangArmorCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineGouXiangArmorRuntime>> getCoreGameEngineGouXiangArmorRuntime) : IGouXiangArmorHost
{
    public void ObtainAttackerHandCard(ProgramSkillFrame frame) => getCoreGameEngineGouXiangArmorRuntime().Value.ObtainGouXiangAttackerHandCard(frame);
}
