#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvKaiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLvKaiOlEntries> getCharacterLvKaiOlRuntime) : ILvKaiProgramHost
{
    public SkillProgramStepOutcome TunanUseRevealedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().TunanProgramUseRevealedCard(frame, effect);
    public SkillProgramStepOutcome BijingMarkHandCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().BijingProgramMarkHandCards(frame, effect);
    public SkillProgramStepOutcome BijingRecastMarkedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().BijingProgramRecastMarkedCards(frame, effect);
    public SkillProgramStepOutcome BijingPunishDiscardPhase(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLvKaiOlRuntime().BijingProgramPunishDiscardPhase(frame, effect);
}
