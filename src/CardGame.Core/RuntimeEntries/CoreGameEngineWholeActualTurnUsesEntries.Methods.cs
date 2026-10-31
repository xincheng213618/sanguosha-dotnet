#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWholeActualTurnUsesEntries
{
    internal int? CapturedWholeActualTurnActorUseOrdinal(CardActionContext action) => _resolve().CapturedWholeActualTurnActorUseOrdinal(action);
    internal int? CapturedWholeActualTurnTrickUseOrdinal(GameEngine.IDamageAttempt attack) => _resolve().CapturedWholeActualTurnTrickUseOrdinal(attack);
    internal int? CapturedWholeActualTurnUsePrefixOrdinal(CurrentTurnCardUseKindsRecordedEvent current, int expectedActorSeat, SkillProgramCardCategory? category) => _resolve().CapturedWholeActualTurnUsePrefixOrdinal(current, expectedActorSeat, category);
    internal CurrentTurnCardUseKindsRecordedEvent[] WholeActualTurnUseHistory() => _resolve().WholeActualTurnUseHistory();
    internal bool HasOngoingWholeActualUseTurn { get => _resolve().HasOngoingWholeActualUseTurn; }
    internal bool TracksWholeActualTurnUseOrdinal { get => _resolve().TracksWholeActualTurnUseOrdinal; }
}
