#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterFuRongFuQianAnniversaryCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterFuRongFuQianAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterFuRongFuQianAnniversaryCharacterFuRongFuQianAnniversaryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterFuRongFuQianAnniversaryCharacterFuRongFuQianAnniversaryOperations
    {
        public bool AnniversaryFuPairFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.FuRongFuQianAnniversaryEntries.AnniversaryFuPairFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterFuRongFuQianAnniversaryResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterFuRongFuQianAnniversaryResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterFuRongFuQianAnniversaryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterFuRongFuQianAnniversaryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterFuRongFuQianAnniversaryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterFuRongFuQianAnniversaryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }
}
