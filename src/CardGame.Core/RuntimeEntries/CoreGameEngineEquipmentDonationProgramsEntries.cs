#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime))]
internal sealed partial class CoreGameEngineEquipmentDonationProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> _resolve;
    private CoreGameEngineEquipmentDonationProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentDonationProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> resolve) => new(resolve);
}
