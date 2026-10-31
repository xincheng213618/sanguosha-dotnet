#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterHuangQuanOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterHuangQuanOlCharacterHuangQuanOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanOlCharacterHuangQuanOlOperations
    {
        public bool HuangQuanFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.HuangQuanOlEntries.HuangQuanFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CharacterHuangQuanOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterHuangQuanOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterHuangQuanOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuangQuanOlCoreJianjiDeclarationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuangQuanOlCoreJianjiDeclarationsOperations2
    {
        public bool HuangQuanDeclarationObserverSuffix(int rootIndex) => engine.JianjiDeclarationsEntries.HuangQuanDeclarationObserverSuffix(rootIndex);
    }
}
