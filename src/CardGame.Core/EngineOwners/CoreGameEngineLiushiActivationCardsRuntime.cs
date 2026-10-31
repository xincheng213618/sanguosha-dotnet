#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineLiushiActivationCardsRuntime : global::CardGame.Core.ILiushiTopdeckSelectionQuery
{
    private readonly ICoreLiushiActivationCardsCardZonesOperations _cardZonesOperations;
    private readonly ICoreLiushiActivationCardsCoreEphemeralCardMaterialsOperations _coreEphemeralCardMaterialsOperations;
    private readonly ICoreLiushiActivationCardsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineLiushiActivationCardsRuntime(ICoreLiushiActivationCardsCardZonesOperations cardZonesOperations, ICoreLiushiActivationCardsCoreEphemeralCardMaterialsOperations coreEphemeralCardMaterialsOperations, ICoreLiushiActivationCardsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
