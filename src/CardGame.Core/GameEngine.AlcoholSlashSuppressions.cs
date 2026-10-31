namespace CardGame.Core;
// These public receipts contain no physical card IDs or private hand state.
public sealed record CardUseAlcoholConsumptionCapturedEvent(long CardUseFrameId, long ActionId, int ActorSeat) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? AlcoholBonus { get; init; }
}
