#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineCardMovementProgramsRuntime : global::CardGame.Core.ICardMovementBatchFacts, global::CardGame.Core.ICardMovementBatchPipeline
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFaZhengMouMobileEntries _characterFaZhengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries _characterHuCheErAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQiaoRuiAnniversaryEntries _characterQiaoRuiAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiOlGod2016Entries _characterSimaYiOlGod2016Operations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries _characterZhouFeiLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPhaseScheduleAccess _programPhaseScheduleOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries _coreActualHandGainProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries _coreGainGiftProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheChanrangEdictEntries _coreShanheChanrangEdictOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries _coreWenhePoisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramShanheSourceZhaoranEntries _coreProgramShanheSourceZhaoranOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateCardsMovedTriggerWindowHostOperations _kernelCoreGameEngineProgramTriggerWindowsOperations;
    private readonly ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations _cardMovementBatchCoordinatorOperations;
    private readonly ICoreCardMovementProgramsCardsMovedWindowOperations _cardsMovedWindowOperations;
    private readonly ICoreCardMovementProgramsProgramWindowCursorOperations _programWindowCursorOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreGameEngineCardMovementProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterFaZhengMouMobileEntries characterFaZhengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries characterHuCheErAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterQiaoRuiAnniversaryEntries characterQiaoRuiAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiOlGod2016Entries characterSimaYiOlGod2016Operations, global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries characterZhouFeiLeOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPhaseScheduleAccess programPhaseScheduleOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries coreActualHandGainProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries coreGainGiftProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheChanrangEdictEntries coreShanheChanrangEdictOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries coreWenhePoisonOperations, global::CardGame.Core.RuntimeEntries.CoreProgramShanheSourceZhaoranEntries coreProgramShanheSourceZhaoranOperations, global::CardGame.Core.EngineOwners.IKernelCreateCardsMovedTriggerWindowHostOperations kernelCoreGameEngineProgramTriggerWindowsOperations, ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations cardMovementBatchCoordinatorOperations, ICoreCardMovementProgramsCardsMovedWindowOperations cardsMovedWindowOperations, ICoreCardMovementProgramsProgramWindowCursorOperations programWindowCursorOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterFaZhengMouMobileOperations = characterFaZhengMouMobileOperations;
        this._characterHuCheErAnniversaryOperations = characterHuCheErAnniversaryOperations;
        this._characterQiaoRuiAnniversaryOperations = characterQiaoRuiAnniversaryOperations;
        this._characterSimaYiOlGod2016Operations = characterSimaYiOlGod2016Operations;
        this._characterZhaoZhongOlOperations = characterZhaoZhongOlOperations;
        this._characterZhouFeiLeOperations = characterZhouFeiLeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPhaseScheduleOperations = programPhaseScheduleOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualHandGainProgramsOperations = coreActualHandGainProgramsOperations;
        this._coreGainGiftProgramsOperations = coreGainGiftProgramsOperations;
        this._coreJudgedRankSlashPoliciesOperations = coreJudgedRankSlashPoliciesOperations;
        this._coreShanheChanrangEdictOperations = coreShanheChanrangEdictOperations;
        this._coreWenhePoisonOperations = coreWenhePoisonOperations;
        this._coreProgramShanheSourceZhaoranOperations = coreProgramShanheSourceZhaoranOperations;
        this._kernelCoreGameEngineProgramTriggerWindowsOperations = kernelCoreGameEngineProgramTriggerWindowsOperations;
        this._cardMovementBatchCoordinatorOperations = cardMovementBatchCoordinatorOperations;
        this._cardsMovedWindowOperations = cardsMovedWindowOperations;
        this._programWindowCursorOperations = programWindowCursorOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
