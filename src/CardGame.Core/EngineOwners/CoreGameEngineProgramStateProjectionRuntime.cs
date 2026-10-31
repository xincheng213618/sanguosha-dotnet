#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramStateProjectionRuntime
{
    private readonly IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineGameActivationProgressEntries _coreGameActivationProgressOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess;
    internal CoreGameEngineProgramStateProjectionRuntime(IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, RuntimeEntries.CoreGameEngineGameActivationProgressEntries coreGameActivationProgressOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess)
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
