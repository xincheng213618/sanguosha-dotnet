#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuLiHost(Func<RuntimeEntries.CharacterLiuLiAnniversaryEntries> getCharacterLiuLiAnniversaryRuntime) : IAnniversaryLiuLiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuLiAnniversaryRuntime().BeginLiuLi(f, op);
}
