#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineParticipantPreventionRuntime : global::CardGame.Core.IParticipantPreventionHost
{
    private readonly ICoreParticipantPreventionContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreParticipantPreventionResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreParticipantPreventionCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreParticipantPreventionCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICoreParticipantPreventionCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreParticipantPreventionCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreParticipantPreventionCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineParticipantPreventionRuntime(ICoreParticipantPreventionContentRegistryOperations contentRegistryOperations, ICoreParticipantPreventionResolutionStackOperations resolutionStackOperations, ICoreParticipantPreventionCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreParticipantPreventionCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICoreParticipantPreventionCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreParticipantPreventionCoreRuntimeOperations coreRuntimeOperations, ICoreParticipantPreventionCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
