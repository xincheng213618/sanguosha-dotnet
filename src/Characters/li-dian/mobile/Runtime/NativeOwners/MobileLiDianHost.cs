#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileLiDianHost(Func<RuntimeEntries.CharacterLiDianMobileEntries> getCharacterLiDianMobileRuntime) : IMobileLiDianWangxiHost
{
    public SkillProgramStepOutcome Draw(ProgramSkillFrame f) => getCharacterLiDianMobileRuntime().BeginMobileLiDianWangxi(f);
}
