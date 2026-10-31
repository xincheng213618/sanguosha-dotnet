using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ProgramJudgmentOutcomeClaimReceipt(long JudgmentFrameId, long RootProgramFrameId,
    string OutcomeBind, int RootInstructionIndex, long ClaimProgramFrameId, string SkillId, string BindingId,
    string SkillInstanceId, string GameplayHash, int ClaimInstructionIndex, int OwnerSeat, int CardId, int MovementSequence);

public sealed record ProgramJudgmentOutcomeClaimRecordedEvent(ProgramJudgmentOutcomeClaimReceipt Receipt) : IGameEvent;

public sealed partial record JudgmentFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramJudgmentOutcomeClaimReceipt? OutcomeClaim { get; init; }
}
