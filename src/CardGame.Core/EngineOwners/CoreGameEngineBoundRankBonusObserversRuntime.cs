#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineBoundRankBonusObserversRuntime
{
    private readonly ICoreBoundRankBonusObserversContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreBoundRankBonusObserversResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreBoundRankBonusObserversCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreBoundRankBonusObserversCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreBoundRankBonusObserversCoreProgramChildResolutionOperations _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineBoundRankBonusObserversRuntime(ICoreBoundRankBonusObserversContentRegistryOperations contentRegistryOperations, ICoreBoundRankBonusObserversResolutionStackOperations resolutionStackOperations, ICoreBoundRankBonusObserversCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreBoundRankBonusObserversCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreBoundRankBonusObserversCoreProgramChildResolutionOperations coreProgramChildResolutionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
