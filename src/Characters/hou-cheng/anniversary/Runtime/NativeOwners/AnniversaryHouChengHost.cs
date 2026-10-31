#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHouChengHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHouChengAnniversaryEntries> getCharacterHouChengAnniversaryRuntime) : IAnniversaryHouChengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterHouChengAnniversaryRuntime().BeginHouCheng(f);
}
