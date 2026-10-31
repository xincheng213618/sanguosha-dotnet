using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed partial record ProgramLifecycleTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AnniversaryCangmingActualStart { get; init; }
}
public sealed record AnniversaryMingActualStartIssuedEvent(long WindowId, int ActualTurn, int OwnerSeat, string SkillInstanceId, string GameplayHash) : IGameEvent;
public sealed record AnniversaryMingActualStartReturnedEvent(long WindowId, int ActualTurn, int OwnerSeat) : IGameEvent;
