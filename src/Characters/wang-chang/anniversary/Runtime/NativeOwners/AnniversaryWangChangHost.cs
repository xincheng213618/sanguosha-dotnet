#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryWangChangHost(Func<RuntimeEntries.CharacterWangChangAnniversaryEntries> getCharacterWangChangAnniversaryRuntime) : IAnniversaryWangChangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWangChangAnniversaryRuntime().BeginWangChang(f, op);
}
