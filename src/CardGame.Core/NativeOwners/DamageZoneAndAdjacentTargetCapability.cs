#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageZoneAndAdjacentTargetCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime>> getCoreGameEngineDamageZoneAcquisitionRuntime, Func<RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries> getCoreGameEngineAdjacentRedTargetsRuntime) : IDamageZoneAndAdjacentTargetHost
{
    public SkillProgramStepOutcome ObtainEachDamageTargetZoneThenSelfOnly(ProgramSkillFrame f) => getCoreGameEngineDamageZoneAcquisitionRuntime().Value.BeginDamageZoneAcquisition(f);
    public SkillProgramStepOutcome JoinAdjacentRedCardTarget(ProgramSkillFrame f) => getCoreGameEngineAdjacentRedTargetsRuntime().BeginAdjacentRedTarget(f);
}
