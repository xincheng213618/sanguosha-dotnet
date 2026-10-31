#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterSunQuanBoundaryMobileRuntime
{
    private readonly ICharacterSunQuanBoundaryMobileCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICharacterSunQuanBoundaryMobileCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterSunQuanBoundaryMobileCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICharacterSunQuanBoundaryMobileCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICharacterSunQuanBoundaryMobileCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    private readonly ICharacterSunQuanBoundaryMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterSunQuanBoundaryMobileRuntime(ICharacterSunQuanBoundaryMobileCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICharacterSunQuanBoundaryMobileCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterSunQuanBoundaryMobileCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICharacterSunQuanBoundaryMobileCoreTableOrderOperations coreTableOrderOperations, ICharacterSunQuanBoundaryMobileCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations, ICharacterSunQuanBoundaryMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
