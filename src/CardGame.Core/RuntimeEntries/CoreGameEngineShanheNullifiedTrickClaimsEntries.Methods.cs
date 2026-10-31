#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheNullifiedTrickClaimsEntries
{
    internal void AssertShanheNullifiedTrickClaims() => _resolve().AssertShanheNullifiedTrickClaims();
    internal bool IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => _resolve().IsShanheNullifiedClaimAwaitingMovement(window);
    internal bool IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => _resolve().IsShanheNullifiedClaimMovement(window, batch);
    internal bool IsShanheNullifiedTrickMaterialClaim(long useId, int cardId) => _resolve().IsShanheNullifiedTrickMaterialClaim(useId, cardId);
    internal void ReturnShanheNullifiedClaimMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnShanheNullifiedClaimMovement(child);
    internal bool TryBeginShanheNullifiedTrickClaim(NullificationWindowFrame window) => _resolve().TryBeginShanheNullifiedTrickClaim(window);
}
