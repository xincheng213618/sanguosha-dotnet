#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangOlRuntime
{
    SkillProgramStepOutcome IZhouFangProgramHost.DuanfaDiscardAndDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => DuanfaProgramDiscardAndDraw(frame, effect);
    SkillProgramStepOutcome IZhouFangProgramHost.YoudiBaitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => YoudiProgramBaitDiscard(frame, effect);
}
