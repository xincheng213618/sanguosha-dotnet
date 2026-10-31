#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXiahouEnHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouEnAnniversaryEntries> getCharacterXiahouEnAnniversaryRuntime) : IAnniversaryXiahouEnHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiahouEnAnniversaryRuntime().BeginAnniversaryXiahouEn(f, operation);
}
