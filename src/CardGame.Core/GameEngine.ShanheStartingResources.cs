namespace CardGame.Core;

// Trusted-host provenance. Player snapshots and the LAN public-event whitelist
// never include these hand identities or the campaign recipe index.
public sealed record ShanheStartingResourceCreatedEvent(int CardId, bool EquipmentResource, int RecipeIndex) : IGameEvent;
public sealed record ShanheStartingResourceUseEnteredEvent(int CardId, long UseFrameId, long ActionId, int EntrySequence) : IGameEvent;
public sealed record ShanheStartingResourceDestroyedEvent(int CardId, int MovementSequence, int EntrySequence,
    long ParentFrameId, long ActionId) : IGameEvent;
