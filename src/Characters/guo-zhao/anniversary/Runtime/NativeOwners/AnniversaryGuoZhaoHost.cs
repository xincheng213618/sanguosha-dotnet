#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGuoZhaoHost(Func<RuntimeEntries.CharacterGuoZhaoAnniversaryEntries> getCharacterGuoZhaoAnniversaryRuntime) : IAnniversaryGuoZhaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoZhaoAnniversaryRuntime().BeginGuoZhao(f, op);
}
