#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterXuChuAnniversaryGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuChuAnniversaryGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterXuChuAnniversaryGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuChuAnniversaryGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterXuChuAnniversaryGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuChuAnniversaryGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterXuChuAnniversaryGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuChuAnniversaryGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }
}
