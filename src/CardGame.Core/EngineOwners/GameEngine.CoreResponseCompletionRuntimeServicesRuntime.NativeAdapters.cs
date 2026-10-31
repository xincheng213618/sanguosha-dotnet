#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreResponseCompletionRuntimeServicesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreResponseCompletionRuntimeServicesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreResponseCompletionRuntimeServicesKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId) => engine.ComposeNativeDuelHandle(ownerId);
        public global::CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId) => engine.ComposeNativeGroupCardHandle(ownerId);
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreResponseCompletionRuntimeServicesPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }
}
