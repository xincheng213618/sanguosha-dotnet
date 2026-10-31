#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFieldEquipmentRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly ICoreFieldEquipmentCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineFieldEquipmentRuntime(global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, ICoreFieldEquipmentCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations)
    {
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
