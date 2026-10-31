#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFieldEquipmentRuntime
{
    private readonly ICoreFieldEquipmentCharacterWeiYanHappyGodOperations _characterWeiYanHappyGodOperations;
    private readonly ICoreFieldEquipmentCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreFieldEquipmentCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineFieldEquipmentRuntime(ICoreFieldEquipmentCharacterWeiYanHappyGodOperations characterWeiYanHappyGodOperations, ICoreFieldEquipmentCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreFieldEquipmentCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
