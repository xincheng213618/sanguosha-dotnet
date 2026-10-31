#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CurrentUsePhysicalClaimProgramCapability(Func<RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries> getCoreGameEngineCurrentUsePhysicalClaimsRuntime) : ICurrentUsePhysicalClaimProgramHost
{
    public SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame) => getCoreGameEngineCurrentUsePhysicalClaimsRuntime().ClaimCurrentUsePhysicalCards(frame);
    public void PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame) => getCoreGameEngineCurrentUsePhysicalClaimsRuntime().PreventCurrentTargetSlashCancellationByRule(frame);
}
