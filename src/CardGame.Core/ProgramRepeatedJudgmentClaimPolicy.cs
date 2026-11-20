using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum SkillProgramClaimHandLimitExemption { ActualTurn = 0, None = 1 }
public sealed record ProgramRepeatedJudgmentFinalOutcome(long JudgmentFrameId, int CardId, CardKind CardKind, Suit FinalSuit, bool Matched)
{
    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal long? EntrySequence { get; init; }
}
public sealed record ProgramRepeatedJudgmentClaimTurn(int TurnNumber, int TurnOwnerSeat);
public sealed record ProgramRepeatedJudgmentClaimReceipt(int InstructionIndex, long JudgmentFrameId, int CardId, int MovementSequence,
    CardLocation From, CardLocation To, int TurnNumber, int TurnOwnerSeat,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] long? GrantSequence);
public sealed record TurnHandLimitExemptCardGrant(long GrantSequence, int TurnNumber, int TurnSeat,
    long ParentFrameId, int EffectIndex, long JudgmentFrameId, CardUseEffectSource Source,
    int BeneficiarySeat, int CardId, int MovementSequence);
public sealed record TurnHandLimitExemptCardGrantedEvent(TurnHandLimitExemptCardGrant Grant) : IGameEvent;

internal interface IRepeatedJudgmentClaimPolicyProgramHost
{
    SkillProgramStepOutcome RepeatJudgmentWithClaimPolicy(ProgramSkillFrame frame, string reason, string bind,
        IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy);
}

internal static class NativeHandLimitExemptionRelations
{
    internal const string JudgmentEntities = "native:turn-hand-entities";
    internal const string CardKinds = "native:turn-hand-kinds";
}
