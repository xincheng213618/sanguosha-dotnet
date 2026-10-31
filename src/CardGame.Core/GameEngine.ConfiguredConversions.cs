namespace CardGame.Core;
public sealed record ConfiguredConversionTierChangedEvent(int OwnerSeat, string StateId, int Tier) : IGameEvent;
public sealed record PhysicalCardNameDeclaredEvent(int OwnerSeat, int DeclarerSeat, string StateId, int CardId, CardKind OutputKind, int TurnNumber) : IGameEvent;
