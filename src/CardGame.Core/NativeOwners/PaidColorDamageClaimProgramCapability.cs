#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PaidColorDamageClaimProgramCapability(Func<RuntimeEntries.CoreGameEnginePaidColorConversionsEntries> getCoreGameEnginePaidColorConversionsRuntime, Func<RuntimeEntries.CoreGameEngineActualTurnDamageClaimsEntries> getCoreGameEngineActualTurnDamageClaimsRuntime) : IPaidColorDamageClaimProgramHost
{
    public SkillProgramStepOutcome DiscardBoundCardForOppositeTurnDuel(ProgramSkillFrame f, string bind) => getCoreGameEnginePaidColorConversionsRuntime().BeginPaidColorConversion(f, bind);
    public SkillProgramStepOutcome ClaimActualTurnDamageEntities(ProgramSkillFrame f) => getCoreGameEngineActualTurnDamageClaimsRuntime().BeginActualTurnDamageClaim(f);
}
