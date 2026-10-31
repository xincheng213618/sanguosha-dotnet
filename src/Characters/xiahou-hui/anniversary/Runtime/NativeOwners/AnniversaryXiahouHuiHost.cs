#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouHuiHost(Func<RuntimeEntries.CharacterXiahouHuiAnniversaryEntries> getCharacterXiahouHuiAnniversaryRuntime) : IAnniversaryXiahouHuiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiahouHuiAnniversaryRuntime().BeginXiahouHui(f, operation);
}
