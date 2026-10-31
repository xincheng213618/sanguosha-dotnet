#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries
{
    internal void AssertShanheDelayedNullifiedTrickClaims() => _resolve().AssertShanheDelayedNullifiedTrickClaims();
    internal bool HasShanheClaimedDelayedOwner(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed) => _resolve().HasShanheClaimedDelayedOwner(window, delayed);
    internal bool IsShanheDelayedNullifiedClaimAwaitingMovement(global::CardGame.Core.NullificationWindowFrame window) => _resolve().IsShanheDelayedNullifiedClaimAwaitingMovement(window);
    internal bool IsShanheDelayedNullifiedClaimMovement(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsShanheDelayedNullifiedClaimMovement(window, batch);
    internal bool TryBeginShanheDelayedNullifiedTrickClaim(global::CardGame.Core.NullificationWindowFrame window) => _resolve().TryBeginShanheDelayedNullifiedTrickClaim(window);
    internal bool TryCompleteShanheClaimedDelayedNullification(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed) => _resolve().TryCompleteShanheClaimedDelayedNullification(window, delayed);
    internal void ValidateShanheDelayedNullifiedTrickClaim(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.ShanheDelayedNullifiedTrickClaim receipt) => _resolve().ValidateShanheDelayedNullifiedTrickClaim(window, receipt);
}
