#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouEnHost(Func<RuntimeEntries.CharacterXiahouEnAnniversaryEntries> getCharacterXiahouEnAnniversaryRuntime) : IAnniversaryXiahouEnHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiahouEnAnniversaryRuntime().BeginAnniversaryXiahouEn(f, operation);
}
