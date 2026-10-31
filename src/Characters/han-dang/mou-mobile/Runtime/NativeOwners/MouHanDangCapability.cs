#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouHanDangCapability(Func<RuntimeEntries.CharacterHanDangMouMobileEntries> getCharacterHanDangMouMobileRuntime) : IMouHanDangHost
{
    public SkillProgramStepOutcome Gongqi(ProgramSkillFrame f) => getCharacterHanDangMouMobileRuntime().StartMouHanDang(f, true);
    public SkillProgramStepOutcome Jiefan(ProgramSkillFrame f) => getCharacterHanDangMouMobileRuntime().StartMouHanDang(f, false);
}
