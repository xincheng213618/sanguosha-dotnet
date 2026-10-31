#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoShuangMobileShiCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangMobileShiCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangMobileShiRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangMobileShiRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangMobileShiKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangMobileShiKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangMobileShiObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangMobileShiObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangMobileShiResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangMobileShiResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangMobileShiContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangMobileShiContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
