#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLuZhiWeiOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuZhiWeiOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLuZhiWeiOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuZhiWeiOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLuZhiWeiOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuZhiWeiOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterLuZhiWeiOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuZhiWeiOlCoreObserverSubtreesOperations
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CharacterLuZhiWeiOlCharacterLuZhiWeiOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuZhiWeiOlCharacterLuZhiWeiOlOperations
    {
        public bool LuZhiWeiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.LuZhiWeiOlEntries.LuZhiWeiFirstChild(root, child);
    }
}
