#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentDonationProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentDonationProgramsEntries> getCoreGameEngineEquipmentDonationProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualEndedTurnEquipmentEntries> getCoreGameEngineActualEndedTurnEquipmentRuntime) : IEquipmentDonationProgramHost
{
    public SkillProgramStepOutcome DonateAllEquipmentAndOfferRecipientBenefits(ProgramSkillFrame f, string usageId) => getCoreGameEngineEquipmentDonationProgramsRuntime().RunAllEquipmentDonation(f, usageId);
    public SkillProgramStepOutcome ChooseEquipmentOrDrawAfterOtherActualTurn(ProgramSkillFrame f) => getCoreGameEngineActualEndedTurnEquipmentRuntime().RunActualEndedTurnEquipment(f);
}
