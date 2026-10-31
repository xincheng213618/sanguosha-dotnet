#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryWangChangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangChangAnniversaryEntries> getCharacterWangChangAnniversaryRuntime) : IAnniversaryWangChangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWangChangAnniversaryRuntime().BeginWangChang(f, op);
}
