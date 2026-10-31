#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHouChengHost(Func<RuntimeEntries.CharacterHouChengAnniversaryEntries> getCharacterHouChengAnniversaryRuntime) : IAnniversaryHouChengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterHouChengAnniversaryRuntime().BeginHouCheng(f);
}
