#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhangFenHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries> getCharacterZhangFenAnniversaryRuntime) : IAnniversaryZhangFenHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangFenAnniversaryRuntime().BeginZhangFen(f, op);
}
