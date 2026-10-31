#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryDuKuiCapability(Func<RuntimeEntries.CharacterDuKuiAnniversaryEntries> getCharacterDuKuiAnniversaryRuntime) : IAnniversaryDuKuiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDuKuiAnniversaryRuntime().BeginDuKui(f, op);
}
