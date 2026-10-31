#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PaidColorDamageClaimProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePaidColorConversionsEntries> getCoreGameEnginePaidColorConversionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnDamageClaimsEntries> getCoreGameEngineActualTurnDamageClaimsRuntime) : IPaidColorDamageClaimProgramHost
{
    public SkillProgramStepOutcome DiscardBoundCardForOppositeTurnDuel(ProgramSkillFrame f, string bind) => getCoreGameEnginePaidColorConversionsRuntime().BeginPaidColorConversion(f, bind);
    public SkillProgramStepOutcome ClaimActualTurnDamageEntities(ProgramSkillFrame f) => getCoreGameEngineActualTurnDamageClaimsRuntime().BeginActualTurnDamageClaim(f);
}
