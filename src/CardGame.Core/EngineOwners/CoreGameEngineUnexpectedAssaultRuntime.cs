#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineUnexpectedAssaultRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries _coreNativeTrickEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries _coreSnapshotValueProjectionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardAttackHandleFactory _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineUnexpectedAssaultRuntime(global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations,global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations,global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations,global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries coreNativeTrickEffectsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries coreSnapshotValueProjectionOperations,global::CardGame.Core.EngineOwners.IKernelCardAttackHandleFactory kernelCoreMainOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreExtraCardEffectRoundsOperations = coreExtraCardEffectRoundsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativeTrickEffectsOperations = coreNativeTrickEffectsOperations;
        this._coreNativeTrickUseOperations = coreNativeTrickUseOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
