#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries
{
    internal void AssertShanheDelayedNullifiedTrickClaims() => _resolve().AssertShanheDelayedNullifiedTrickClaims();
    internal bool HasShanheClaimedDelayedOwner(NullificationWindowFrame window, DelayedEffectContinuation delayed) => _resolve().HasShanheClaimedDelayedOwner(window, delayed);
    internal bool IsShanheDelayedNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => _resolve().IsShanheDelayedNullifiedClaimAwaitingMovement(window);
    internal bool IsShanheDelayedNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => _resolve().IsShanheDelayedNullifiedClaimMovement(window, batch);
    internal bool TryBeginShanheDelayedNullifiedTrickClaim(NullificationWindowFrame window) => _resolve().TryBeginShanheDelayedNullifiedTrickClaim(window);
    internal bool TryCompleteShanheClaimedDelayedNullification(NullificationWindowFrame window, DelayedEffectContinuation delayed) => _resolve().TryCompleteShanheClaimedDelayedNullification(window, delayed);
    internal void ValidateShanheDelayedNullifiedTrickClaim(NullificationWindowFrame window, ShanheDelayedNullifiedTrickClaim receipt) => _resolve().ValidateShanheDelayedNullifiedTrickClaim(window, receipt);
}
