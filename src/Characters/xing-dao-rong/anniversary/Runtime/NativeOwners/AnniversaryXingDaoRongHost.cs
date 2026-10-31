#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXingDaoRongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXingDaoRongAnniversaryEntries> getCharacterXingDaoRongAnniversaryRuntime) : IAnniversaryXingDaoRongHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterXingDaoRongAnniversaryRuntime().BeginXingDaoRong(f);
}
