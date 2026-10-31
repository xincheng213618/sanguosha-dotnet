using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheWoxinCreditEarnedEvent(int Ordinal, CardUseEffectSource Source, string GameplayHash,
    long DamageFrameId, int ActualTurn, int TurnOwnerSeat, int ReceivedAmount) : IGameEvent;
public sealed record ShanheWoxinReservation(long Id, long AttackFrameId, CardUseEffectSource Source,
    string GameplayHash, int OriginalTargetSeat, int Amount);
public sealed record ShanheWoxinReservationIssuedEvent(ShanheWoxinReservation Receipt) : IGameEvent;
public sealed record ShanheWoxinCreditReservedEvent(long ReservationId, int CreditOrdinal) : IGameEvent;
public sealed record ShanheWoxinReservationConsumedEvent(long ReservationId, long DamageFrameId,
    int SourceSeat, int TargetSeat, int AppliedAmount) : IGameEvent;

public sealed partial record AttackAttemptState
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheWoxinReservation? ShanheWoxin { get; init; }
}
public sealed partial record CardAttackState
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheWoxinReservation? ShanheWoxin { get; init; }
}
