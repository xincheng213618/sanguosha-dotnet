#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAttributedMarkerEventsRuntime : global::CardGame.Core.IAttributedEventProgramHost
{
    private readonly ICoreAttributedMarkerEventsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAttributedMarkerEventsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAttributedMarkerEventsCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreAttributedMarkerEventsCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICoreAttributedMarkerEventsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreAttributedMarkerEventsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreAttributedMarkerEventsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreAttributedMarkerEventsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAttributedMarkerEventsRuntime(ICoreAttributedMarkerEventsContentRegistryOperations contentRegistryOperations, ICoreAttributedMarkerEventsResolutionStackOperations resolutionStackOperations, ICoreAttributedMarkerEventsCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreAttributedMarkerEventsCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICoreAttributedMarkerEventsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreAttributedMarkerEventsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreAttributedMarkerEventsCoreTableOrderOperations coreTableOrderOperations, ICoreAttributedMarkerEventsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
