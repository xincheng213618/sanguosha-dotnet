#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed partial class CoreGameEngineDeckEquipmentProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _resolve;
    private CoreGameEngineDeckEquipmentProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeckEquipmentProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> resolve) => new(resolve);
}
