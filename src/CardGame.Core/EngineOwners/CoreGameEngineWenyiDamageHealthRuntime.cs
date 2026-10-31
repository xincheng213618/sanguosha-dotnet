#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenyiDamageHealthRuntime
{
    private readonly ICoreWenyiDamageHealthCharacterHuanShuJunOlOperations _characterHuanShuJunOlOperations;
    private readonly ICoreWenyiDamageHealthCardZonesOperations _cardZonesOperations;
    private readonly ICoreWenyiDamageHealthResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWenyiDamageHealthCoreDamageArmorPaymentsOperations _coreDamageArmorPaymentsOperations;
    private readonly ICoreWenyiDamageHealthCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreWenyiDamageHealthCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreWenyiDamageHealthCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreWenyiDamageHealthCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreWenyiDamageHealthCoreNativeDamageCompletionOperations _coreNativeDamageCompletionOperations;
    private readonly ICoreWenyiDamageHealthCoreNativeDamageWindowsOperations _coreNativeDamageWindowsOperations;
    private readonly ICoreWenyiDamageHealthCoreNativeDyingOperations _coreNativeDyingOperations;
    private readonly ICoreWenyiDamageHealthKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreGameEngineWenyiDamageHealthRuntime(ICoreWenyiDamageHealthCharacterHuanShuJunOlOperations characterHuanShuJunOlOperations, ICoreWenyiDamageHealthCardZonesOperations cardZonesOperations, ICoreWenyiDamageHealthResolutionStackOperations resolutionStackOperations, ICoreWenyiDamageHealthCoreDamageArmorPaymentsOperations coreDamageArmorPaymentsOperations, ICoreWenyiDamageHealthCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreWenyiDamageHealthCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreWenyiDamageHealthCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreWenyiDamageHealthCoreRuntimeOperations coreRuntimeOperations, ICoreWenyiDamageHealthCoreNativeDamageCompletionOperations coreNativeDamageCompletionOperations, ICoreWenyiDamageHealthCoreNativeDamageWindowsOperations coreNativeDamageWindowsOperations, ICoreWenyiDamageHealthCoreNativeDyingOperations coreNativeDyingOperations, ICoreWenyiDamageHealthKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._cardZonesOperations = cardZonesOperations;
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
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
