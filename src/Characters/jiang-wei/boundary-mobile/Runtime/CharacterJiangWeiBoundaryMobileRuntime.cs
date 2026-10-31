#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterJiangWeiBoundaryMobileRuntime
{
    private readonly ICharacterJiangWeiBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterJiangWeiBoundaryMobileCoreAssistedPhysicalCardsOperations _coreAssistedPhysicalCardsOperations;
    private readonly ICharacterJiangWeiBoundaryMobileCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterJiangWeiBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterJiangWeiBoundaryMobileCoreProgramSlashRequestOperations _coreProgramSlashRequestOperations;
    private readonly ICharacterJiangWeiBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterJiangWeiBoundaryMobileCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterJiangWeiBoundaryMobileRuntime(ICharacterJiangWeiBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterJiangWeiBoundaryMobileCoreAssistedPhysicalCardsOperations coreAssistedPhysicalCardsOperations, ICharacterJiangWeiBoundaryMobileCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterJiangWeiBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterJiangWeiBoundaryMobileCoreProgramSlashRequestOperations coreProgramSlashRequestOperations, ICharacterJiangWeiBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterJiangWeiBoundaryMobileCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAssistedPhysicalCardsOperations = coreAssistedPhysicalCardsOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramSlashRequestOperations = coreProgramSlashRequestOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
