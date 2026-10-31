#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryQiaoRuiHost(Func<RuntimeEntries.CharacterQiaoRuiAnniversaryEntries> getCharacterQiaoRuiAnniversaryRuntime) : IAnniversaryQiaoRuiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterQiaoRuiAnniversaryRuntime().BeginQiaoRui(f, op);
}
