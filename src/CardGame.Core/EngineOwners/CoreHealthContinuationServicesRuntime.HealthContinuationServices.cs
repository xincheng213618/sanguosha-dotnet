#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHealthContinuationServicesRuntime
{
    int global::CardGame.Core.IHealthContinuationFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame global::CardGame.Core.IHealthContinuationFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int global::CardGame.Core.IHealthContinuationFacts.FrameIndex(long id) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
    int global::CardGame.Core.IHealthContinuationFacts.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool global::CardGame.Core.IHealthContinuationFacts.IsValidPlayerSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    int global::CardGame.Core.IHealthContinuationFacts.MovementCount => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count;

    CardMovementRecord global::CardGame.Core.IHealthContinuationFacts.MovementAt(int index) => _coreCardMovementJournalCardMovementsAccess._cardMovements[index];
    int global::CardGame.Core.IHealthContinuationFacts.PendingHpChangeCount => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Count;

    HpChangeContext global::CardGame.Core.IHealthContinuationFacts.PendingHpChangeAt(int index) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges[index];
    DyingFrame? global::CardGame.Core.IHealthContinuationFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    DamageTriggerWindowFrame? global::CardGame.Core.IHealthContinuationFacts.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    SkillProgram? global::CardGame.Core.IHealthContinuationFacts.Program(string skillId) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(skillId, out var skill) ? skill.Program : null;
    SkillProgram global::CardGame.Core.IHealthContinuationFacts.RequireProgram(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program!;
    IEnumerable<IGameEvent> global::CardGame.Core.IHealthContinuationFacts.CompleteProgramEventHistory() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
    IReadOnlyList<T> global::CardGame.Core.IHealthContinuationFacts.ProgramEventHistory<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    int global::CardGame.Core.IHealthContinuationFacts.ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    bool global::CardGame.Core.IHealthContinuationProtocols.IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame payment) => _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, payment);
    bool global::CardGame.Core.IHealthContinuationProtocols.HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext action) => _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action);
    bool global::CardGame.Core.IHealthContinuationProtocols.ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame window) => _coreResponseCompletionOperations.ValidResponseCompletionWindow(window);
    bool global::CardGame.Core.IHealthContinuationProtocols.IsResponseCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _coreResponseCompletionHealthOperations.IsResponseCompletionHealthChild(window, child);
    bool global::CardGame.Core.IHealthContinuationProtocols.IsWenhePoisonDyingChild(ResolutionFrame? parent, DyingFrame child) => _coreWenhePoisonOperations.IsWenhePoisonDyingChild(parent, child);
    bool global::CardGame.Core.IHealthContinuationProtocols.TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child);
    bool global::CardGame.Core.IHealthContinuationProtocols.TypedDamageObserverCandidateMatches(ProgramSkillFrame observer, ProgramTriggerCandidate candidate) => _coreDamageObserverCursorQueriesOperations.TypedDamageObserverCandidateMatches(observer, candidate);
    void global::CardGame.Core.IHealthContinuationProtocols.AssertEquipmentPlacementAndHpPair(ProgramSkillFrame producer, ProgramExecutionPlan plan) => _coreEquipmentPlacementAndHpPairOperations.AssertEquipmentPlacementAndHpPair(producer, plan);
    int global::CardGame.Core.IHealthContinuationProtocols.SupportedRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount) => _coreRecoveryAmountBonusesOperations.SupportedRecoveryAmount(recovery, baseAmount);
    int global::CardGame.Core.IHealthContinuationProtocols.SupportedRecoveryHpBefore(RecoveryReplacementFrame recovery, int fallback) => _characterCaoSongOlOperations.SupportedRecoveryHpBefore(recovery, fallback);
    bool global::CardGame.Core.IHealthContinuationProtocols.IsRecoveryParent(ProgramRecoveryParentPhase phase, ProgramSkillFrame parent, RecoveryReplacementFrame child) => _coreProgramRuntimeHooksOperations.RuntimeHooks.IsRecoveryParent(phase, parent, child);
    bool global::CardGame.Core.IHealthContinuationProtocols.NativeCardRecoveryReturnMatches(CardUseFrame use, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.NativeCardRecoveryReturnMatches(use, hp, history);
    bool global::CardGame.Core.IHealthContinuationProtocols.ExactPaidNativeRecoveryUseEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.ExactPaidNativeRecoveryUseEdge(parent, child, history);
    bool global::CardGame.Core.IHealthContinuationProtocols.ExactPaidNativeRecoveryReplacementEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.ExactPaidNativeRecoveryReplacementEdge(parent, child, history);
    bool global::CardGame.Core.IHealthContinuationProtocols.DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, window);
    bool global::CardGame.Core.IHealthContinuationProtocols.IsAmountSupportHealthChild(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp) => _characterCaoSongOlOperations.IsAmountSupportHealthChild(recovery, hp);
}
