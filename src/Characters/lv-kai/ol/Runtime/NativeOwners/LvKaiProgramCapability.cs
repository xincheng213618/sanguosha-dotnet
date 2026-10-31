#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvKaiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLvKaiOlRuntime>> getCharacterLvKaiOlRuntime) : ILvKaiProgramHost
{
    public SkillProgramStepOutcome TunanUseRevealedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().Value.TunanProgramUseRevealedCard(frame, effect);
    public SkillProgramStepOutcome BijingMarkHandCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().Value.BijingProgramMarkHandCards(frame, effect);
    public SkillProgramStepOutcome BijingRecastMarkedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().Value.BijingProgramRecastMarkedCards(frame, effect);
    public SkillProgramStepOutcome BijingPunishDiscardPhase(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().Value.BijingProgramPunishDiscardPhase(frame, effect);
}
