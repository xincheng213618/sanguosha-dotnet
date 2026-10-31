#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryDuKuiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuKuiAnniversaryEntries> getCharacterDuKuiAnniversaryRuntime) : IAnniversaryDuKuiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDuKuiAnniversaryRuntime().BeginDuKui(f, op);
}
