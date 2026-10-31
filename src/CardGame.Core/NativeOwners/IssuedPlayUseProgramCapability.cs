#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class IssuedPlayUseProgramCapability(Func<RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries> getCoreGameEngineIssuedPlayUsePoliciesRuntime) : IIssuedPlayUseProgramHost
{
    public void IssueCardNoResponseAndPlayUseBan(ProgramSkillFrame frame) => getCoreGameEngineIssuedPlayUsePoliciesRuntime().IssueCardNoResponseAndPlayUseBan(frame);
}
