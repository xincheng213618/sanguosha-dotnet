#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::System.Collections.Generic.IReadOnlyList<int>? _cachedTableOrder { get; }

    ref global::CardGame.Core.TableSeatMovedEvent? _cachedTableMovement { get; }
}
