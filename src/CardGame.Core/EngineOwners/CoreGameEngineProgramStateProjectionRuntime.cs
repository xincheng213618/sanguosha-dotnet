#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramStateProjectionRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGameActivationProgressEntries _coreGameActivationProgressOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess;
    internal CoreGameEngineProgramStateProjectionRuntime(global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGameActivationProgressEntries coreGameActivationProgressOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess)
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
