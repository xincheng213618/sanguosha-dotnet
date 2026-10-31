#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhouFangProgramCapability(Func<RuntimeEntries.CharacterZhouFangOlEntries> getCharacterZhouFangOlRuntime) : IZhouFangProgramHost
{
    public SkillProgramStepOutcome DuanfaDiscardAndDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhouFangOlRuntime().DuanfaProgramDiscardAndDraw(frame, effect);
    public SkillProgramStepOutcome YoudiBaitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhouFangOlRuntime().YoudiProgramBaitDiscard(frame, effect);
}
