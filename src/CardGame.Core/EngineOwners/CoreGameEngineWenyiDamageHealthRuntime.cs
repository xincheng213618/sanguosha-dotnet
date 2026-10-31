#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenyiDamageHealthRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageArmorPaymentsEntries _coreDamageArmorPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageCompletionEntries _coreNativeDamageCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries _coreNativeDamageWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreGameEngineWenyiDamageHealthRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageArmorPaymentsEntries coreDamageArmorPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageCompletionEntries coreNativeDamageCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries coreNativeDamageWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
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
