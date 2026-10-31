#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicPlayedCardsRuntime
{
    private readonly ICorePublicPlayedCardsCardZonesOperations _cardZonesOperations;
    private readonly ICorePublicPlayedCardsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePublicPlayedCardsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICorePublicPlayedCardsCoreWushuangJinzhiOperations _coreWushuangJinzhiOperations;
    private readonly ICorePublicPlayedCardsCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    internal CoreGameEnginePublicPlayedCardsRuntime(ICorePublicPlayedCardsCardZonesOperations cardZonesOperations, ICorePublicPlayedCardsResolutionStackOperations resolutionStackOperations, ICorePublicPlayedCardsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICorePublicPlayedCardsCoreWushuangJinzhiOperations coreWushuangJinzhiOperations, ICorePublicPlayedCardsCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
    }
}
