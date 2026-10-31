#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiOlMagicCharacterSimaYiOlMagicAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiOlMagicCharacterSimaYiOlMagicOperations
    {
        public global::CardGame.Core.NativeOwners.MoSimaYiFacts CreateMoSimaYiFacts() => engine.ComposeNativeMoSimaYiFacts();
    }
}
