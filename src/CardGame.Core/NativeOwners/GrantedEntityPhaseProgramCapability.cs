#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GrantedEntityPhaseProgramCapability(Func<RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries> getCoreGameEngineGrantedEntityPhasesRuntime) : IGrantedEntityPhaseProgramHost
{
    public SkillProgramStepOutcome InsertGrantedEntityPlayPhase(ProgramSkillFrame f) => getCoreGameEngineGrantedEntityPhasesRuntime().BeginGrantedEntityPlayPhase(f);
    public SkillProgramStepOutcome ClaimGrantedPhaseSlash(ProgramSkillFrame f) => getCoreGameEngineGrantedEntityPhasesRuntime().BeginGrantedPhaseSlashClaim(f);
}
