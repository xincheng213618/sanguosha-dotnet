#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterGuanPingMouCharacterGuanPingMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanPingMouCharacterGuanPingMouOperations
    {
        public global::CardGame.Core.NativeOwners.MouGuanPingFacts CreateMouGuanPingFacts() => engine.ComposeNativeMouGuanPingFacts();
    }

    [ProgramCapability]
    private sealed class CharacterGuanPingMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanPingMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterGuanPingMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanPingMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGuanPingMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuanPingMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
