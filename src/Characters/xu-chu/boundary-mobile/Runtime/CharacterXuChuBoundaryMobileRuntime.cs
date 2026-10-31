#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterXuChuBoundaryMobileRuntime : global::CardGame.Core.IBoundaryXuChuHost
{
    private readonly ICharacterXuChuBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterXuChuBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterXuChuBoundaryMobileCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICharacterXuChuBoundaryMobileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterXuChuBoundaryMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterXuChuBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterXuChuBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterXuChuBoundaryMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterXuChuBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterXuChuBoundaryMobileRuntime(ICharacterXuChuBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterXuChuBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterXuChuBoundaryMobileCoreActualTurnsOperations coreActualTurnsOperations, ICharacterXuChuBoundaryMobileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterXuChuBoundaryMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterXuChuBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterXuChuBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterXuChuBoundaryMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterXuChuBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
