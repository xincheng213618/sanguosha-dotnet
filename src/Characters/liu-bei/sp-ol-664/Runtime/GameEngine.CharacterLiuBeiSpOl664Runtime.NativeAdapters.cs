#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLiuBeiSpOl664SkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiuBeiSpOl664SkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CharacterLiuBeiSpOl664ContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiuBeiSpOl664ContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLiuBeiSpOl664ResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiuBeiSpOl664ResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLiuBeiSpOl664CardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiuBeiSpOl664CardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLiuBeiSpOl664KernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLiuBeiSpOl664KernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }
}
