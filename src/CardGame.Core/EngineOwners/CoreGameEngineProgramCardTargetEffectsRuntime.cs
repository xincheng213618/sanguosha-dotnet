#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardTargetEffectsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangXiuMouOlEntries _characterZhangXiuMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries _characterZhouQunOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries _coreNamedAmbushInvalidationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries _coreProgramCardUseNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashTargetDuelEntries _coreNativeSlashTargetDuelOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardTargetEffectsRuntime(global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangXiuMouOlEntries characterZhangXiuMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries characterZhouQunOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries coreNamedAmbushInvalidationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries coreProgramCardUseNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashTargetDuelEntries coreNativeSlashTargetDuelOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterXinPiOlOperations = characterXinPiOlOperations;
        this._characterZhangXiuMouOlOperations = characterZhangXiuMouOlOperations;
        this._characterZhouQunOlOperations = characterZhouQunOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreNamedAmbushInvalidationOperations = coreNamedAmbushInvalidationOperations;
        this._coreProgramCardUseNullificationOperations = coreProgramCardUseNullificationOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeNullificationFramesOperations = coreNativeNullificationFramesOperations;
        this._coreNativeSlashTargetDuelOperations = coreNativeSlashTargetDuelOperations;
        this._coreNativeTrickUseOperations = coreNativeTrickUseOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
