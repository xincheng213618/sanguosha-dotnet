using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), global::CardGame.Core.FriendPredictionPolicy> _friendPredictions => ref _friendTrio.ReferenceToFriendPredictions;
    private ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), int> _friendPredictionCounts => ref _friendTrio.ReferenceToFriendPredictionCounts;
    private ref readonly global::System.Collections.Generic.HashSet<(int Seat, string Instance)> _friendFangqiuSpent => ref _friendTrio.ReferenceToFriendFangqiuSpent;
    private ref readonly global::System.Collections.Generic.HashSet<int> _friendQihuiUnlimited => ref _friendTrio.ReferenceToFriendQihuiUnlimited;
}
