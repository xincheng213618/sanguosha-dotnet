#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed partial class CoreGameEngineDeckEquipmentProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _resolve;
    private CoreGameEngineDeckEquipmentProgramsEntries(Func<EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeckEquipmentProgramsEntries Create(Func<EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> resolve) => new(resolve);
}
