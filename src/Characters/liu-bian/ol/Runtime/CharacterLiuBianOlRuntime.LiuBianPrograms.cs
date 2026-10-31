#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBianOlRuntime
{
    SkillProgramStepOutcome ILiuBianProgramHost.ShiYuanTargetDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => ShiYuanProgramTargetDraw(frame, effect);
    SkillProgramStepOutcome ILiuBianProgramHost.DuShiGrantSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => DuShiProgramGrantSkill(frame, effect);
    SkillProgramStepOutcome ILiuBianProgramHost.YuWeiMarkActiveTurn(ProgramSkillFrame frame, SkillProgramEffect effect) => YuWeiProgramMarkActiveTurn(frame, effect);
}
