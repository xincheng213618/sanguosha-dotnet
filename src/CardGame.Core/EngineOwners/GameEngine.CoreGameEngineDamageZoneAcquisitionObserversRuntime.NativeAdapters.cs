#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDamageZoneAcquisitionObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageZoneAcquisitionObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDamageZoneAcquisitionObserversCoreDamageZoneAcquisitionObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageZoneAcquisitionObserversCoreDamageZoneAcquisitionObserversOperations
    {
        public bool DamageAcquisitionFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.DamageZoneAcquisitionObserversEntries.DamageAcquisitionFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreDamageZoneAcquisitionObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageZoneAcquisitionObserversCoreObserverSubtreesOperations2
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }
}
