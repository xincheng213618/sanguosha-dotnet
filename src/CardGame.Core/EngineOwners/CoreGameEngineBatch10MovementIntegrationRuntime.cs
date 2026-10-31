#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineBatch10MovementIntegrationRuntime
{
    private readonly ICoreBatch10MovementIntegrationCharacterBuZhiOlOperations _characterBuZhiOlOperations;
    private readonly ICoreBatch10MovementIntegrationCardZonesOperations _cardZonesOperations;
    private readonly ICoreBatch10MovementIntegrationContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreBatch10MovementIntegrationCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreBatch10MovementIntegrationCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreBatch10MovementIntegrationCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineBatch10MovementIntegrationRuntime(ICoreBatch10MovementIntegrationCharacterBuZhiOlOperations characterBuZhiOlOperations, ICoreBatch10MovementIntegrationCardZonesOperations cardZonesOperations, ICoreBatch10MovementIntegrationContentRegistryOperations contentRegistryOperations, ICoreBatch10MovementIntegrationCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreBatch10MovementIntegrationCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreBatch10MovementIntegrationCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBuZhiOlOperations = characterBuZhiOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
