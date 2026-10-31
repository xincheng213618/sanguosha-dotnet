#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKaiOlRuntime
{
    SkillProgramStepOutcome ILvKaiProgramHost.TunanUseRevealedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => TunanProgramUseRevealedCard(frame, effect);
    SkillProgramStepOutcome ILvKaiProgramHost.BijingMarkHandCards(ProgramSkillFrame frame, SkillProgramEffect effect) => BijingProgramMarkHandCards(frame, effect);
    SkillProgramStepOutcome ILvKaiProgramHost.BijingRecastMarkedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => BijingProgramRecastMarkedCards(frame, effect);
    SkillProgramStepOutcome ILvKaiProgramHost.BijingPunishDiscardPhase(ProgramSkillFrame frame, SkillProgramEffect effect) => BijingProgramPunishDiscardPhase(frame, effect);
}
