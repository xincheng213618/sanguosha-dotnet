#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRoundGainedEquipmentObserversCoreRoundGainedEquipmentObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundGainedEquipmentObserversCoreRoundGainedEquipmentObserversOperations
    {
        public bool RoundGainedEquipmentDrawFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.RoundGainedEquipmentObserversEntries.RoundGainedEquipmentDrawFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CoreRoundGainedEquipmentObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundGainedEquipmentObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreRoundGainedEquipmentObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRoundGainedEquipmentObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
