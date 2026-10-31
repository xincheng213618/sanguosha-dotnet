#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheS5ShuangxiongRuntime
{
    private readonly ICoreShanheS5ShuangxiongCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheS5ShuangxiongContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5ShuangxiongResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5ShuangxiongTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreShanheS5ShuangxiongCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreShanheS5ShuangxiongCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreShanheS5ShuangxiongCorePaidColorConversionsOperations _corePaidColorConversionsOperations;
    private readonly ICoreShanheS5ShuangxiongCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheS5ShuangxiongCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheS5ShuangxiongKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5ShuangxiongRuntime(ICoreShanheS5ShuangxiongCardZonesOperations cardZonesOperations, ICoreShanheS5ShuangxiongContentRegistryOperations contentRegistryOperations, ICoreShanheS5ShuangxiongResolutionStackOperations resolutionStackOperations, ICoreShanheS5ShuangxiongTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreShanheS5ShuangxiongCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreShanheS5ShuangxiongCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreShanheS5ShuangxiongCorePaidColorConversionsOperations corePaidColorConversionsOperations, ICoreShanheS5ShuangxiongCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheS5ShuangxiongCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheS5ShuangxiongKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._corePaidColorConversionsOperations = corePaidColorConversionsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
