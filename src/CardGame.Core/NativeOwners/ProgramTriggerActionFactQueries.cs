#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ProgramTriggerActionFactQueries(Func<RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime) : IProgramTriggerActionFactQueries
{
    public bool TracksActualTurnUseOrdinal => getCoreGameEngineCurrentTurnUseKindsRuntime().TracksActualTurnUseOrdinal;

    public int? CapturedActualTurnActorUseOrdinal(CardActionContext action) => getCoreGameEngineCurrentTurnUseKindsRuntime().CapturedActualTurnActorUseOrdinal(action);
    public bool TracksPlayCardHistory => getCoreGameEnginePlayCardHistoryRuntime().TracksPlayCardHistory;

    public bool MatchesPreviousPlayCard(CardActionContext action) => getCoreGameEnginePlayCardHistoryRuntime().MatchesPreviousPlayCard(action);
    public bool PreviousPlayCardIsBasic(CardActionContext action) => getCoreGameEnginePlayCardHistoryRuntime().PreviousPlayCardIsBasic(action);
}
