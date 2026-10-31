using System.Text.Json.Serialization;

namespace CardGame.Core;

public readonly record struct ActualTurnPreludeContext(int OwnerSeat, int TurnNumber);

public abstract partial record ResolutionFrame
{
    [JsonIgnore]
    public virtual ActualTurnPreludeContext? UpcomingTurnPrelude => null;
}
