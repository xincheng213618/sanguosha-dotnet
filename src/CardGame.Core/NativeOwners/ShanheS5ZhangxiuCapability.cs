#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5ZhangxiuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5ZhangxiuRuntime>> getCoreGameEngineShanheS5ZhangxiuRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheLeizhenRuntime>> getCoreGameEngineShanheLeizhenRuntime) : IShanheS5ZhangxiuHost
{
    public SkillProgramStepOutcome PayShanheS5Xiongluan(ProgramSkillFrame f) => getCoreGameEngineShanheS5ZhangxiuRuntime().Value.PayShanheS5Xiongluan(f);
    public void ResolveShanheLeizhenUse(ProgramSkillFrame f) => getCoreGameEngineShanheLeizhenRuntime().Value.ResolveShanheLeizhenUse(f);
}
