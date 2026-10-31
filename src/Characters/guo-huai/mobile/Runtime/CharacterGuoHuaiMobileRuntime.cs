#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGuoHuaiMobileRuntime : global::CardGame.Core.IMobileGuoHuaiHost
{
    private readonly ICharacterGuoHuaiMobileCoreActualPlayPhaseCardUsesOperations _coreActualPlayPhaseCardUsesOperations;
    private readonly ICharacterGuoHuaiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterGuoHuaiMobileRuntime(ICharacterGuoHuaiMobileCoreActualPlayPhaseCardUsesOperations coreActualPlayPhaseCardUsesOperations, ICharacterGuoHuaiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActualPlayPhaseCardUsesOperations = coreActualPlayPhaseCardUsesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
