#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnPublicPileConversionsEntries
{
    internal global::CardGame.Core.Card? FindOwnPublicPileConversionCard(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().FindOwnPublicPileConversionCard(owner, cardId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner) => _resolve().GetOwnPublicPileConversionCards(owner);
    internal bool IsOwnPublicPileActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => _resolve().IsOwnPublicPileActionCost(action, cost);
    internal bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsOwnPublicPileCard(owner, card);
    internal bool IsOwnPublicPileSlashMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsOwnPublicPileSlashMaterial(owner, card);
    internal bool MatchesOwnPublicPileConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule) => _resolve().MatchesOwnPublicPileConversion(owner, card, instance, rule);
    internal global::CardGame.Core.Card? OwnPublicPileResponseMaterial(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source) => _resolve().OwnPublicPileResponseMaterial(costs, source);
    internal global::CardGame.Core.CardKind PhysicalSlashUseBaseKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().PhysicalSlashUseBaseKind(owner, card);
    internal void RefreshOwnPublicPileAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardUseFrame previous) => _resolve().RefreshOwnPublicPileAddedTargetCandidates(use, previous);
    internal void ValidateOwnPublicPileConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse, global::CardGame.Core.CardKind? effectiveKind = default) => _resolve().ValidateOwnPublicPileConversionCommit(source, provider, materials, forResponse, effectiveKind);
}
