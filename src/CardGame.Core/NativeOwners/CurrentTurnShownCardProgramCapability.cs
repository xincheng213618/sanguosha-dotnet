#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CurrentTurnShownCardProgramCapability(Func<RuntimeEntries.CoreGameEngineCurrentTurnShownCardBenefitsEntries> getCoreGameEngineCurrentTurnShownCardBenefitsRuntime, Func<RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries> getCoreGameEngineCurrentTurnSkillSuppressionsRuntime) : ICurrentTurnShownCardProgramHost
{
    public void RevealOwnedBoundCardAppearance(ProgramSkillFrame f, string bind) => getCoreGameEngineCurrentTurnShownCardBenefitsRuntime().RevealOwnedBoundCardAppearance(f, bind);
    public void IssueCurrentTurnNonLockedSkillSuppression(ProgramSkillFrame f, int seat) => getCoreGameEngineCurrentTurnSkillSuppressionsRuntime().IssueCurrentTurnNonLockedSkillSuppression(f, seat);
    public void GrantCurrentTurnDirectedHeartSlashBonus(ProgramSkillFrame f, int seat) => getCoreGameEngineCurrentTurnShownCardBenefitsRuntime().GrantCurrentTurnDirectedHeartSlashBonus(f, seat);
}
