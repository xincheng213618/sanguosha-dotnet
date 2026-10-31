#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAnqiFeirenRuntime : global::CardGame.Core.IAnqiFeirenHost
{
    private readonly ICoreAnqiFeirenContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAnqiFeirenRandomOperations _randomOperations;
    private readonly ICoreAnqiFeirenResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAnqiFeirenCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreAnqiFeirenCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreAnqiFeirenCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreAnqiFeirenCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreAnqiFeirenCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreAnqiFeirenCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreAnqiFeirenKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess;
    internal CoreGameEngineAnqiFeirenRuntime(ICoreAnqiFeirenContentRegistryOperations contentRegistryOperations, ICoreAnqiFeirenRandomOperations randomOperations, ICoreAnqiFeirenResolutionStackOperations resolutionStackOperations, ICoreAnqiFeirenCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreAnqiFeirenCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreAnqiFeirenCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreAnqiFeirenCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreAnqiFeirenCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreAnqiFeirenCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreAnqiFeirenKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess = coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess;
    }
}
