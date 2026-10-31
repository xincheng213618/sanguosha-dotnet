#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineRecordedSuitSlashRuntime : global::CardGame.Core.IRecordedSuitSlashProgramHost
{
    private readonly ICoreRecordedSuitSlashCardZonesOperations _cardZonesOperations;
    private readonly ICoreRecordedSuitSlashContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRecordedSuitSlashResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRecordedSuitSlashCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreRecordedSuitSlashCoreAlternatingSuitAndCompletedTopOperations _coreAlternatingSuitAndCompletedTopOperations;
    private readonly ICoreRecordedSuitSlashCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreRecordedSuitSlashCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICoreRecordedSuitSlashCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreRecordedSuitSlashCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreRecordedSuitSlashCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreRecordedSuitSlashCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreRecordedSuitSlashKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRecordedSuitSlashRuntime(ICoreRecordedSuitSlashCardZonesOperations cardZonesOperations, ICoreRecordedSuitSlashContentRegistryOperations contentRegistryOperations, ICoreRecordedSuitSlashResolutionStackOperations resolutionStackOperations, ICoreRecordedSuitSlashCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreRecordedSuitSlashCoreAlternatingSuitAndCompletedTopOperations coreAlternatingSuitAndCompletedTopOperations, ICoreRecordedSuitSlashCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreRecordedSuitSlashCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICoreRecordedSuitSlashCoreRuntimeOperations coreRuntimeOperations, ICoreRecordedSuitSlashCoreMatchLogOperations coreMatchLogOperations, ICoreRecordedSuitSlashCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreRecordedSuitSlashCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreRecordedSuitSlashKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
