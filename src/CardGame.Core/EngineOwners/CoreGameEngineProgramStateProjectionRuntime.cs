#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramStateProjectionRuntime
{
    private readonly ICoreProgramStateProjectionSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreProgramStateProjectionTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreProgramStateProjectionCoreGameActivationProgressOperations _coreGameActivationProgressOperations;
    private readonly ICoreProgramStateProjectionCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramStateProjectionCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICoreProgramStateProjectionCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess;
    internal CoreGameEngineProgramStateProjectionRuntime(ICoreProgramStateProjectionSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreProgramStateProjectionTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreProgramStateProjectionCoreGameActivationProgressOperations coreGameActivationProgressOperations, ICoreProgramStateProjectionCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramStateProjectionCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICoreProgramStateProjectionCoreSkillProgramsOperations coreSkillProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreGameActivationProgressOperations = coreGameActivationProgressOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess = coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess;
    }
}
