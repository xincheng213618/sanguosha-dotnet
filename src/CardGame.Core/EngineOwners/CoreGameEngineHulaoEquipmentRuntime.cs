#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHulaoEquipmentRuntime
{
    private readonly ICoreHulaoEquipmentCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreHulaoEquipmentCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreHulaoEquipmentCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreHulaoEquipmentCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreHulaoEquipmentCoreExtendedViewAsOperations _coreExtendedViewAsOperations;
    private readonly ICoreHulaoEquipmentCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreHulaoEquipmentCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineHulaoEquipmentRuntime(ICoreHulaoEquipmentCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreHulaoEquipmentCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreHulaoEquipmentCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreHulaoEquipmentCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreHulaoEquipmentCoreExtendedViewAsOperations coreExtendedViewAsOperations, ICoreHulaoEquipmentCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreHulaoEquipmentCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
