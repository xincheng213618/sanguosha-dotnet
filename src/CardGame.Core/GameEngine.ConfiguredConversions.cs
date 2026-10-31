namespace CardGame.Core;
public sealed record ConfiguredConversionTierChangedEvent(int OwnerSeat, string StateId, int Tier) : IGameEvent;
public sealed record PhysicalCardNameDeclaredEvent(int OwnerSeat, int DeclarerSeat, string StateId, int CardId, CardKind OutputKind, int TurnNumber) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), int> _configuredConversionTiers => ref _configuredConversions.ReferenceToConfiguredConversionTiers;
    private ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), (int Turn, int Card, global::CardGame.Core.CardKind Kind)> _declaredConversionCards => ref _configuredConversions.ReferenceToDeclaredConversionCards;
}
