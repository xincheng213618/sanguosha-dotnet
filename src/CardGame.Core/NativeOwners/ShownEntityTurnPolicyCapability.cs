#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShownEntityTurnPolicyCapability(Func<RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries> getCoreGameEngineShownEntityTurnPoliciesRuntime) : IShownEntityTurnPolicyHost
{
    public void IssueShownEntityTurnPolicy(ProgramSkillFrame f, string bind, string stateId) => getCoreGameEngineShownEntityTurnPoliciesRuntime().IssueShownEntityTurnPolicy(f, bind, stateId);
}
