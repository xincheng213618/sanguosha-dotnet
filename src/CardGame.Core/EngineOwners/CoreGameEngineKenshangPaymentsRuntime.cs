#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineKenshangPaymentsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries _coreCardUsePaymentsOperations;
    internal CoreGameEngineKenshangPaymentsRuntime(global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries coreCardUsePaymentsOperations)
    {
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
    }
}
