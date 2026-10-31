#nullable enable

namespace CardGame.Core.NativeOwners;

internal readonly struct PlayerViewGroupCardQueries(GroupCardHandle group) : IPlayerViewGroupCardQueries
    {
        public GroupCardEffect Effect => group.Effect;
        public int TargetIndex => group.TargetIndex;
        public IReadOnlyList<int> TargetSeats => group.TargetSeats;
        public IReadOnlyList<int> RevealedCardIds => group.RevealedCardIds;
    }
