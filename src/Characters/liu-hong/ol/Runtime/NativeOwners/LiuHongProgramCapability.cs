#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuHongProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuHongOlEntries> getCharacterLiuHongOlRuntime) : ILiuHongProgramHost
{
    public SkillProgramStepOutcome YujueResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuHongOlRuntime().YujueProgramResolve(frame, effect);
    public SkillProgramStepOutcome ZhihuExpire(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuHongOlRuntime().ZhihuProgramExpire(frame, effect);
    public SkillProgramStepOutcome TuxingArmGameDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLiuHongOlRuntime().TuxingProgramArmGameDamage(frame, effect);
}
