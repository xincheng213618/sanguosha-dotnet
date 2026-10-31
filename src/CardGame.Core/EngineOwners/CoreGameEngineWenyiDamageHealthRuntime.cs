#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenyiDamageHealthRuntime
{
    private readonly RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageArmorPaymentsEntries _coreDamageArmorPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreNativeDamageCompletionEntries _coreNativeDamageCompletionOperations;
    private readonly RuntimeEntries.CoreNativeDamageWindowsEntries _coreNativeDamageWindowsOperations;
    private readonly RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreGameEngineWenyiDamageHealthRuntime(RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineDamageArmorPaymentsEntries coreDamageArmorPaymentsOperations,RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreNativeDamageCompletionEntries coreNativeDamageCompletionOperations,RuntimeEntries.CoreNativeDamageWindowsEntries coreNativeDamageWindowsOperations,RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageArmorPaymentsOperations = coreDamageArmorPaymentsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeDamageCompletionOperations = coreNativeDamageCompletionOperations;
        this._coreNativeDamageWindowsOperations = coreNativeDamageWindowsOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
