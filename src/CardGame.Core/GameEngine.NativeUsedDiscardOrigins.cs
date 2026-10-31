namespace CardGame.Core;

public enum NativeTrueUseDiscardKind { Use, Dodge, Nullification }

// A payment fact, not a pending response or a second accepted action. Native
// counterspells create their response action after both physical moves, so the
// original native parent must identify that payment before its cleanup runs.
// A successful declaration can transfer its already entered entity to a new
// native Use. Its real claim binds that same entry sequence without a new move.
public sealed record NativeTrueUseProcessingEntryEvent(long ParentFrameId, long ParentActionId,
    int CardId, CardKind CardKind, CardLocation From, long EntrySequence, int ProviderSeat,
    NativeTrueUseDiscardKind Kind) : IGameEvent;
