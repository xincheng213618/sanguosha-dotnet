#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTengFangLanOlRuntime
{
    SkillProgramStepOutcome ITengFangLanProgramHost.LuochongResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => LuochongProgramResolve(frame, effect);
    SkillProgramStepOutcome ITengFangLanProgramHost.AichenRemoveOption(ProgramSkillFrame frame, SkillProgramEffect effect) => AichenProgramRemoveOption(frame, effect);
}
