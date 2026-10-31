using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record HappyGuardDeathRemoval(int Ordinal,int RemovedSeat,int GuardOwner,long ResolutionId);
public sealed partial record DeathFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingDefault)]public bool HappyGuardMarkerCleanupCompleted{get;init;}
    private readonly IReadOnlyList<HappyGuardDeathRemoval>? _happyGuardRemovals;
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public IReadOnlyList<HappyGuardDeathRemoval>? HappyGuardRemovals
    {get=>_happyGuardRemovals;init=>_happyGuardRemovals=value is null?null:Array.AsReadOnly(value.ToArray());}
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingDefault)]public int HappyGuardRemovalIndex{get;init;}
}
