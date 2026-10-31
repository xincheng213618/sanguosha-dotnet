#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXiaoruiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheXiaoruiRuntime>> getCoreGameEngineShanheXiaoruiRuntime) : IShanheXiaoruiProgramHost
{
    public SkillProgramStepOutcome GrantActualTurnSlashQuota(ProgramSkillFrame frame) => getCoreGameEngineShanheXiaoruiRuntime().Value.GrantShanheXiaoruiQuota(frame);
}
