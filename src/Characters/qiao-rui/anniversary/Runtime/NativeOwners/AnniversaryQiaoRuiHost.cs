#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryQiaoRuiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQiaoRuiAnniversaryEntries> getCharacterQiaoRuiAnniversaryRuntime) : IAnniversaryQiaoRuiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterQiaoRuiAnniversaryRuntime().BeginQiaoRui(f, op);
}
