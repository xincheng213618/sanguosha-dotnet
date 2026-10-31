#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXuJingHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuJingAnniversaryEntries> getCharacterXuJingAnniversaryRuntime) : IAnniversaryXuJingHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterXuJingAnniversaryRuntime().BeginAnniversaryXuJing(frame, operation);
}
