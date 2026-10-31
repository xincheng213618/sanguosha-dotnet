#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramAllHandBonusOperationRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuMengMouMobileEntries _characterLuMengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiHappyGodEntries _characterSimaYiHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries _coreBoundDiscardSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries _coreHandComparisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramAllHandBonusOperationRuntime(global::CardGame.Core.RuntimeEntries.CharacterLuMengMouMobileEntries characterLuMengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiHappyGodEntries characterSimaYiHappyGodOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries coreBoundDiscardSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries coreHandComparisonOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuMengMouMobileOperations = characterLuMengMouMobileOperations;
        this._characterSimaYiHappyGodOperations = characterSimaYiHappyGodOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreBoundDiscardSlashBenefitsOperations = coreBoundDiscardSlashBenefitsOperations;
        this._coreHandComparisonOperations = coreHandComparisonOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
