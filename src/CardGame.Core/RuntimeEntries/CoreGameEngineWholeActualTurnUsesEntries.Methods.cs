#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWholeActualTurnUsesEntries
{
    internal int? CapturedWholeActualTurnActorUseOrdinal(global::CardGame.Core.CardActionContext action) => _resolve().CapturedWholeActualTurnActorUseOrdinal(action);
    internal int? CapturedWholeActualTurnTrickUseOrdinal(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CapturedWholeActualTurnTrickUseOrdinal(attack);
    internal int? CapturedWholeActualTurnUsePrefixOrdinal(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent current, int expectedActorSeat, global::CardGame.Core.SkillProgramCardCategory? category) => _resolve().CapturedWholeActualTurnUsePrefixOrdinal(current, expectedActorSeat, category);
    internal global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent[] WholeActualTurnUseHistory() => _resolve().WholeActualTurnUseHistory();
    internal bool HasOngoingWholeActualUseTurn { get => _resolve().HasOngoingWholeActualUseTurn; }
    internal bool TracksWholeActualTurnUseOrdinal { get => _resolve().TracksWholeActualTurnUseOrdinal; }
}
