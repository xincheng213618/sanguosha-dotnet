#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PlacedEquipmentBenefitProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlacedEquipmentBenefitsEntries> getCoreGameEnginePlacedEquipmentBenefitsRuntime) : IPlacedEquipmentBenefitProgramHost
{
    public SkillProgramStepOutcome PlaceOwnedEquipmentThenResolveSlotBenefit(ProgramSkillFrame f) => getCoreGameEnginePlacedEquipmentBenefitsRuntime().BeginPlacedEquipmentBenefit(f);
}
