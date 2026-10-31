#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheNullifiedTrickClaimsEntries
{
    internal void AssertShanheNullifiedTrickClaims() => _resolve().AssertShanheNullifiedTrickClaims();
    internal bool IsShanheNullifiedClaimAwaitingMovement(global::CardGame.Core.NullificationWindowFrame window) => _resolve().IsShanheNullifiedClaimAwaitingMovement(window);
    internal bool IsShanheNullifiedClaimMovement(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsShanheNullifiedClaimMovement(window, batch);
    internal bool IsShanheNullifiedTrickMaterialClaim(long useId, int cardId) => _resolve().IsShanheNullifiedTrickMaterialClaim(useId, cardId);
    internal void ReturnShanheNullifiedClaimMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnShanheNullifiedClaimMovement(child);
    internal bool TryBeginShanheNullifiedTrickClaim(global::CardGame.Core.NullificationWindowFrame window) => _resolve().TryBeginShanheNullifiedTrickClaim(window);
}
