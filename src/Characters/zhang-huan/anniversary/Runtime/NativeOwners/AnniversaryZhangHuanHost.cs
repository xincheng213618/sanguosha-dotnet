#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangHuanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangHuanAnniversaryEntries> getCharacterZhangHuanAnniversaryRuntime) : IAnniversaryZhangHuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhangHuanAnniversaryRuntime().BeginZhangHuan(f);
}
