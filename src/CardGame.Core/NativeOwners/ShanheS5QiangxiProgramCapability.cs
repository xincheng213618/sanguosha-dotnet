#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5QiangxiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5QiangxiRuntime>> getCoreGameEngineShanheS5QiangxiRuntime) : IShanheS5QiangxiHost
{
    public SkillProgramStepOutcome ResolveShanheS5Qiangxi(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheS5QiangxiRuntime().Value.ResolveShanheS5Qiangxi(frame, stage);
}
