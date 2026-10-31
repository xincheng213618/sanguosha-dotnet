namespace CardGame.Core;

public sealed record ProgramSkillStartedEvent(long FrameId, int OwnerSeat, string SkillId, string ActivationId) : IGameEvent;
public sealed record ProgramSkillResolvedEvent(long FrameId, int OwnerSeat, string SkillId, string ActivationId, bool Completed) : IGameEvent;
public sealed record ProgramSkillHpLostEvent(long FrameId, string SkillId, int TargetSeat, int Amount, int RemainingHp) : IGameEvent
{
    /// <summary>The exact public card use whose completion triggered this loss; absent for unrelated or earlier payments.</summary>
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public long? CompletedCardUseResolutionId { get; init; }
}

public sealed record ProgramSkillContributionResolvedEvent(long ActionId, int ProviderSeat, int SkillOwnerSeat, string SkillId, string ContributionId, int CardId, CardKind CardKind, Suit CardSuit, [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] string? SkillInstanceId = null) : IGameEvent;
