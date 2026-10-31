#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangWanOlRuntime
{
    internal static int SuitMask(Suit suit) => RuntimeSkills.RuntimeRules_yang_wan_ol_YangWanPrograms.SuitMask(suit);
    SkillProgramStepOutcome IYangWanProgramHost.YouyanGainSuitCards(ProgramSkillFrame frame, SkillProgramEffect effect) => YouyanProgramGainSuitCards(frame, effect);
    SkillProgramStepOutcome IYangWanProgramHost.ZhuihuanArm(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhuihuanProgramArm(frame, effect);
    SkillProgramStepOutcome IYangWanProgramHost.ZhuihuanRetaliate(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhuihuanProgramRetaliate(frame, effect);
}
