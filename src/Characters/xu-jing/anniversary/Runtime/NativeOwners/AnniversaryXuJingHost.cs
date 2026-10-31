#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXuJingHost(Func<RuntimeEntries.CharacterXuJingAnniversaryEntries> getCharacterXuJingAnniversaryRuntime) : IAnniversaryXuJingHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterXuJingAnniversaryRuntime().BeginAnniversaryXuJing(frame, operation);
}
