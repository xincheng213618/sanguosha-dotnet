#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PlacedEquipmentBenefitProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePlacedEquipmentBenefitsRuntime>> getCoreGameEnginePlacedEquipmentBenefitsRuntime) : IPlacedEquipmentBenefitProgramHost
{
    public SkillProgramStepOutcome PlaceOwnedEquipmentThenResolveSlotBenefit(ProgramSkillFrame f) => getCoreGameEnginePlacedEquipmentBenefitsRuntime().Value.BeginPlacedEquipmentBenefit(f);
}
