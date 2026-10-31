#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineFriendTrioState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess
{
    private global::System.Collections.Generic.HashSet<(int Seat, string Instance)> _friendFangqiuSpent = [];
    internal ref readonly global::System.Collections.Generic.HashSet<(int Seat, string Instance)> ReferenceToFriendFangqiuSpent => ref _friendFangqiuSpent;

    private global::System.Collections.Generic.Dictionary<(int Seat, string Instance), int> _friendPredictionCounts = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), int> ReferenceToFriendPredictionCounts => ref _friendPredictionCounts;

    private global::System.Collections.Generic.Dictionary<(int Seat, string Instance), global::CardGame.Core.FriendPredictionPolicy> _friendPredictions = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), global::CardGame.Core.FriendPredictionPolicy> ReferenceToFriendPredictions => ref _friendPredictions;

    private global::System.Collections.Generic.HashSet<int> _friendQihuiUnlimited = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToFriendQihuiUnlimited => ref _friendQihuiUnlimited;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), global::CardGame.Core.FriendPredictionPolicy> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendPredictions => ref _friendPredictions;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendPredictionCounts => ref _friendPredictionCounts;

    ref readonly global::System.Collections.Generic.HashSet<(int Seat, string Instance)> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendFangqiuSpent => ref _friendFangqiuSpent;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess._friendQihuiUnlimited => ref _friendQihuiUnlimited;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess._friendQihuiUnlimited => ref _friendQihuiUnlimited;
}
