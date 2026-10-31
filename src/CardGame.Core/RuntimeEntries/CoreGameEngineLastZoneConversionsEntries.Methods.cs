#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastZoneConversionsEntries
{
    internal global::CardGame.Core.Card? FindLastZoneJudgmentConversionCard(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().FindLastZoneJudgmentConversionCard(owner, cardId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse) => _resolve().GetLastZoneJudgmentConversionCards(owner, forResponse);
    internal bool IsLastZoneJudgmentCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsLastZoneJudgmentCard(owner, card);
    internal bool MatchesLastSourceZoneConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.SkillProgramViewAs rule) => _resolve().MatchesLastSourceZoneConversion(owner, card, rule);
    internal bool TryGetLastZoneJudgmentConversionLocation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, scoped out global::CardGame.Core.CardLocation location) => _resolve().TryGetLastZoneJudgmentConversionLocation(owner, card, out location);
    internal void ValidateLastZoneConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse) => _resolve().ValidateLastZoneConversionCommit(source, provider, materials, forResponse);
}
