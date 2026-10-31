#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCurrentEnhancementSlashRedirectsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCurrentEnhancementSlashRedirectsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreCurrentEnhancementSlashRedirectsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
