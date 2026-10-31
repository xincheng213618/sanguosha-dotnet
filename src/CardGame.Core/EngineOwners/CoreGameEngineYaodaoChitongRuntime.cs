#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineYaodaoChitongRuntime : global::CardGame.Core.IYaodaoChitongHost
{
    private readonly ICoreYaodaoChitongRandomOperations _randomOperations;
    private readonly ICoreYaodaoChitongResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreYaodaoChitongCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreYaodaoChitongCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreYaodaoChitongCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreYaodaoChitongCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreYaodaoChitongCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreYaodaoChitongCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreYaodaoChitongCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreYaodaoChitongCoreJudgmentOutcomeDamageEdgesOperations _coreJudgmentOutcomeDamageEdgesOperations;
    private readonly ICoreYaodaoChitongCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreYaodaoChitongCoreProgramJudgmentOutcomesOperations _coreProgramJudgmentOutcomesOperations;
    private readonly ICoreYaodaoChitongCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreYaodaoChitongCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreYaodaoChitongCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreYaodaoChitongCoreNativeCardUseFramesOperations _coreNativeCardUseFramesOperations;
    private readonly ICoreYaodaoChitongCoreNativeEquipmentAndFactionQueriesOperations _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly ICoreYaodaoChitongCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreYaodaoChitongCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreYaodaoChitongCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreYaodaoChitongKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineYaodaoChitongRuntime(ICoreYaodaoChitongRandomOperations randomOperations, ICoreYaodaoChitongResolutionStackOperations resolutionStackOperations, ICoreYaodaoChitongCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreYaodaoChitongCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreYaodaoChitongCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreYaodaoChitongCoreCardActionsOperations coreCardActionsOperations, ICoreYaodaoChitongCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreYaodaoChitongCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreYaodaoChitongCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreYaodaoChitongCoreJudgmentOutcomeDamageEdgesOperations coreJudgmentOutcomeDamageEdgesOperations, ICoreYaodaoChitongCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreYaodaoChitongCoreProgramJudgmentOutcomesOperations coreProgramJudgmentOutcomesOperations, ICoreYaodaoChitongCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreYaodaoChitongCoreRuntimeOperations coreRuntimeOperations, ICoreYaodaoChitongCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreYaodaoChitongCoreNativeCardUseFramesOperations coreNativeCardUseFramesOperations, ICoreYaodaoChitongCoreNativeEquipmentAndFactionQueriesOperations coreNativeEquipmentAndFactionQueriesOperations, ICoreYaodaoChitongCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreYaodaoChitongCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreYaodaoChitongCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreYaodaoChitongKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreJudgmentOutcomeDamageEdgesOperations = coreJudgmentOutcomeDamageEdgesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramJudgmentOutcomesOperations = coreProgramJudgmentOutcomesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
