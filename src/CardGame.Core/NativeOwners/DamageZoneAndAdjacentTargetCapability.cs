#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DamageZoneAndAdjacentTargetCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageZoneAcquisitionEntries> getCoreGameEngineDamageZoneAcquisitionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries> getCoreGameEngineAdjacentRedTargetsRuntime) : IDamageZoneAndAdjacentTargetHost
{
    public SkillProgramStepOutcome ObtainEachDamageTargetZoneThenSelfOnly(ProgramSkillFrame f) => getCoreGameEngineDamageZoneAcquisitionRuntime().BeginDamageZoneAcquisition(f);
    public SkillProgramStepOutcome JoinAdjacentRedCardTarget(ProgramSkillFrame f) => getCoreGameEngineAdjacentRedTargetsRuntime().BeginAdjacentRedTarget(f);
}
