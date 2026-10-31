#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIssuedPlayUsePoliciesEntries
{
    internal void CleanupIssuedPlayPhaseUseBans() => _resolve().CleanupIssuedPlayPhaseUseBans();
    internal bool DistanceCardUseExemption140HasFirstActualPlayUseDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption140HasFirstActualPlayUseDistance(q);
    internal bool DistanceCardUseExemption150HasIssuedFirstPlayUseDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption150HasIssuedFirstPlayUseDistance(q);
    internal bool HasFirstActualPlayUseDistance(CharacterState actor) => _resolve().HasFirstActualPlayUseDistance(actor);
    internal bool HasIssuedFirstPlayUseDistance(long? frameId) => _resolve().HasIssuedFirstPlayUseDistance(frameId);
    internal bool HasIssuedPlayPhaseUseBan(int seat) => _resolve().HasIssuedPlayPhaseUseBan(seat);
    internal bool HasIssuedPolicyForAction(long actionId) => _resolve().HasIssuedPolicyForAction(actionId);
    internal bool IsIssuedCardUnrespondable(long frameId) => _resolve().IsIssuedCardUnrespondable(frameId);
    internal void IssueCardNoResponseAndPlayUseBan(ProgramSkillFrame frame) => _resolve().IssueCardNoResponseAndPlayUseBan(frame);
}
