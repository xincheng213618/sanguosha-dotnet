#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GaoLanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGaoLanOlEntries> getCharacterGaoLanOlRuntime) : IGaoLanProgramHost
{
    public void XizhenResponseBenefit(ProgramSkillFrame frame) => getCharacterGaoLanOlRuntime().XizhenProgramResponseBenefit(frame);
}
