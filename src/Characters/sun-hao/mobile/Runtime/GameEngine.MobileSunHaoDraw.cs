using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MobileSunHaoDrawIssuedEvent(long FrameId, CardConversionSource Source, string Hash, int ActualTurn, int TurnOwner, long WindowId, int X) : IGameEvent;
public sealed record MobileSunHaoDrawReturnedEvent(long FrameId, long? AttemptId, long Before, long After, int Drawn, int Gained) : IGameEvent;
public sealed record MobileSunHaoDrawReceipt(CardConversionSource Source, string Hash, int ActualTurn, int TurnOwner, long WindowId, int X,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] NativeDrawOutcome? Outcome = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileSunHaoDrawReceipt? MobileSunHaoDraw { get; init; }
}
