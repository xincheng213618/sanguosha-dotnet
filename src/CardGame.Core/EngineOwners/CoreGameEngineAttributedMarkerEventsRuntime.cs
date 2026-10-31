#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAttributedMarkerEventsRuntime : global::CardGame.Core.IAttributedEventProgramHost
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries _coreDamagePreventionProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAttributedMarkerEventsRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries coreDamagePreventionProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
