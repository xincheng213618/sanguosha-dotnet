#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhouFangProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhouFangOlRuntime>> getCharacterZhouFangOlRuntime) : IZhouFangProgramHost
{
    public SkillProgramStepOutcome DuanfaDiscardAndDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhouFangOlRuntime().Value.DuanfaProgramDiscardAndDraw(frame, effect);
    public SkillProgramStepOutcome YoudiBaitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhouFangOlRuntime().Value.YoudiProgramBaitDiscard(frame, effect);
}
