#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileLiDianHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiDianMobileEntries> getCharacterLiDianMobileRuntime) : IMobileLiDianWangxiHost
{
    public SkillProgramStepOutcome Draw(ProgramSkillFrame f) => getCharacterLiDianMobileRuntime().BeginMobileLiDianWangxi(f);
}
