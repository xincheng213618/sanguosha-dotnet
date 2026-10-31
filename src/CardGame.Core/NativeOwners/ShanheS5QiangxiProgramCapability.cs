#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5QiangxiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QiangxiEntries> getCoreGameEngineShanheS5QiangxiRuntime) : IShanheS5QiangxiHost
{
    public SkillProgramStepOutcome ResolveShanheS5Qiangxi(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheS5QiangxiRuntime().ResolveShanheS5Qiangxi(frame, stage);
}
