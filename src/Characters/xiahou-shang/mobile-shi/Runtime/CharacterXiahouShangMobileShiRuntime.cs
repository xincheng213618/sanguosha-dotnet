#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterXiahouShangMobileShiRuntime : global::CardGame.Core.IMobileShiTanfengHost
{
    private readonly ICharacterXiahouShangMobileShiContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterXiahouShangMobileShiResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterXiahouShangMobileShiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterXiahouShangMobileShiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterXiahouShangMobileShiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterXiahouShangMobileShiCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterXiahouShangMobileShiCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterXiahouShangMobileShiCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterXiahouShangMobileShiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterXiahouShangMobileShiRuntime(ICharacterXiahouShangMobileShiContentRegistryOperations contentRegistryOperations, ICharacterXiahouShangMobileShiResolutionStackOperations resolutionStackOperations, ICharacterXiahouShangMobileShiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterXiahouShangMobileShiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterXiahouShangMobileShiCoreRuntimeOperations coreRuntimeOperations, ICharacterXiahouShangMobileShiCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterXiahouShangMobileShiCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterXiahouShangMobileShiCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterXiahouShangMobileShiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
