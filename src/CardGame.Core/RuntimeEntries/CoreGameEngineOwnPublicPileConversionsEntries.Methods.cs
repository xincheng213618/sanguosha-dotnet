#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnPublicPileConversionsEntries
{
    internal Card? FindOwnPublicPileConversionCard(CharacterState owner, int cardId) => _resolve().FindOwnPublicPileConversionCard(owner, cardId);
    internal IReadOnlyList<Card> GetOwnPublicPileConversionCards(CharacterState owner) => _resolve().GetOwnPublicPileConversionCards(owner);
    internal bool IsOwnPublicPileActionCost(CardActionContext action, CardActionCost cost) => _resolve().IsOwnPublicPileActionCost(action, cost);
    internal bool IsOwnPublicPileCard(CharacterState owner, Card card) => _resolve().IsOwnPublicPileCard(owner, card);
    internal bool IsOwnPublicPileSlashMaterial(CharacterState owner, Card card) => _resolve().IsOwnPublicPileSlashMaterial(owner, card);
    internal bool MatchesOwnPublicPileConversion(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule) => _resolve().MatchesOwnPublicPileConversion(owner, card, instance, rule);
    internal Card? OwnPublicPileResponseMaterial(IReadOnlyList<CardActionCost> costs, CardConversionSource? source) => _resolve().OwnPublicPileResponseMaterial(costs, source);
    internal CardKind PhysicalSlashUseBaseKind(CharacterState owner, Card card) => _resolve().PhysicalSlashUseBaseKind(owner, card);
    internal void RefreshOwnPublicPileAddedTargetCandidates(CardUseFrame use, CardUseFrame previous) => _resolve().RefreshOwnPublicPileAddedTargetCandidates(use, previous);
    internal void ValidateOwnPublicPileConversionCommit(CardConversionSource? source, CharacterState provider, IReadOnlyList<Card> materials, bool forResponse, CardKind? effectiveKind = default) => _resolve().ValidateOwnPublicPileConversionCommit(source, provider, materials, forResponse, effectiveKind);
}
