#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCappedHandRefreshObserverRuntime
{
    private readonly ICoreCappedHandRefreshObserverContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreCappedHandRefreshObserverResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCappedHandRefreshObserverCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreCappedHandRefreshObserverCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreCappedHandRefreshObserverCoreProgramChildResolutionOperations _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    internal CoreGameEngineCappedHandRefreshObserverRuntime(ICoreCappedHandRefreshObserverContentRegistryOperations contentRegistryOperations, ICoreCappedHandRefreshObserverResolutionStackOperations resolutionStackOperations, ICoreCappedHandRefreshObserverCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreCappedHandRefreshObserverCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreCappedHandRefreshObserverCoreProgramChildResolutionOperations coreProgramChildResolutionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
    }
}
