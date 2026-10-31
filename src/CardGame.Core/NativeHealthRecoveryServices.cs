namespace CardGame.Core;

// Reads stay indexed so each validator preserves its original first/last frame
// search, short-circuit order and lazy event-history capture.
internal interface INativeHealthRecoveryFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    int FrameIndex(long id);
    int FrameLastIndex(long id);
    IEnumerable<IGameEvent> CompleteProgramEventHistory();
    int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference);
}

// Adjacent mechanisms retain their own exact receipt and return validators.
internal interface INativeHealthRecoveryProtocols : IProgramRuntimeOperations
{
    bool HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    void ValidateHulaoPeachUse(CardUseFrame use);
    bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history);
    bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child);
    bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window);
    bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame producer, RecoveryReplacementFrame recovery,
        IReadOnlyList<IGameEvent> history);
    bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp,
        IReadOnlyList<IGameEvent> history);
    bool ProgramRecoveryReceiptMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame hp,
        IReadOnlyList<IGameEvent> history);
    SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer);
}

public sealed partial class GameEngine
{
}
