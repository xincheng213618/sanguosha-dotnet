
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_EquipmentDonationPrograms
{
    internal static string EquipmentDonationUsage(string id) => $"equipment-donation:{id}";
}
