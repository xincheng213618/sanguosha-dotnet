#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaZhongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaZhongMobileEntries> getCharacterMaZhongMobileRuntime) : IMobileMaZhongHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterMaZhongMobileRuntime().PrepareMobileMaZhongFuman(frame);
}
