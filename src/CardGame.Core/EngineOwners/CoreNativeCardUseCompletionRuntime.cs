#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardUseCompletionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries _characterDengAiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuAnGuoOlEntries _characterWuAnGuoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries _characterXunXuAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUseTargetCandidateRefreshEntries _coreCurrentUseTargetCandidateRefreshOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries _coreHpLossSlashMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries _coreMantianGuohaiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries _coreNextCardTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNoDamageSkillDisableEntries _coreNoDamageSkillDisableOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries _coreSoleIncomingRangeDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeFactionSlashEntries _coreNativeFactionSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeFactionSlashCompletionEntries _coreNativeFactionSlashCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashTargetDuelEntries _coreNativeSlashTargetDuelOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly ICoreNativeCardUseCompletionCoreProgramNativeReturnsOperations _coreProgramNativeReturnsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    internal CoreNativeCardUseCompletionRuntime(global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries characterDengAiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWuAnGuoOlEntries characterWuAnGuoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries characterXunXuAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUseTargetCandidateRefreshEntries coreCurrentUseTargetCandidateRefreshOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries coreHpLossSlashMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries coreMantianGuohaiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries coreNextCardTargetAdjustmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNoDamageSkillDisableEntries coreNoDamageSkillDisableOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries coreSoleIncomingRangeDrawOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeFactionSlashEntries coreNativeFactionSlashOperations, global::CardGame.Core.RuntimeEntries.CoreNativeFactionSlashCompletionEntries coreNativeFactionSlashCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashTargetDuelEntries coreNativeSlashTargetDuelOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, ICoreNativeCardUseCompletionCoreProgramNativeReturnsOperations coreProgramNativeReturnsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess)
    {
        this._characterDengAiAnniversaryGodOperations = characterDengAiAnniversaryGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterHuaManOlOperations = characterHuaManOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterWuAnGuoOlOperations = characterWuAnGuoOlOperations;
        this._characterXinPiOlOperations = characterXinPiOlOperations;
        this._characterXunXuAnniversaryOperations = characterXunXuAnniversaryOperations;
        this._characterZhugeGuoLeOperations = characterZhugeGuoLeOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreCurrentUseTargetCandidateRefreshOperations = coreCurrentUseTargetCandidateRefreshOperations;
        this._coreExtraCardEffectRoundsOperations = coreExtraCardEffectRoundsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreHpLossSlashMaterialsOperations = coreHpLossSlashMaterialsOperations;
        this._coreMantianGuohaiOperations = coreMantianGuohaiOperations;
        this._coreNextCardTargetAdjustmentsOperations = coreNextCardTargetAdjustmentsOperations;
        this._coreNoDamageSkillDisableOperations = coreNoDamageSkillDisableOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreSoleIncomingRangeDrawOperations = coreSoleIncomingRangeDrawOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeFactionSlashOperations = coreNativeFactionSlashOperations;
        this._coreNativeFactionSlashCompletionOperations = coreNativeFactionSlashCompletionOperations;
        this._coreNativeSlashTargetDuelOperations = coreNativeSlashTargetDuelOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreProgramNativeReturnsOperations = coreProgramNativeReturnsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
    }
}
