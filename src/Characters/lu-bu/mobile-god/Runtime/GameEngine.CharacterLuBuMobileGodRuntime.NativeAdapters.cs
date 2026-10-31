#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, global::CardGame.Core.CardConversionSource? conversionSource = default) => engine.ComposeNativeFangtianHalberdHandle(resolutionId, sourceSeat, card, effectiveCardKind, ignoresArmor, damageAmount, targetSeats, usesFangtian, countedTowardSlashLimit, conversionSource);
    }

    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class CharacterLuBuMobileGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLuBuMobileGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
