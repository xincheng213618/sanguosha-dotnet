#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryWuFuHost(Func<RuntimeEntries.CharacterWuFuAnniversaryEntries> getCharacterWuFuAnniversaryRuntime) : IAnniversaryWuFuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuFuAnniversaryRuntime().BeginWuFu(f, op);
}
