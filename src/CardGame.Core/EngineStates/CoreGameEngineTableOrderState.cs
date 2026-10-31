#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineTableOrderState : Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess
{
    private TableSeatMovedEvent? _cachedTableMovement = default !;
    internal ref TableSeatMovedEvent? ReferenceToCachedTableMovement => ref _cachedTableMovement;

    private IReadOnlyList<int>? _cachedTableOrder = default !;
    internal ref IReadOnlyList<int>? ReferenceToCachedTableOrder => ref _cachedTableOrder;

    ref IReadOnlyList<int>? Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableOrder => ref _cachedTableOrder;

    ref TableSeatMovedEvent? Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableMovement => ref _cachedTableMovement;
}
