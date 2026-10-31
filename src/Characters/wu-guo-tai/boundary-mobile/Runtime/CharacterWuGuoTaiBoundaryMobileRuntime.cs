#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterWuGuoTaiBoundaryMobileRuntime : global::CardGame.Core.IBoundaryWuGuoTaiQueries
{
    private readonly ICharacterWuGuoTaiBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterWuGuoTaiBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterWuGuoTaiBoundaryMobileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterWuGuoTaiBoundaryMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterWuGuoTaiBoundaryMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterWuGuoTaiBoundaryMobileRuntime(ICharacterWuGuoTaiBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterWuGuoTaiBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterWuGuoTaiBoundaryMobileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterWuGuoTaiBoundaryMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterWuGuoTaiBoundaryMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
