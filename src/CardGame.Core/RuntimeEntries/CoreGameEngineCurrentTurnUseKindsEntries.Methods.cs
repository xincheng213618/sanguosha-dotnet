#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnUseKindsEntries
{
    internal int? CapturedActualTurnActorUseOrdinal(global::CardGame.Core.CardActionContext action) => _resolve().CapturedActualTurnActorUseOrdinal(action);
    internal int CurrentTurnCategoryDrawCount(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().CurrentTurnCategoryDrawCount(frame, targetSeat);
    internal bool CurrentTurnOwnerUsedSameSuitCards() => _resolve().CurrentTurnOwnerUsedSameSuitCards();
    internal (int HandSuits, int Categories) CurrentTurnUseKinds(int actorSeat) => _resolve().CurrentTurnUseKinds(actorSeat);
    internal bool IsActualTurnLegacyVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsActualTurnLegacyVirtualUse(use);
    internal void ObserveCurrentTurnUseKinds(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveCurrentTurnUseKinds(payload);
    internal void RecordCurrentTurnUseKinds(long frameId, int actorSeat, global::CardGame.Core.CardActionContext? action, global::CardGame.Core.CardKind effectiveKind, bool nullificationUse, int? programInstructionIndex = default) => _resolve().RecordCurrentTurnUseKinds(frameId, actorSeat, action, effectiveKind, nullificationUse, programInstructionIndex);
    internal bool HasOngoingActualUseTurn { get => _resolve().HasOngoingActualUseTurn; }
    internal bool TracksActualTurnUseOrdinal { get => _resolve().TracksActualTurnUseOrdinal; }
    internal bool TracksCurrentTurnUseKinds { get => _resolve().TracksCurrentTurnUseKinds; }
}
