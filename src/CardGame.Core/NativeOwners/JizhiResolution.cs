#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class JizhiResolution(
        int playerSeat,        long resolutionId,        Card card,        CardKind effectiveCardKind,        IReadOnlyList<int> targetSeats,        LegalActionKind actionKind,        int? targetCardId,        CardKind? requiredCardKind)
    {
        public int PlayerSeat { get; } = playerSeat;
        public long ResolutionId { get; } = resolutionId;
        public Card Card { get; } = card;
        public CardKind EffectiveCardKind { get; } = effectiveCardKind;
        public IReadOnlyList<int> TargetSeats { get; } = Array.AsReadOnly(targetSeats.ToArray());
        public LegalActionKind ActionKind { get; } = actionKind;
        public int? TargetCardId { get; } = targetCardId;
        public CardKind? RequiredCardKind { get; } = requiredCardKind;
    }
