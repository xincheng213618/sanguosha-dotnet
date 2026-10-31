#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageCounterpartAcquisitionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries> getCoreGameEngineDamageCounterpartAcquisitionRuntime) : IDamageCounterpartAcquisitionHost
{
    public SkillProgramStepOutcome RecordDamageCounterpartAcquisition(ProgramSkillFrame f, string stateId, string skillId) => getCoreGameEngineDamageCounterpartAcquisitionRuntime().RecordDamageCounterpartAcquisition(f, stateId, skillId);
}
