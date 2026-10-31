#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Seat, string Instance), FriendPredictionPolicy> _friendPredictions { get; }

    ref readonly Dictionary<(int Seat, string Instance), int> _friendPredictionCounts { get; }

    ref readonly HashSet<(int Seat, string Instance)> _friendFangqiuSpent { get; }

    ref readonly HashSet<int> _friendQihuiUnlimited { get; }
}
