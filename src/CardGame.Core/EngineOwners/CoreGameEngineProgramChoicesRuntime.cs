#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramChoicesRuntime
{
    private readonly RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries _characterPanZhangMaZhongBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterXuChuBoundaryMobileEntries _characterXuChuBoundaryMobileOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheS5XuchuAiEntries _coreShanheS5XuchuAiOperations;
    private readonly RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramChoicesProgramPaidCardOperations _programPaidCardOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramChoicesRuntime(RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries characterPanZhangMaZhongBoundaryMobileOperations, RuntimeEntries.CharacterXuChuBoundaryMobileEntries characterXuChuBoundaryMobileOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, RuntimeEntries.CoreGameEngineShanheS5XuchuAiEntries coreShanheS5XuchuAiOperations, RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreProgramChoicesProgramPaidCardOperations programPaidCardOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterPanZhangMaZhongBoundaryMobileOperations = characterPanZhangMaZhongBoundaryMobileOperations;
        this._characterXuChuBoundaryMobileOperations = characterXuChuBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreShanheS5XuchuAiOperations = coreShanheS5XuchuAiOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
