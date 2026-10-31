#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed record ShiHcLedgerView(
        IReadOnlyList<GameEngine.ShiHcLedgerGrant> CujunGrants,
        IReadOnlyList<GameEngine.ShiHcLedgerPlayStart> PlayStarts,
        IReadOnlyList<ShiHcSiliSlashArmedEvent> ArmedSlashes,
        IReadOnlyList<ShiHcSiliDrawGrantedEvent> DrawGrants,
        IReadOnlyList<long> ConsumedDrawFrameIds,
        IReadOnlyList<ShiHcSiliBeishuiArmedEvent> BeishuiArmed,
        IReadOnlyList<long> ConsumedBeishuiFrameIds,
        IReadOnlyList<ShiHcCujunClaimIssuedEvent> Claims,
        IReadOnlyList<long> SettledClaimFrameIds)
    {
        internal static readonly ShiHcLedgerView Empty = new([], [], [], [], [], [], [], [], []);

        internal ShiHcCujunClaimIssuedEvent? PendingClaim(int ownerSeat, int turnNumber) => Claims.FirstOrDefault(claim =>
            claim.OwnerSeat == ownerSeat && claim.TurnNumber == turnNumber && !SettledClaimFrameIds.Contains(claim.FrameId));

        internal IReadOnlyList<ShiHcSiliDrawGrantedEvent> PendingDrawGrants(int ownerSeat, int turnNumber) => DrawGrants
            .Where(grant => grant.OwnerSeat == ownerSeat && !ConsumedDrawFrameIds.Contains(grant.FrameId) &&
                !(grant.GrantedInDrawPhase && grant.TurnNumber == turnNumber))
            .ToArray();

        internal IReadOnlyList<ShiHcSiliBeishuiArmedEvent> PendingBeishui(int ownerSeat) => BeishuiArmed
            .Where(entry => entry.OwnerSeat == ownerSeat && !ConsumedBeishuiFrameIds.Contains(entry.FrameId))
            .ToArray();

        internal bool IsSlashArmed(int seat, int cardId) => ArmedSlashes.Any(entry =>
            entry.OwnerSeat == seat && entry.CardId == cardId);
    }
