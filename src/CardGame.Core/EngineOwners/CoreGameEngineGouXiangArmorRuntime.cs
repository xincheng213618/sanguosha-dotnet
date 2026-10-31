#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineGouXiangArmorRuntime : global::CardGame.Core.IGouXiangArmorHost
{
    private readonly ICoreGouXiangArmorCharacterHuCheErMouOperations _characterHuCheErMouOperations;
    private readonly ICoreGouXiangArmorContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreGouXiangArmorRandomOperations _randomOperations;
    private readonly ICoreGouXiangArmorResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreGouXiangArmorCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreGouXiangArmorCoreCardDeclarationsOperations _coreCardDeclarationsOperations;
    private readonly ICoreGouXiangArmorCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreGouXiangArmorCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreGouXiangArmorCoreExtendedViewAsOperations _coreExtendedViewAsOperations;
    private readonly ICoreGouXiangArmorCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreGouXiangArmorCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreGouXiangArmorCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreGouXiangArmorCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreGouXiangArmorCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreGouXiangArmorKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly ICoreGouXiangArmorKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineGouXiangArmorRuntime(ICoreGouXiangArmorCharacterHuCheErMouOperations characterHuCheErMouOperations, ICoreGouXiangArmorContentRegistryOperations contentRegistryOperations, ICoreGouXiangArmorRandomOperations randomOperations, ICoreGouXiangArmorResolutionStackOperations resolutionStackOperations, ICoreGouXiangArmorCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreGouXiangArmorCoreCardDeclarationsOperations coreCardDeclarationsOperations, ICoreGouXiangArmorCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreGouXiangArmorCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreGouXiangArmorCoreExtendedViewAsOperations coreExtendedViewAsOperations, ICoreGouXiangArmorCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreGouXiangArmorCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreGouXiangArmorCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreGouXiangArmorCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreGouXiangArmorCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreGouXiangArmorKernelCoreMainOperations kernelCoreMainOperations, ICoreGouXiangArmorKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
