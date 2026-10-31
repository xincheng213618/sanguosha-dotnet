#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PersistentCombatPolicyProgramCapability(Func<RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries> getCoreGameEngineProgramPersistentCombatPoliciesRuntime) : IPersistentCombatPolicyProgramHost
{
    public void SetTurnHandLimitFromPlayDamage(ProgramSkillFrame frame) => getCoreGameEngineProgramPersistentCombatPoliciesRuntime().SetProgramTurnHandLimitFromPlayDamage(frame);
    public void GrantGameFactionAttackRangeTargets(ProgramSkillFrame frame, string factionId) => getCoreGameEngineProgramPersistentCombatPoliciesRuntime().GrantProgramGameFactionAttackRangeTargets(frame, factionId);
}
