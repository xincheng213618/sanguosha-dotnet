#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangFenHost(Func<RuntimeEntries.CharacterZhangFenAnniversaryEntries> getCharacterZhangFenAnniversaryRuntime) : IAnniversaryZhangFenHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangFenAnniversaryRuntime().BeginZhangFen(f, op);
}
