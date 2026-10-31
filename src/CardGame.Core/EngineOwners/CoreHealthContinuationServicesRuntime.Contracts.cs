#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHealthContinuationServicesCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreHealthContinuationServicesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreHealthContinuationServicesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHealthContinuationServicesCharacterCaoSongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int SupportedRecoveryHpBefore(global::CardGame.Core.RecoveryReplacementFrame recovery, int fallback);
    bool IsAmountSupportHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame hp);
}

internal interface ICoreHealthContinuationServicesCoreNativeRecoveryDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool NativeCardRecoveryReturnMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
    bool ExactPaidNativeRecoveryUseEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
    bool ExactPaidNativeRecoveryReplacementEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
}

internal interface ICoreHealthContinuationServicesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreHealthContinuationServicesCoreCompletedCategoryUseProofOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext completed);
}

internal interface ICoreHealthContinuationServicesCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreHealthContinuationServicesCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
}

internal interface ICoreHealthContinuationServicesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreHealthContinuationServicesCoreResponseCompletionHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHealthContinuationServicesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreHealthContinuationServicesCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreHealthContinuationServicesCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreHealthContinuationServicesCoreRecoveryAmountBonusesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int SupportedRecoveryAmount(global::CardGame.Core.RecoveryReplacementFrame recovery, int baseAmount);
}

internal interface ICoreHealthContinuationServicesCoreEquipmentPlacementAndHpPairOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertEquipmentPlacementAndHpPair(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreHealthContinuationServicesCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child);
}

internal interface ICoreHealthContinuationServicesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreHealthContinuationServicesCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool TypedDamageObserverCandidateMatches(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate);
    bool DamageProgramMatchesWindow(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DamageTriggerWindowFrame window);
}
