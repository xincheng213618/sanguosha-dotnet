#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineFriendTrioState : Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess, Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess
{
    private HashSet<(int Seat, string Instance)> _friendFangqiuSpent = [];
    internal ref readonly HashSet<(int Seat, string Instance)> ReferenceToFriendFangqiuSpent => ref _friendFangqiuSpent;

    private Dictionary<(int Seat, string Instance), int> _friendPredictionCounts = [];
    internal ref readonly Dictionary<(int Seat, string Instance), int> ReferenceToFriendPredictionCounts => ref _friendPredictionCounts;

    private Dictionary<(int Seat, string Instance), FriendPredictionPolicy> _friendPredictions = [];
    internal ref readonly Dictionary<(int Seat, string Instance), FriendPredictionPolicy> ReferenceToFriendPredictions => ref _friendPredictions;

    private HashSet<int> _friendQihuiUnlimited = [];
    internal ref readonly HashSet<int> ReferenceToFriendQihuiUnlimited => ref _friendQihuiUnlimited;

    ref readonly Dictionary<(int Seat, string Instance), FriendPredictionPolicy> Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendPredictions => ref _friendPredictions;

    ref readonly Dictionary<(int Seat, string Instance), int> Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendPredictionCounts => ref _friendPredictionCounts;

    ref readonly HashSet<(int Seat, string Instance)> Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendFangqiuSpent => ref _friendFangqiuSpent;

    ref readonly HashSet<int> Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendQihuiUnlimited => ref _friendQihuiUnlimited;

    ref readonly HashSet<int> Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess._friendQihuiUnlimited => ref _friendQihuiUnlimited;
}
