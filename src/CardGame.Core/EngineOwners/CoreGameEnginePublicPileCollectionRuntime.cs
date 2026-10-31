#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePublicPileCollectionRuntime : global::CardGame.Core.IPublicPileCollectionProgramHost
{
    private readonly ICorePublicPileCollectionCardZonesOperations _cardZonesOperations;
    private readonly ICorePublicPileCollectionContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePublicPileCollectionResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePublicPileCollectionCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICorePublicPileCollectionCoreEphemeralCardMaterialsOperations _coreEphemeralCardMaterialsOperations;
    private readonly ICorePublicPileCollectionCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICorePublicPileCollectionCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePublicPileCollectionCoreProgramCardPaymentsOperations _coreProgramCardPaymentsOperations;
    private readonly ICorePublicPileCollectionCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePublicPileCollectionCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICorePublicPileCollectionCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICorePublicPileCollectionCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePublicPileCollectionRuntime(ICorePublicPileCollectionCardZonesOperations cardZonesOperations, ICorePublicPileCollectionContentRegistryOperations contentRegistryOperations, ICorePublicPileCollectionResolutionStackOperations resolutionStackOperations, ICorePublicPileCollectionCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICorePublicPileCollectionCoreEphemeralCardMaterialsOperations coreEphemeralCardMaterialsOperations, ICorePublicPileCollectionCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICorePublicPileCollectionCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePublicPileCollectionCoreProgramCardPaymentsOperations coreProgramCardPaymentsOperations, ICorePublicPileCollectionCoreRuntimeOperations coreRuntimeOperations, ICorePublicPileCollectionCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICorePublicPileCollectionCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICorePublicPileCollectionCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
