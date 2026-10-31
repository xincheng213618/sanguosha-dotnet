#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangWanOlRuntime
{
    internal static int SuitMask(Suit suit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_yang_wan_ol_YangWanPrograms.SuitMask(suit);
    SkillProgramStepOutcome global::CardGame.Core.IYangWanProgramHost.YouyanGainSuitCards(ProgramSkillFrame frame, SkillProgramEffect effect) => YouyanProgramGainSuitCards(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IYangWanProgramHost.ZhuihuanArm(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhuihuanProgramArm(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IYangWanProgramHost.ZhuihuanRetaliate(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhuihuanProgramRetaliate(frame, effect);
}
