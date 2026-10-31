#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversarySunHuanHost(Func<RuntimeEntries.CharacterSunHuanAnniversaryEntries> getCharacterSunHuanAnniversaryRuntime) : IAnniversarySunHuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunHuanAnniversaryRuntime().BeginSunHuan(f, op);
}
