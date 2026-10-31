#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class IssuedPlayUseProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries> getCoreGameEngineIssuedPlayUsePoliciesRuntime) : IIssuedPlayUseProgramHost
{
    public void IssueCardNoResponseAndPlayUseBan(ProgramSkillFrame frame) => getCoreGameEngineIssuedPlayUsePoliciesRuntime().IssueCardNoResponseAndPlayUseBan(frame);
}
