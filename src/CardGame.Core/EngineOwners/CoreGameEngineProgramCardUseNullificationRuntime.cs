#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramCardUseNullificationRuntime : IWholeCardUseNullificationHost
{
    private readonly RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries _characterCaoZhenBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangOriginalSkillRulesEntries _coreWushuangOriginalSkillRulesOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly ICoreProgramCardUseNullificationCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardUseNullificationRuntime(RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries characterCaoZhenBoundaryMobileOperations, RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineWushuangOriginalSkillRulesEntries coreWushuangOriginalSkillRulesOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, ICoreProgramCardUseNullificationCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoZhenBoundaryMobileOperations = characterCaoZhenBoundaryMobileOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreWushuangOriginalSkillRulesOperations = coreWushuangOriginalSkillRulesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
