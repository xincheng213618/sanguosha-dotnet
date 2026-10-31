#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess : IProgramRuntimeOperations
{
    ref IReadOnlyList<int>? _cachedTableOrder { get; }

    ref TableSeatMovedEvent? _cachedTableMovement { get; }
}
