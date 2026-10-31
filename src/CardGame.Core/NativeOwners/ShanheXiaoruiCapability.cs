#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXiaoruiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheXiaoruiEntries> getCoreGameEngineShanheXiaoruiRuntime) : IShanheXiaoruiProgramHost
{
    public SkillProgramStepOutcome GrantActualTurnSlashQuota(ProgramSkillFrame frame) => getCoreGameEngineShanheXiaoruiRuntime().GrantShanheXiaoruiQuota(frame);
}
