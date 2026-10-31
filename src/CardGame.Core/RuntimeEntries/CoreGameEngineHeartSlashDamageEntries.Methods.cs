#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHeartSlashDamageEntries
{
    internal bool MatchesOwnerUsedHeartSlash(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MatchesOwnerUsedHeartSlash(attack);
    internal global::CardGame.Core.Suit PublicProposedSlashSuit(global::CardGame.Core.GameSnapshot view, int actorSeat, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind) => _resolve().PublicProposedSlashSuit(view, actorSeat, action, kind);
}
