#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSoleTargetProtectionObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreSoleTargetProtectionObserversCoreSoleTargetProtectionObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionObserversCoreSoleTargetProtectionObserversOperations
    {
        public bool SoleTargetProtectionFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SoleTargetProtectionObserversEntries.SoleTargetProtectionFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CoreSoleTargetProtectionObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreSoleTargetProtectionObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSoleTargetProtectionObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
