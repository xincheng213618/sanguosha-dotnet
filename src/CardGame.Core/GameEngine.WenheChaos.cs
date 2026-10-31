using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record WenheChaosReceipt(long ParentFrameId, long? CardActionId, int RoundNumber, int TurnNumber)
{
    private readonly IReadOnlyList<int> _actors = Array.Empty<int>();
    public IReadOnlyList<int> ActorSeats { get => _actors; init => _actors = Array.AsReadOnly(value.ToArray()); }
}
public sealed record WenheChaosIssuedEvent(long ProgramFrameId, long ParentFrameId, long? CardActionId,
    int OwnerSeat, string SkillInstanceId, string GameplayHash, int RoundNumber, int TurnNumber, int ActorCount) : IGameEvent;
public sealed record WenheChaosActorEvent(long ProgramFrameId, int Index, int Seat) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenheChaosReceipt? WenheChaos { get; init; }
}
