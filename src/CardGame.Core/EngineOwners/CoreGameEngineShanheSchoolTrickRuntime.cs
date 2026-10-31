#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSchoolTrickRuntime : global::CardGame.Core.IShanheSchoolTrickHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries _characterHuangYueyingOlMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJiangJiMouEntries _characterJiangJiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries _coreNamedAmbushInvalidationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries _coreNextCardTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries _coreShanheSchoolsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries _coreSoleIncomingEffectRepeatOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSchoolTrickRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations,global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations,global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries characterHuangYueyingOlMouOperations,global::CardGame.Core.RuntimeEntries.CharacterJiangJiMouEntries characterJiangJiMouOperations,global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries coreNamedAmbushInvalidationOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries coreNextCardTargetAdjustmentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries coreShanheSchoolsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries coreSoleIncomingEffectRepeatOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterHuangYueyingOlMouOperations = characterHuangYueyingOlMouOperations;
        this._characterJiangJiMouOperations = characterJiangJiMouOperations;
        this._characterSunQuanOlMagicOperations = characterSunQuanOlMagicOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreNamedAmbushInvalidationOperations = coreNamedAmbushInvalidationOperations;
        this._coreNextCardTargetAdjustmentsOperations = coreNextCardTargetAdjustmentsOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreShanheSchoolsOperations = coreShanheSchoolsOperations;
        this._coreSoleIncomingEffectRepeatOperations = coreSoleIncomingEffectRepeatOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
