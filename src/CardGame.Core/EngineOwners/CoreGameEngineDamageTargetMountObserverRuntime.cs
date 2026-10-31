#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamageTargetMountObserverRuntime
{
    private readonly ICoreDamageTargetMountObserverResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDamageTargetMountObserverCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreDamageTargetMountObserverCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreDamageTargetMountObserverCoreDamageTargetMountProgramsOperations _coreDamageTargetMountProgramsOperations;
    private readonly ICoreDamageTargetMountObserverCoreProgramChildResolutionOperations _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineDamageTargetMountObserverRuntime(ICoreDamageTargetMountObserverResolutionStackOperations resolutionStackOperations, ICoreDamageTargetMountObserverCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreDamageTargetMountObserverCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreDamageTargetMountObserverCoreDamageTargetMountProgramsOperations coreDamageTargetMountProgramsOperations, ICoreDamageTargetMountObserverCoreProgramChildResolutionOperations coreProgramChildResolutionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreDamageTargetMountProgramsOperations = coreDamageTargetMountProgramsOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
