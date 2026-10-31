#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5ZhangxiuCapability(Func<RuntimeEntries.CoreGameEngineShanheS5ZhangxiuEntries> getCoreGameEngineShanheS5ZhangxiuRuntime, Func<RuntimeEntries.CoreGameEngineShanheLeizhenEntries> getCoreGameEngineShanheLeizhenRuntime) : IShanheS5ZhangxiuHost
{
    public SkillProgramStepOutcome PayShanheS5Xiongluan(ProgramSkillFrame f) => getCoreGameEngineShanheS5ZhangxiuRuntime().PayShanheS5Xiongluan(f);
    public void ResolveShanheLeizhenUse(ProgramSkillFrame f) => getCoreGameEngineShanheLeizhenRuntime().ResolveShanheLeizhenUse(f);
}
