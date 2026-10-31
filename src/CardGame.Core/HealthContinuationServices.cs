namespace CardGame.Core;

// Indexed facts retain the original first/last search and stack traversal order.
// No mutable frame, movement or HP-journal collection crosses this boundary.
internal interface IHealthContinuationFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    int FrameIndex(long id);
    int PlayerCount { get; }
    bool IsValidPlayerSeat(int seat);
    int MovementCount { get; }
    CardMovementRecord MovementAt(int index);
    int PendingHpChangeCount { get; }
    HpChangeContext PendingHpChangeAt(int index);
    DyingFrame? ActiveDying { get; }
    DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
    SkillProgram? Program(string skillId);
    SkillProgram RequireProgram(string skillId);
    IEnumerable<IGameEvent> CompleteProgramEventHistory();
    IReadOnlyList<T> ProgramEventHistory<T>() where T : IGameEvent;
    int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference);
}

// These are existing neighboring protocols: native payment/response returns,
// ordinary post-event edges, and recovery amount support. Cursor ownership and
// recovery receipts are evaluated by HealthContinuationRuntime itself.
internal interface IHealthContinuationProtocols : IProgramRuntimeOperations
{
    bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame payment);
    bool HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext action);
    bool ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame window);
    bool IsResponseCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child);
    bool IsWenhePoisonDyingChild(ResolutionFrame? parent, DyingFrame child);
    bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child);
    bool TypedDamageObserverCandidateMatches(ProgramSkillFrame observer, ProgramTriggerCandidate candidate);
    int SupportedRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount);
    int SupportedRecoveryHpBefore(RecoveryReplacementFrame recovery, int fallback);
    bool IsRecoveryParent(ProgramRecoveryParentPhase phase, ProgramSkillFrame parent, RecoveryReplacementFrame child);
    bool NativeCardRecoveryReturnMatches(CardUseFrame use, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history);
    bool ExactPaidNativeRecoveryUseEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history);
    bool ExactPaidNativeRecoveryReplacementEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history);
    bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window);
    bool IsAmountSupportHealthChild(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp);
}

public sealed partial class GameEngine
{
}
