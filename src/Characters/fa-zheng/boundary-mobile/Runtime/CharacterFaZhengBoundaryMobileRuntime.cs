#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterFaZhengBoundaryMobileRuntime
{
    private readonly ICharacterFaZhengBoundaryMobileCharacterJiangWeiBoundaryMobileOperations _characterJiangWeiBoundaryMobileOperations;
    private readonly ICharacterFaZhengBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterFaZhengBoundaryMobileCoreAssistedPhysicalCardsOperations _coreAssistedPhysicalCardsOperations;
    private readonly ICharacterFaZhengBoundaryMobileCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterFaZhengBoundaryMobileCoreProgramDodgeResponsesOperations _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterFaZhengBoundaryMobileRuntime(ICharacterFaZhengBoundaryMobileCharacterJiangWeiBoundaryMobileOperations characterJiangWeiBoundaryMobileOperations, ICharacterFaZhengBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterFaZhengBoundaryMobileCoreAssistedPhysicalCardsOperations coreAssistedPhysicalCardsOperations, ICharacterFaZhengBoundaryMobileCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterFaZhengBoundaryMobileCoreProgramDodgeResponsesOperations coreProgramDodgeResponsesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterJiangWeiBoundaryMobileOperations = characterJiangWeiBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAssistedPhysicalCardsOperations = coreAssistedPhysicalCardsOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
