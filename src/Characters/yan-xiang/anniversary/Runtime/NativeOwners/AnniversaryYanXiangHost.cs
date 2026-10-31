#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYanXiangHost(Func<RuntimeEntries.CharacterYanXiangAnniversaryEntries> getCharacterYanXiangAnniversaryRuntime) : IAnniversaryYanXiangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanXiangAnniversaryRuntime().BeginYanXiang(f, op);
}
