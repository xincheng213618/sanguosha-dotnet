#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileSourcesEntries
{
    internal global::System.Collections.Generic.IReadOnlyDictionary<string, int>? CapturePublicPileProgramCounts(global::CardGame.Core.CharacterState owner) => _resolve().CapturePublicPileProgramCounts(owner);
    internal global::CardGame.Core.PublicPersistentPileSnapshot CreatePublicPersistentPileSnapshot(int seat, global::CardGame.Core.PublicPersistentPileSource source) => _resolve().CreatePublicPersistentPileSnapshot(seat, source);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSnapshot>? CreatePublicPersistentPileSnapshots(int seat) => _resolve().CreatePublicPersistentPileSnapshots(seat);
    internal global::CardGame.Core.PublicPersistentPileSource EnsurePublicPileSource(global::CardGame.Core.ProgramSkillFrame frame, int capacity) => _resolve().EnsurePublicPileSource(frame, capacity);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source) => _resolve().PublicPileCards(source);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards2(int seat) => _resolve().PublicPileCards(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileDraftCards(global::CardGame.Core.ProgramPublicPileDraft draft) => _resolve().PublicPileDraftCards(draft);
    internal global::CardGame.Core.CardLocation PublicPileDraftLocation(global::CardGame.Core.ProgramPublicPileDraft draft) => _resolve().PublicPileDraftLocation(draft);
    internal global::CardGame.Core.PublicPersistentPileSource? PublicPileDraftSource(global::CardGame.Core.ProgramPublicPileDraft draft) => _resolve().PublicPileDraftSource(draft);
    internal int PublicPileProgramCount(int seat, string skill, string instance) => _resolve().PublicPileProgramCount(seat, skill, instance);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> PublicPileSources(int seat) => _resolve().PublicPileSources(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance = default) => _resolve().ReferencedPublicPileSources(seat, skill, consumerInstance);
    internal global::CardGame.Core.PublicPersistentPileSource? SinglePublicPileSource(int seat) => _resolve().SinglePublicPileSource(seat);
    internal bool SupportsMultiplePublicPiles { get => _resolve().SupportsMultiplePublicPiles; }
}
