#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNeighborDiscardOpportunitiesRuntime
{
    private readonly ICoreNeighborDiscardOpportunitiesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreNeighborDiscardOpportunitiesCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    private readonly ICoreNeighborDiscardOpportunitiesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreNeighborDiscardOpportunitiesCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreNeighborDiscardOpportunitiesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNeighborDiscardOpportunitiesRuntime(ICoreNeighborDiscardOpportunitiesContentRegistryOperations contentRegistryOperations, ICoreNeighborDiscardOpportunitiesCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations, ICoreNeighborDiscardOpportunitiesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreNeighborDiscardOpportunitiesCoreTableOrderOperations coreTableOrderOperations, ICoreNeighborDiscardOpportunitiesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
