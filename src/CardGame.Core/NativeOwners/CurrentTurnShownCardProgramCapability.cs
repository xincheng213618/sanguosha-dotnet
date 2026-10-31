#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CurrentTurnShownCardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnShownCardBenefitsEntries> getCoreGameEngineCurrentTurnShownCardBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries> getCoreGameEngineCurrentTurnSkillSuppressionsRuntime) : ICurrentTurnShownCardProgramHost
{
    public void RevealOwnedBoundCardAppearance(ProgramSkillFrame f, string bind) => getCoreGameEngineCurrentTurnShownCardBenefitsRuntime().RevealOwnedBoundCardAppearance(f, bind);
    public void IssueCurrentTurnNonLockedSkillSuppression(ProgramSkillFrame f, int seat) => getCoreGameEngineCurrentTurnSkillSuppressionsRuntime().IssueCurrentTurnNonLockedSkillSuppression(f, seat);
    public void GrantCurrentTurnDirectedHeartSlashBonus(ProgramSkillFrame f, int seat) => getCoreGameEngineCurrentTurnShownCardBenefitsRuntime().GrantCurrentTurnDirectedHeartSlashBonus(f, seat);
}
