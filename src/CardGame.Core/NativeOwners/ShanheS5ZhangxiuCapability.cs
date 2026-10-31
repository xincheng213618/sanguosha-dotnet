#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5ZhangxiuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ZhangxiuEntries> getCoreGameEngineShanheS5ZhangxiuRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheLeizhenEntries> getCoreGameEngineShanheLeizhenRuntime) : IShanheS5ZhangxiuHost
{
    public SkillProgramStepOutcome PayShanheS5Xiongluan(ProgramSkillFrame f) => getCoreGameEngineShanheS5ZhangxiuRuntime().PayShanheS5Xiongluan(f);
    public void ResolveShanheLeizhenUse(ProgramSkillFrame f) => getCoreGameEngineShanheLeizhenRuntime().ResolveShanheLeizhenUse(f);
}
