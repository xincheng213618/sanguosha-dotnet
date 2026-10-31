namespace CardGame.Core;

public enum RecoveryAttemptProducer
{
    Program, ProgramMaximum, SilverLion, CardUse, GroupCard, VirtualBasic,
    DyingBoundAlcohol, DyingVirtualAlcohol, Hengye,
    SkillInvocation = 38700
}

public sealed record RecoveryAttemptCompletion(RecoveryAttemptProducer Producer,
    int InstructionIndex = 0, int? CardId = null, CardKind? CardKind = null,
    CardMoveReason? MoveReason = null, long? ProgramFrameId = null,
    long? DyingFrameId = null, string? SkillId = null, int? SkillOwnerSeat = null,
    IReadOnlyList<ProgramRecoveryPolicySource>? Policies = null);

public sealed record RecoveryReplacementCandidate(int OwnerSeat, string SkillId,
    string SkillInstanceId, string PolicyId, int RecoveryAmount, int ProviderDrawCount);

/// <summary>A produced recovery belongs to its exact live rules frame, before any HP delta.</summary>
public sealed partial record RecoveryAttempt(long Id, int SourceSeat, int TargetSeat, int Amount,
    int? HpBefore, int ActualTurnNumber, int ActualTurnOwnerSeat,
    IReadOnlyList<RecoveryReplacementCandidate> Candidates, RecoveryAttemptCompletion Completion)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoDualHealth? HappyQiaoBefore { get; init; }
}

public enum RecoveryReplacementStage { Choosing, RecoveryApplied, RewardApplied }

public sealed record RecoveryReplacementReturn(PostEventContinuation Continuation,
    long? ResumeFrameId = null, int? CardId = null, CardKind? CardKind = null);

public sealed record RecoveryRewardMovementReceipt(long SequenceBefore, long SequenceAfter)
{
    private IReadOnlyList<CardMovementBatchContext> _batches = Array.AsReadOnly(Array.Empty<CardMovementBatchContext>());
    public IReadOnlyList<CardMovementBatchContext> Batches
    {
        get => _batches;
        init => _batches = Array.AsReadOnly(value.Select(batch => batch with
        {
            Movements = Array.AsReadOnly(batch.Movements.ToArray()),
            SourceCounts = Array.AsReadOnly(batch.SourceCounts.ToArray()),
            DestinationCounts = batch.DestinationCounts is null ? null : Array.AsReadOnly(batch.DestinationCounts.ToArray())
        }).ToArray());
    }
}

public sealed record PreparedRecoveryApplication(int SourceSeat, int TargetSeat, int RequestedAmount, int? CandidateIndex);

public sealed partial record RecoveryReplacementFrame(long Id, long ParentFrameId, RecoveryAttempt Attempt,
    RecoveryReplacementReturn Return, RecoveryReplacementStage Stage = RecoveryReplacementStage.Choosing,
    int? CandidateIndex = null, int OriginalRecovered = 0,
    ResolutionFrameStep Step = ResolutionFrameStep.ResolvingEffect)
    : ResolutionFrame(Id, ResolutionFrameKind.RecoveryReplacement, Step)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public RecoveryRewardMovementReceipt? RewardMovementReceipt { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public PreparedRecoveryApplication? PendingApplication { get; init; }
}

public sealed record RecoveryReplacementChosenEvent(long RecoveryFrameId, int RecoveringSeat,
    int BeneficiarySeat, string SkillId, string SkillInstanceId, string PolicyId,
    int ReplacedAmount, int RecoveryAmount) : IGameEvent;

public enum RecoveryPaidCardUseKind { Trick, Simple, CommittedSlash, SlashTarget, AttackDamage, EquipmentUse, DodgeCompletion = 6 }

public sealed partial record RecoveryPaidCardUseContinuation(RecoveryPaidCardUseKind Kind, int SourceSeat,
    int? CardId = null, IReadOnlyList<int>? TargetSeats = null, LegalActionKind? ActionKind = null,
    int? TargetCardId = null, CardKind? RequiredCardKind = null, CardKind? PlayedCardKind = null,
    ProgramSimpleCardContinuation? Simple = null, int? ReplacedCardId = null, EquipmentSlot? EquipmentSlot = null);
