#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFieldEquipmentRuntime
{
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly ICoreFieldEquipmentCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineFieldEquipmentRuntime(RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, ICoreFieldEquipmentCoreEquipmentContentOperations coreEquipmentContentOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations)
    {
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
