#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IZhouFangProgramHost.DuanfaDiscardAndDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => DuanfaProgramDiscardAndDraw(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IZhouFangProgramHost.YoudiBaitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => YoudiProgramBaitDiscard(frame, effect);
}
