#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXiaoruiCapability(Func<RuntimeEntries.CoreGameEngineShanheXiaoruiEntries> getCoreGameEngineShanheXiaoruiRuntime) : IShanheXiaoruiProgramHost
{
    public SkillProgramStepOutcome GrantActualTurnSlashQuota(ProgramSkillFrame frame) => getCoreGameEngineShanheXiaoruiRuntime().GrantShanheXiaoruiQuota(frame);
}
