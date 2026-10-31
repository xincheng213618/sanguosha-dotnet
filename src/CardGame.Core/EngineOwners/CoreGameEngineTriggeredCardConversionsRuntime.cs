#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineTriggeredCardConversionsRuntime : global::CardGame.Core.ITriggeredCardConversionHost
{
    private readonly ICoreTriggeredCardConversionsCardZonesOperations _cardZonesOperations;
    private readonly ICoreTriggeredCardConversionsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreTriggeredCardConversionsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreTriggeredCardConversionsCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreTriggeredCardConversionsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreTriggeredCardConversionsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreTriggeredCardConversionsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreTriggeredCardConversionsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreTriggeredCardConversionsCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreTriggeredCardConversionsCoreNativeSlashUseOperations _coreNativeSlashUseOperations;
    private readonly ICoreTriggeredCardConversionsCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreTriggeredCardConversionsCoreProgramParticipantPhaseOperationsOperations _coreProgramParticipantPhaseOperationsOperations;
    private readonly ICoreTriggeredCardConversionsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTriggeredCardConversionsRuntime(ICoreTriggeredCardConversionsCardZonesOperations cardZonesOperations, ICoreTriggeredCardConversionsContentRegistryOperations contentRegistryOperations, ICoreTriggeredCardConversionsResolutionStackOperations resolutionStackOperations, ICoreTriggeredCardConversionsCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreTriggeredCardConversionsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreTriggeredCardConversionsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreTriggeredCardConversionsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreTriggeredCardConversionsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreTriggeredCardConversionsCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreTriggeredCardConversionsCoreNativeSlashUseOperations coreNativeSlashUseOperations, ICoreTriggeredCardConversionsCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreTriggeredCardConversionsCoreProgramParticipantPhaseOperationsOperations coreProgramParticipantPhaseOperationsOperations, ICoreTriggeredCardConversionsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
