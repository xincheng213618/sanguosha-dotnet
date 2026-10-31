#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GaoLanProgramCapability(Func<RuntimeEntries.CharacterGaoLanOlEntries> getCharacterGaoLanOlRuntime) : IGaoLanProgramHost
{
    public void XizhenResponseBenefit(ProgramSkillFrame frame) => getCharacterGaoLanOlRuntime().XizhenProgramResponseBenefit(frame);
}
