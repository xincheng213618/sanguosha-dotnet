#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuLuXunZhangcaiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuXunWumiaoEntries> getCharacterLuXunWumiaoRuntime) : IWuLuXunZhangcaiHost
{
    public SkillProgramStepOutcome Zhangcai(ProgramSkillFrame frame) => getCharacterLuXunWumiaoRuntime().BeginWuLuXunZhangcai(frame);
    public SkillProgramStepOutcome Ruxian(ProgramSkillFrame frame) => getCharacterLuXunWumiaoRuntime().ActivateWuLuXunRuxian(frame);
}
