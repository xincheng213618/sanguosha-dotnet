#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSelectionAiRuntime : global::CardGame.Core.IProgramSelectionAiState, global::CardGame.Core.IProgramSelectionRewardState
{
    private readonly ICoreProgramSelectionAiCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramSelectionAiCoreNextActualUseTargetAdjustmentsOperations _coreNextActualUseTargetAdjustmentsOperations;
    private readonly ICoreProgramSelectionAiCorePindianPoliciesOperations _corePindianPoliciesOperations;
    private readonly ICoreProgramSelectionAiCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreProgramSelectionAiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramSelectionAiCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreProgramSelectionAiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramSelectionAiRuntime(ICoreProgramSelectionAiCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramSelectionAiCoreNextActualUseTargetAdjustmentsOperations coreNextActualUseTargetAdjustmentsOperations, ICoreProgramSelectionAiCorePindianPoliciesOperations corePindianPoliciesOperations, ICoreProgramSelectionAiCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreProgramSelectionAiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramSelectionAiCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreProgramSelectionAiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreNextActualUseTargetAdjustmentsOperations = coreNextActualUseTargetAdjustmentsOperations;
        this._corePindianPoliciesOperations = corePindianPoliciesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
