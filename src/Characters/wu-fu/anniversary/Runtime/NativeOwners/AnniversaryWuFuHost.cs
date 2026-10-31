#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryWuFuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuFuAnniversaryEntries> getCharacterWuFuAnniversaryRuntime) : IAnniversaryWuFuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuFuAnniversaryRuntime().BeginWuFu(f, op);
}
