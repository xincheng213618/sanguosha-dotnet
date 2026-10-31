#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterFaZhengBoundaryMobileRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJiangWeiBoundaryMobileEntries _characterJiangWeiBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAssistedPhysicalCardsEntries _coreAssistedPhysicalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries _corePrivateOfferReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterFaZhengBoundaryMobileRuntime(global::CardGame.Core.RuntimeEntries.CharacterJiangWeiBoundaryMobileEntries characterJiangWeiBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAssistedPhysicalCardsEntries coreAssistedPhysicalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries corePrivateOfferReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterJiangWeiBoundaryMobileOperations = characterJiangWeiBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAssistedPhysicalCardsOperations = coreAssistedPhysicalCardsOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
