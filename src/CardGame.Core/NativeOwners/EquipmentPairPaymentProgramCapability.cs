#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentPairPaymentProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPairPaymentsEntries> getCoreGameEngineEquipmentPairPaymentsRuntime) : IEquipmentPairPaymentProgramHost
{
    public SkillProgramStepOutcome SelectEquipmentPairAndPayment(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineEquipmentPairPaymentsRuntime().SelectEquipmentPairAndPayment(f, e);
}
