#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramMultiTargetDuelsRuntime
{
    private readonly ICoreProgramMultiTargetDuelsCoreDirectedTurnCardPoliciesOperations _coreDirectedTurnCardPoliciesOperations;
    private readonly ICoreProgramMultiTargetDuelsCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreProgramMultiTargetDuelsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreProgramMultiTargetDuelsCoreNativeCardUseOperations _coreNativeCardUseOperations;
    private readonly ICoreProgramMultiTargetDuelsCoreNativeLegalActionsOperations _coreNativeLegalActionsOperations;
    private readonly ICoreProgramMultiTargetDuelsCoreNativeNullificationOperations _coreNativeNullificationOperations;
    private readonly ICoreProgramMultiTargetDuelsCoreNativeOwnedCardQueriesOperations _coreNativeOwnedCardQueriesOperations;
    private readonly ICoreProgramMultiTargetDuelsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramMultiTargetDuelsRuntime(ICoreProgramMultiTargetDuelsCoreDirectedTurnCardPoliciesOperations coreDirectedTurnCardPoliciesOperations, ICoreProgramMultiTargetDuelsCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreProgramMultiTargetDuelsCoreTableOrderOperations coreTableOrderOperations, ICoreProgramMultiTargetDuelsCoreNativeCardUseOperations coreNativeCardUseOperations, ICoreProgramMultiTargetDuelsCoreNativeLegalActionsOperations coreNativeLegalActionsOperations, ICoreProgramMultiTargetDuelsCoreNativeNullificationOperations coreNativeNullificationOperations, ICoreProgramMultiTargetDuelsCoreNativeOwnedCardQueriesOperations coreNativeOwnedCardQueriesOperations, ICoreProgramMultiTargetDuelsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
