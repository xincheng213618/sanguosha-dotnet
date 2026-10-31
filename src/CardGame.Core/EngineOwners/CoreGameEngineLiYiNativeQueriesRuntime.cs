#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineLiYiNativeQueriesRuntime
{
    private readonly ICoreLiYiNativeQueriesCharacterLiYiOlOperations _characterLiYiOlOperations;
    private readonly ICoreLiYiNativeQueriesCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreLiYiNativeQueriesCoreFieldEquipmentOperations _coreFieldEquipmentOperations;
    private readonly ICoreLiYiNativeQueriesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreLiYiNativeQueriesCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreLiYiNativeQueriesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineLiYiNativeQueriesRuntime(ICoreLiYiNativeQueriesCharacterLiYiOlOperations characterLiYiOlOperations, ICoreLiYiNativeQueriesCoreActionQueriesOperations coreActionQueriesOperations, ICoreLiYiNativeQueriesCoreFieldEquipmentOperations coreFieldEquipmentOperations, ICoreLiYiNativeQueriesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreLiYiNativeQueriesCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreLiYiNativeQueriesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLiYiOlOperations = characterLiYiOlOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
