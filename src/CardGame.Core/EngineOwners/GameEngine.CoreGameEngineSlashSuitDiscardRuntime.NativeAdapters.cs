#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSlashSuitDiscardKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashSuitDiscardKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSlashSuitDiscardResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashSuitDiscardResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSlashSuitDiscardCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashSuitDiscardCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreSlashSuitDiscardKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashSuitDiscardKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreSlashSuitDiscardContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashSuitDiscardContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
