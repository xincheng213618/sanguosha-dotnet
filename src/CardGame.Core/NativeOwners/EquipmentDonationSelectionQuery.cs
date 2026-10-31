#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentDonationSelectionQuery(Func<RuntimeEntries.CoreGameEngineEquipmentDonationProgramsEntries> getCoreGameEngineEquipmentDonationProgramsRuntime) : IEquipmentDonationSelectionQuery
{
    public bool CanPay(int ownerSeat, string skillId, string stateId) => getCoreGameEngineEquipmentDonationProgramsRuntime().HasPayableAllEquipmentDonation(ownerSeat, skillId, stateId);
}
