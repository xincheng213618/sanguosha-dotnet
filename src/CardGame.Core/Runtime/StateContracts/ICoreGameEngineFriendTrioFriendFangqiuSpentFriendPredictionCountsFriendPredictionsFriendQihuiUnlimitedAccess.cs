#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), global::CardGame.Core.FriendPredictionPolicy> _friendPredictions { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Instance), int> _friendPredictionCounts { get; }

    ref readonly global::System.Collections.Generic.HashSet<(int Seat, string Instance)> _friendFangqiuSpent { get; }

    ref readonly global::System.Collections.Generic.HashSet<int> _friendQihuiUnlimited { get; }
}
