#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEquipmentSuitHandLimitRuntime
{
    private readonly ICoreEquipmentSuitHandLimitCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreEquipmentSuitHandLimitCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreEquipmentSuitHandLimitCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineEquipmentSuitHandLimitRuntime(ICoreEquipmentSuitHandLimitCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreEquipmentSuitHandLimitCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreEquipmentSuitHandLimitCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
