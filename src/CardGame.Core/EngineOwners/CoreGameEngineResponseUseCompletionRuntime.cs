#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseUseCompletionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries _characterCuiYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries _characterHuangGaiMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries _characterMaMiDiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuAnGuoOlEntries _characterWuAnGuoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries _characterZouShiLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries _coreCurrentTurnUseKindsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedNullificationEntries _coreDelayedNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries _coreResponseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _coreTieredRoundZeroResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries _coreNativeDodgeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineResponseUseCompletionRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries characterCuiYanOlOperations,global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries characterHuangGaiMouMobileOperations,global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations,global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations,global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations,global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations,global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries characterMaMiDiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations,global::CardGame.Core.RuntimeEntries.CharacterWuAnGuoOlEntries characterWuAnGuoOlOperations,global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries characterZouShiLeOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries coreCurrentTurnUseKindsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedNullificationEntries coreDelayedNullificationOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries coreResponseCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries coreTieredRoundZeroResponsesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries coreNativeDodgeOperations,global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterCuiYanOlOperations = characterCuiYanOlOperations;
        this._characterHuangGaiMouMobileOperations = characterHuangGaiMouMobileOperations;
        this._characterLiFengOlOperations = characterLiFengOlOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterMaMiDiOlOperations = characterMaMiDiOlOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterWuAnGuoOlOperations = characterWuAnGuoOlOperations;
        this._characterZouShiLeOperations = characterZouShiLeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCurrentTurnUseKindsOperations = coreCurrentTurnUseKindsOperations;
        this._coreDelayedNullificationOperations = coreDelayedNullificationOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
        this._coreTieredRoundZeroResponsesOperations = coreTieredRoundZeroResponsesOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
