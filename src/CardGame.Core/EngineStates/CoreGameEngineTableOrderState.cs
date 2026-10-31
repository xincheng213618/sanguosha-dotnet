#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineTableOrderState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess
{
    private global::CardGame.Core.TableSeatMovedEvent? _cachedTableMovement = default !;
    internal ref global::CardGame.Core.TableSeatMovedEvent? ReferenceToCachedTableMovement => ref _cachedTableMovement;

    private global::System.Collections.Generic.IReadOnlyList<int>? _cachedTableOrder = default !;
    internal ref global::System.Collections.Generic.IReadOnlyList<int>? ReferenceToCachedTableOrder => ref _cachedTableOrder;

    ref global::System.Collections.Generic.IReadOnlyList<int>? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableOrder => ref _cachedTableOrder;

    ref global::CardGame.Core.TableSeatMovedEvent? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess._cachedTableMovement => ref _cachedTableMovement;
}
