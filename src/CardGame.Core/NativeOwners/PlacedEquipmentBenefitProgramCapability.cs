#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PlacedEquipmentBenefitProgramCapability(Func<RuntimeEntries.CoreGameEnginePlacedEquipmentBenefitsEntries> getCoreGameEnginePlacedEquipmentBenefitsRuntime) : IPlacedEquipmentBenefitProgramHost
{
    public SkillProgramStepOutcome PlaceOwnedEquipmentThenResolveSlotBenefit(ProgramSkillFrame f) => getCoreGameEnginePlacedEquipmentBenefitsRuntime().BeginPlacedEquipmentBenefit(f);
}
