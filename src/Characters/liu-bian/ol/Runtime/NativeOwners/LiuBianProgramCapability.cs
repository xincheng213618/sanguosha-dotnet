#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuBianProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuBianOlRuntime>> getCharacterLiuBianOlRuntime) : ILiuBianProgramHost
{
    public SkillProgramStepOutcome ShiYuanTargetDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuBianOlRuntime().Value.ShiYuanProgramTargetDraw(frame, effect);
    public SkillProgramStepOutcome DuShiGrantSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuBianOlRuntime().Value.DuShiProgramGrantSkill(frame, effect);
    public SkillProgramStepOutcome YuWeiMarkActiveTurn(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuBianOlRuntime().Value.YuWeiProgramMarkActiveTurn(frame, effect);
}
