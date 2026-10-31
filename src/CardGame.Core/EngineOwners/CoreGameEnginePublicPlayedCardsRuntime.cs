#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicPlayedCardsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries _coreSnapshotValueProjectionOperations;
    internal CoreGameEnginePublicPlayedCardsRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries coreSnapshotValueProjectionOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
    }
}
