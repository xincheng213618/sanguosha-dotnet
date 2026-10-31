#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTengFangLanOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ITengFangLanProgramHost.LuochongResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => LuochongProgramResolve(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ITengFangLanProgramHost.AichenRemoveOption(ProgramSkillFrame frame, SkillProgramEffect effect) => AichenProgramRemoveOption(frame, effect);
}
