#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileSourcesEntries
{
    internal IReadOnlyDictionary<string, int>? CapturePublicPileProgramCounts(CharacterState owner) => _resolve().CapturePublicPileProgramCounts(owner);
    internal PublicPersistentPileSnapshot CreatePublicPersistentPileSnapshot(int seat, PublicPersistentPileSource source) => _resolve().CreatePublicPersistentPileSnapshot(seat, source);
    internal IReadOnlyList<PublicPersistentPileSnapshot>? CreatePublicPersistentPileSnapshots(int seat) => _resolve().CreatePublicPersistentPileSnapshots(seat);
    internal PublicPersistentPileSource EnsurePublicPileSource(ProgramSkillFrame frame, int capacity) => _resolve().EnsurePublicPileSource(frame, capacity);
    internal IReadOnlyList<Card> PublicPileCards(PublicPersistentPileSource source) => _resolve().PublicPileCards(source);
    internal IReadOnlyList<Card> PublicPileCards2(int seat) => _resolve().PublicPileCards(seat);
    internal IReadOnlyList<Card> PublicPileDraftCards(ProgramPublicPileDraft draft) => _resolve().PublicPileDraftCards(draft);
    internal CardLocation PublicPileDraftLocation(ProgramPublicPileDraft draft) => _resolve().PublicPileDraftLocation(draft);
    internal PublicPersistentPileSource? PublicPileDraftSource(ProgramPublicPileDraft draft) => _resolve().PublicPileDraftSource(draft);
    internal int PublicPileProgramCount(int seat, string skill, string instance) => _resolve().PublicPileProgramCount(seat, skill, instance);
    internal IEnumerable<PublicPersistentPileSource> PublicPileSources(int seat) => _resolve().PublicPileSources(seat);
    internal IReadOnlyList<PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance = default) => _resolve().ReferencedPublicPileSources(seat, skill, consumerInstance);
    internal PublicPersistentPileSource? SinglePublicPileSource(int seat) => _resolve().SinglePublicPileSource(seat);
    internal bool SupportsMultiplePublicPiles { get => _resolve().SupportsMultiplePublicPiles; }
}
