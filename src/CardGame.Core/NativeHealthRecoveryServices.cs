namespace CardGame.Core;

// Reads stay indexed so each validator preserves its original first/last frame
// search and stack traversal order.
internal interface INativeHealthRecoveryFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    int FrameIndex(long id);
    int FrameLastIndex(long id);
    int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference);
}

// Adjacent mechanisms retain their own exact receipt and return validators.
internal interface INativeHealthRecoveryProtocols : IProgramRuntimeOperations
{
    bool HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    void ValidateHulaoPeachUse(CardUseFrame use);
    bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery);
    bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child);
    bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window);
    bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame producer, RecoveryReplacementFrame recovery);
    bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp);
    bool ProgramRecoveryReceiptMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame hp);
    SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer);
}
