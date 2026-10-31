#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramCardUseNullificationRuntime : global::CardGame.Core.IWholeCardUseNullificationHost
{
    private readonly ICoreProgramCardUseNullificationCharacterCaoZhenBoundaryMobileOperations _characterCaoZhenBoundaryMobileOperations;
    private readonly ICoreProgramCardUseNullificationCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICoreProgramCardUseNullificationContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramCardUseNullificationResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramCardUseNullificationCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramCardUseNullificationCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreProgramCardUseNullificationCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreProgramCardUseNullificationCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramCardUseNullificationCoreWushuangOriginalSkillRulesOperations _coreWushuangOriginalSkillRulesOperations;
    private readonly ICoreProgramCardUseNullificationCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramCardUseNullificationCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramCardUseNullificationKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardUseNullificationRuntime(ICoreProgramCardUseNullificationCharacterCaoZhenBoundaryMobileOperations characterCaoZhenBoundaryMobileOperations, ICoreProgramCardUseNullificationCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICoreProgramCardUseNullificationContentRegistryOperations contentRegistryOperations, ICoreProgramCardUseNullificationResolutionStackOperations resolutionStackOperations, ICoreProgramCardUseNullificationCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramCardUseNullificationCoreCardActionsOperations coreCardActionsOperations, ICoreProgramCardUseNullificationCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreProgramCardUseNullificationCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramCardUseNullificationCoreWushuangOriginalSkillRulesOperations coreWushuangOriginalSkillRulesOperations, ICoreProgramCardUseNullificationCoreMatchLogOperations coreMatchLogOperations, ICoreProgramCardUseNullificationCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramCardUseNullificationKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
