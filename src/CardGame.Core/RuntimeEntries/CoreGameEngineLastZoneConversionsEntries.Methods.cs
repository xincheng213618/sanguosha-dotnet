#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastZoneConversionsEntries
{
    internal Card? FindLastZoneJudgmentConversionCard(CharacterState owner, int cardId) => _resolve().FindLastZoneJudgmentConversionCard(owner, cardId);
    internal IReadOnlyList<Card> GetLastZoneJudgmentConversionCards(CharacterState owner, bool forResponse) => _resolve().GetLastZoneJudgmentConversionCards(owner, forResponse);
    internal bool IsLastZoneJudgmentCard(CharacterState owner, Card card) => _resolve().IsLastZoneJudgmentCard(owner, card);
    internal bool MatchesLastSourceZoneConversion(CharacterState owner, Card card, SkillProgramViewAs rule) => _resolve().MatchesLastSourceZoneConversion(owner, card, rule);
    internal bool TryGetLastZoneJudgmentConversionLocation(CharacterState owner, Card card, scoped out CardLocation location) => _resolve().TryGetLastZoneJudgmentConversionLocation(owner, card, out location);
    internal void ValidateLastZoneConversionCommit(CardConversionSource? source, CharacterState provider, IReadOnlyList<Card> materials, bool forResponse) => _resolve().ValidateLastZoneConversionCommit(source, provider, materials, forResponse);
}
