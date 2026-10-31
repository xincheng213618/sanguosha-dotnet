#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGaoXiangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGaoXiangAnniversaryEntries> getCharacterGaoXiangAnniversaryRuntime) : IAnniversaryGaoXiangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterGaoXiangAnniversaryRuntime().BeginGaoXiang(f);
}
