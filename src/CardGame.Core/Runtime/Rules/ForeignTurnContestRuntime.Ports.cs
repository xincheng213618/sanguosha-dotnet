namespace CardGame.Core;

internal interface IForeignTurnContestFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    bool HasHpValue(int seat);
    bool IsRuleWounded(int seat);
    bool HasSkillInstance(int seat, string skillId, string instanceId);
    bool HasAllowedSlashTargetTail(CardUseFrame use);
}

// Existing native windows, shared grant and binding completion remain the
// owners of those operations; this contract exposes no contest continuation.
internal interface IForeignTurnContestContinuations : IProgramRuntimeOperations
{
    bool BeginRecoveryReplacement(long frameId);
    bool BeginHpChanged(long frameId);
    void GrantSelfOnly(ProgramSkillFrame frame, int subjectSeat);
    void Finish(ProgramSkillFrame frame, bool completed);
}

// These are transient values, never a second pending object or receipt ledger.
// The only persisted Slash return is attached to the original frames.
internal sealed record ForeignTurnContestSlashIssue(ForeignTurnContestSlashReturn Return, CardActionContext Action);
internal sealed record ForeignTurnContestSlashCompletion(long? ProgramFrameId, long ResolutionId, ForeignTurnContestSlashReturn Return);

internal interface IForeignTurnContestSlashProducer : IProgramRuntimeOperations
{
    bool CanIssue(int actorSeat, int targetSeat);
    ForeignTurnContestSlashIssue Capture(long programFrameId, int instructionIndex,
        CardConversionSource source, ForeignTurnPindianOrigin origin);
    void Commit(ForeignTurnContestSlashIssue issue);
}

public sealed partial class GameEngine
{
}
