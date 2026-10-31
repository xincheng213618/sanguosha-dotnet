#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GaoLanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGaoLanOlRuntime>> getCharacterGaoLanOlRuntime) : IGaoLanProgramHost
{
    public void XizhenResponseBenefit(ProgramSkillFrame frame) => getCharacterGaoLanOlRuntime().Value.XizhenProgramResponseBenefit(frame);
}
