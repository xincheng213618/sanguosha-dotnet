#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramFactionPoliciesRuntime
{
    private readonly ICoreProgramFactionPoliciesCharacterGuanYuRuntimeOperations _characterGuanYuRuntimeOperations;
    private readonly ICoreProgramFactionPoliciesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramFactionPoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreProgramFactionPoliciesCoreNativeEquipmentAndFactionQueriesOperations _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly ICoreProgramFactionPoliciesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramFactionPoliciesRuntime(ICoreProgramFactionPoliciesCharacterGuanYuRuntimeOperations characterGuanYuRuntimeOperations, ICoreProgramFactionPoliciesContentRegistryOperations contentRegistryOperations, ICoreProgramFactionPoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreProgramFactionPoliciesCoreNativeEquipmentAndFactionQueriesOperations coreNativeEquipmentAndFactionQueriesOperations, ICoreProgramFactionPoliciesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuRuntimeOperations = characterGuanYuRuntimeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
