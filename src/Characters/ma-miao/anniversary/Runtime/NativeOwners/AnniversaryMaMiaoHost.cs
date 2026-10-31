#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryMaMiaoHost(Func<RuntimeEntries.CharacterMaMiaoAnniversaryEntries> getCharacterMaMiaoAnniversaryRuntime) : IAnniversaryMaMiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaMiaoAnniversaryRuntime().BeginMaMiao(f, op);
}
