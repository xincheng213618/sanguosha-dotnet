#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuBianProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuBianOlEntries> getCharacterLiuBianOlRuntime) : ILiuBianProgramHost
{
    public SkillProgramStepOutcome ShiYuanTargetDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuBianOlRuntime().ShiYuanProgramTargetDraw(frame, effect);
    public SkillProgramStepOutcome DuShiGrantSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuBianOlRuntime().DuShiProgramGrantSkill(frame, effect);
    public SkillProgramStepOutcome YuWeiMarkActiveTurn(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuBianOlRuntime().YuWeiProgramMarkActiveTurn(frame, effect);
}
