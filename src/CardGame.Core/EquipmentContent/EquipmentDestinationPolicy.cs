namespace CardGame.Core;

/// <summary>Trusted creation identity reconstructed from the existing creation facts, never from player commands.</summary>
public sealed record EquipmentCardOrigin(int CardId, string SourceId, int? InitialOwnerSeat = null);

/// <summary>Movement facts supplied by the owning card store. This is a read-only query, not additional pending state.</summary>
public sealed record EquipmentDestinationContext(Card Card, CardLocation From, CardLocation To,
    EquipmentCardOrigin? Origin = null, bool HasPriorMovement = false, bool AlreadyDestroyed = false);

/// <summary>A source-specific lifetime. Ordinary and purchased entities of the same card kind may differ.</summary>
public sealed record EquipmentDestinationPolicy(string SourceId, bool DestroyOnEquipmentExit = false,
    bool RestrictInitialPlacementToOwnerEquipment = false, CardKind? RequiredEquipmentKind = null)
{
    public CardLocation Normalize(EquipmentDestinationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Origin is not { } origin || origin.SourceId != SourceId) return context.To;
        if (origin.CardId != context.Card.Id || origin.InitialOwnerSeat is < 0 ||
            RequiredEquipmentKind is { } requiredKind && context.Card.Kind != requiredKind)
            throw new InvalidOperationException("The equipment creation identity does not match this physical card.");
        if (context.From == context.To) return context.To;
        if (context.From == CardLocation.OutsideGame && context.AlreadyDestroyed) return CardLocation.OutsideGame;
        if (DestroyOnEquipmentExit && context.From.Zone == CardZoneKind.Equipment) return CardLocation.OutsideGame;
        if (!RestrictInitialPlacementToOwnerEquipment || context.From != CardLocation.OutsideGame) return context.To;
        if (origin.InitialOwnerSeat is not { } owner)
            throw new InvalidOperationException("Restricted equipment creation requires its original owner seat.");
        return !context.HasPriorMovement && context.To == CardLocation.Equipment(owner)
            ? context.To : CardLocation.OutsideGame;
    }
}
