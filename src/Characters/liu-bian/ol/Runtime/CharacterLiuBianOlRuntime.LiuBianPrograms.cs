#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBianOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ILiuBianProgramHost.ShiYuanTargetDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => ShiYuanProgramTargetDraw(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ILiuBianProgramHost.DuShiGrantSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => DuShiProgramGrantSkill(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ILiuBianProgramHost.YuWeiMarkActiveTurn(ProgramSkillFrame frame, SkillProgramEffect effect) => YuWeiProgramMarkActiveTurn(frame, effect);
}
