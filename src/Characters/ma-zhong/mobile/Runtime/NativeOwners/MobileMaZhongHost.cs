#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaZhongHost(Func<RuntimeEntries.CharacterMaZhongMobileEntries> getCharacterMaZhongMobileRuntime) : IMobileMaZhongHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterMaZhongMobileRuntime().PrepareMobileMaZhongFuman(frame);
}
