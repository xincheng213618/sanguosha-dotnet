#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<int> _friendQihuiUnlimited { get; }
}
