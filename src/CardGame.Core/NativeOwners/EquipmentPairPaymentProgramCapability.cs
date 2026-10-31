#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentPairPaymentProgramCapability(Func<RuntimeEntries.CoreGameEngineEquipmentPairPaymentsEntries> getCoreGameEngineEquipmentPairPaymentsRuntime) : IEquipmentPairPaymentProgramHost
{
    public SkillProgramStepOutcome SelectEquipmentPairAndPayment(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineEquipmentPairPaymentsRuntime().SelectEquipmentPairAndPayment(f, e);
}
