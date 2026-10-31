#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardDamageMaximumHpReplacementRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengPuBoundaryMobileEntries _characterChengPuBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries _characterHuangZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries _characterPanZhangMaZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries _characterZhangFeiBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePlayerDeathEntries _coreNativePlayerDeathOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCardDamageMaximumHpReplacementRuntime(global::CardGame.Core.RuntimeEntries.CharacterChengPuBoundaryMobileEntries characterChengPuBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries characterHuangZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries characterPanZhangMaZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries characterZhangFeiBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreNativePlayerDeathEntries coreNativePlayerDeathOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterChengPuBoundaryMobileOperations = characterChengPuBoundaryMobileOperations;
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterHuangZhongBoundaryMobileOperations = characterHuangZhongBoundaryMobileOperations;
        this._characterPanZhangMaZhongBoundaryMobileOperations = characterPanZhangMaZhongBoundaryMobileOperations;
        this._characterZhangFeiBoundaryMobileOperations = characterZhangFeiBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativePlayerDeathOperations = coreNativePlayerDeathOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
