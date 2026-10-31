#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime))]
internal sealed partial class CoreGameEngineEquipmentDonationProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> _resolve;
    private CoreGameEngineEquipmentDonationProgramsEntries(Func<EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentDonationProgramsEntries Create(Func<EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> resolve) => new(resolve);
}
