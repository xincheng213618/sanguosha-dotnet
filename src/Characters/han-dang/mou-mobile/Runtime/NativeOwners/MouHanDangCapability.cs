#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouHanDangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHanDangMouMobileEntries> getCharacterHanDangMouMobileRuntime) : IMouHanDangHost
{
    public SkillProgramStepOutcome Gongqi(ProgramSkillFrame f) => getCharacterHanDangMouMobileRuntime().StartMouHanDang(f, true);
    public SkillProgramStepOutcome Jiefan(ProgramSkillFrame f) => getCharacterHanDangMouMobileRuntime().StartMouHanDang(f, false);
}
