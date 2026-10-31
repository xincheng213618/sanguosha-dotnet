#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePindianOwnedPileSourcesRuntime
{
    private readonly ICorePindianOwnedPileSourcesCharacterGuanHaiOlOperations _characterGuanHaiOlOperations;
    private readonly ICorePindianOwnedPileSourcesCoreOutsidePhaseDrawDiscardOperations _coreOutsidePhaseDrawDiscardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CorePindianOwnedPileSourcesRuntime(ICorePindianOwnedPileSourcesCharacterGuanHaiOlOperations characterGuanHaiOlOperations, ICorePindianOwnedPileSourcesCoreOutsidePhaseDrawDiscardOperations coreOutsidePhaseDrawDiscardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
