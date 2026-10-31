#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHeartSlashDamageEntries
{
    internal bool MatchesOwnerUsedHeartSlash(GameEngine.IDamageAttempt attack) => _resolve().MatchesOwnerUsedHeartSlash(attack);
    internal Suit PublicProposedSlashSuit(GameSnapshot view, int actorSeat, LegalAction action, CardKind kind) => _resolve().PublicProposedSlashSuit(view, actorSeat, action, kind);
}
