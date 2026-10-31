#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageCounterpartAcquisitionCapability(Func<RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries> getCoreGameEngineDamageCounterpartAcquisitionRuntime) : IDamageCounterpartAcquisitionHost
{
    public SkillProgramStepOutcome RecordDamageCounterpartAcquisition(ProgramSkillFrame f, string stateId, string skillId) => getCoreGameEngineDamageCounterpartAcquisitionRuntime().RecordDamageCounterpartAcquisition(f, stateId, skillId);
}
