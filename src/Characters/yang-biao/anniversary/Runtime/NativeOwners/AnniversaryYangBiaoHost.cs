#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYangBiaoHost(Func<RuntimeEntries.CharacterYangBiaoAnniversaryEntries> getCharacterYangBiaoAnniversaryRuntime) : IAnniversaryYangBiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYangBiaoAnniversaryRuntime().BeginYangBiao(f, op);
}
