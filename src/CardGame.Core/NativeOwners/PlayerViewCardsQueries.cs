#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct PlayerViewCardsQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries> getCharacterCaoZhiLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeFireAttackEntries> getCoreNativeFireAttackRuntime) : IPlayerViewCardsQueries
{
    public CardSnapshot ProjectFuleHandCard(CharacterState player, Card card, CardSnapshot snapshot) => getCharacterCaoZhiLeRuntime().ProjectFuleHandCard(player, card, snapshot);
    public PublicPersistentPileSource? SinglePublicPileSource(int seat) => getCoreGameEnginePublicPileSourcesRuntime().SinglePublicPileSource(seat);
    public IReadOnlyList<Card> PublicPileCards(PublicPersistentPileSource source) => getCoreGameEnginePublicPileSourcesRuntime().PublicPileCards(source);
    public IReadOnlyList<Card> PublicPileDraftCards(ProgramPublicPileDraft draft) => getCoreGameEnginePublicPileSourcesRuntime().PublicPileDraftCards(draft);
    public IReadOnlyList<PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance) => getCoreGameEnginePublicPileSourcesRuntime().ReferencedPublicPileSources(seat, skill, consumerInstance);
    public Card GetFireAttackRevealedCard(CardUseFrame frame) => getCoreNativeFireAttackRuntime().GetFireAttackRevealedCard(frame);
}
