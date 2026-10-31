#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHealthContinuationServicesRuntime
{
    int IHealthContinuationFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame IHealthContinuationFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int IHealthContinuationFacts.FrameIndex(long id) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
    int IHealthContinuationFacts.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool IHealthContinuationFacts.IsValidPlayerSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    int IHealthContinuationFacts.MovementCount => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count;

    CardMovementRecord IHealthContinuationFacts.MovementAt(int index) => _coreCardMovementJournalCardMovementsAccess._cardMovements[index];
    int IHealthContinuationFacts.PendingHpChangeCount => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Count;

    HpChangeContext IHealthContinuationFacts.PendingHpChangeAt(int index) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges[index];
    DyingFrame? IHealthContinuationFacts.ActiveDying => _coreDyingCursorRuntimeOperations.Value.ActiveDying;

    DamageTriggerWindowFrame? IHealthContinuationFacts.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger;

    SkillProgram? IHealthContinuationFacts.Program(string skillId) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(skillId, out var skill) ? skill.Program : null;
    SkillProgram IHealthContinuationFacts.RequireProgram(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program!;
    IEnumerable<IGameEvent> IHealthContinuationFacts.CompleteProgramEventHistory() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
    IReadOnlyList<T> IHealthContinuationFacts.ProgramEventHistory<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    int IHealthContinuationFacts.ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    bool IHealthContinuationProtocols.IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame payment) => _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, payment);
    bool IHealthContinuationProtocols.HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext action) => _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action);
    bool IHealthContinuationProtocols.ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame window) => _coreResponseCompletionOperations.ValidResponseCompletionWindow(window);
    bool IHealthContinuationProtocols.IsResponseCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _coreResponseCompletionHealthOperations.Value.IsResponseCompletionHealthChild(window, child);
    bool IHealthContinuationProtocols.IsWenhePoisonDyingChild(ResolutionFrame? parent, DyingFrame child) => _coreWenhePoisonOperations.IsWenhePoisonDyingChild(parent, child);
    bool IHealthContinuationProtocols.TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child);
    bool IHealthContinuationProtocols.TypedDamageObserverCandidateMatches(ProgramSkillFrame observer, ProgramTriggerCandidate candidate) => _coreDamageObserverCursorQueriesOperations.TypedDamageObserverCandidateMatches(observer, candidate);
    int IHealthContinuationProtocols.SupportedRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount) => _coreRecoveryAmountBonusesOperations.Value.SupportedRecoveryAmount(recovery, baseAmount);
    int IHealthContinuationProtocols.SupportedRecoveryHpBefore(RecoveryReplacementFrame recovery, int fallback) => _characterCaoSongOlOperations.SupportedRecoveryHpBefore(recovery, fallback);
    bool IHealthContinuationProtocols.IsRecoveryParent(ProgramRecoveryParentPhase phase, ProgramSkillFrame parent, RecoveryReplacementFrame child) => _coreProgramRuntimeHooksOperations.RuntimeHooks.IsRecoveryParent(phase, parent, child);
    bool IHealthContinuationProtocols.NativeCardRecoveryReturnMatches(CardUseFrame use, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.NativeCardRecoveryReturnMatches(use, hp, history);
    bool IHealthContinuationProtocols.ExactPaidNativeRecoveryUseEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.ExactPaidNativeRecoveryUseEdge(parent, child, history);
    bool IHealthContinuationProtocols.ExactPaidNativeRecoveryReplacementEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history) => _coreNativeRecoveryDamageOperations.ExactPaidNativeRecoveryReplacementEdge(parent, child, history);
    bool IHealthContinuationProtocols.DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, window);
    bool IHealthContinuationProtocols.IsAmountSupportHealthChild(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp) => _characterCaoSongOlOperations.IsAmountSupportHealthChild(recovery, hp);
}
