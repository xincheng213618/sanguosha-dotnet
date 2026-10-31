namespace CardGame.Core;
public sealed record DuFuRenOwnedOriginEvent(long MovementSequence, int OwnerSeat, Suit Suit) : IGameEvent;
public sealed record DuFuRenTransferProducerEvent(long EntrySequence, long ProducerId, int OwnerSeat, int RecipientSeat) : IGameEvent;
