namespace CardGame.Core;

public sealed record WushuangJinzhiPaidEvent(long FrameId, int Owner, int RoundNumber, int Ordinal,
    int ActualTurn, int MovementBefore, CardActionContext Action) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public long? MaterialPaymentFrameId { get; init; }
}

public sealed record WushuangJinzhiMaterialSelection(int RequiredCount, IReadOnlyList<int> CardIds);
