#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterXuHuangBoundaryMobileRuntime
{
    private readonly ICharacterXuHuangBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterXuHuangBoundaryMobileCoreNativeSkillContextQueriesOperations _coreNativeSkillContextQueriesOperations;
    private readonly ICharacterXuHuangBoundaryMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CharacterXuHuangBoundaryMobileRuntime(ICharacterXuHuangBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterXuHuangBoundaryMobileCoreNativeSkillContextQueriesOperations coreNativeSkillContextQueriesOperations, ICharacterXuHuangBoundaryMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
