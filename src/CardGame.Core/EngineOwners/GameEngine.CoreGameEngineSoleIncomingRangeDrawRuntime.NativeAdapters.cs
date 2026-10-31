#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSoleIncomingRangeDrawResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingRangeDrawResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSoleIncomingRangeDrawContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingRangeDrawContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSoleIncomingRangeDrawKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingRangeDrawKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSoleIncomingRangeDrawKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingRangeDrawKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }
}
