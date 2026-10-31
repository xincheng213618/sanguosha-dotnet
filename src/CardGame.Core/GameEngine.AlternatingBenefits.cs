namespace CardGame.Core;
public sealed record AlternatingChoiceBenefitResolvedEvent(int OwnerSeat,string SkillId,string SkillInstanceId,string StateId,string OptionId,int Amount,string? PendingOptionId) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? OwnerTurnOrdinal { get; init; }
}
public sealed record AlternatingChoiceDrawBenefitIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardUseEffectSource Source, string GameplayHash, string StateId, int TurnNumber, int OwnerTurnOrdinal, int Amount) : IGameEvent;
public sealed record AlternatingChoiceDrawPhaseAssignedEvent(long ProgramFrameId, int InstructionIndex,
    ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
public sealed record ConsecutiveTargetDeckGiftEvent(int OwnerSeat,string SkillId,string SkillInstanceId,string StateId,int TargetSeat,int OwnerTurnOrdinal,int? CardId,bool Consecutive) : IGameEvent;
public sealed partial class GameEngine
{
}
