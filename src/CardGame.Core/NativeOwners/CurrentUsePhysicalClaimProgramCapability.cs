#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CurrentUsePhysicalClaimProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries> getCoreGameEngineCurrentUsePhysicalClaimsRuntime) : ICurrentUsePhysicalClaimProgramHost
{
    public SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame) => getCoreGameEngineCurrentUsePhysicalClaimsRuntime().ClaimCurrentUsePhysicalCards(frame);
    public void PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame) => getCoreGameEngineCurrentUsePhysicalClaimsRuntime().PreventCurrentTargetSlashCancellationByRule(frame);
}
