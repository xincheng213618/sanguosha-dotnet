#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterHuaTuoHappyGodRuntime : global::CardGame.Core.IHappyHuaTuoHost
{
    private readonly ICharacterHuaTuoHappyGodContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterHuaTuoHappyGodResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterHuaTuoHappyGodCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterHuaTuoHappyGodCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterHuaTuoHappyGodCoreParticipantAndReserveProgramsOperations _coreParticipantAndReserveProgramsOperations;
    private readonly ICharacterHuaTuoHappyGodCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterHuaTuoHappyGodCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterHuaTuoHappyGodCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterHuaTuoHappyGodCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICharacterHuaTuoHappyGodCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICharacterHuaTuoHappyGodCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterHuaTuoHappyGodCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterHuaTuoHappyGodCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterHuaTuoHappyGodKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterHuaTuoHappyGodRuntime(ICharacterHuaTuoHappyGodContentRegistryOperations contentRegistryOperations, ICharacterHuaTuoHappyGodResolutionStackOperations resolutionStackOperations, ICharacterHuaTuoHappyGodCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterHuaTuoHappyGodCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterHuaTuoHappyGodCoreParticipantAndReserveProgramsOperations coreParticipantAndReserveProgramsOperations, ICharacterHuaTuoHappyGodCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterHuaTuoHappyGodCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterHuaTuoHappyGodCoreRuntimeOperations coreRuntimeOperations, ICharacterHuaTuoHappyGodCoreTableOrderOperations coreTableOrderOperations, ICharacterHuaTuoHappyGodCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICharacterHuaTuoHappyGodCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterHuaTuoHappyGodCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterHuaTuoHappyGodCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterHuaTuoHappyGodKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreParticipantAndReserveProgramsOperations = coreParticipantAndReserveProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
