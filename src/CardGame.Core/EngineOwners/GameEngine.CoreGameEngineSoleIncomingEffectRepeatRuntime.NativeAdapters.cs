#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSoleIncomingEffectRepeatKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingEffectRepeatKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSoleIncomingEffectRepeatContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingEffectRepeatContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSoleIncomingEffectRepeatResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingEffectRepeatResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSoleIncomingEffectRepeatKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleIncomingEffectRepeatKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }
}
