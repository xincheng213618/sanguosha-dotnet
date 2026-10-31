#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePhaseNamePredictionObserversCorePhaseNamePredictionObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseNamePredictionObserversCorePhaseNamePredictionObserversOperations
    {
        public bool PhaseNamePredictionFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.PhaseNamePredictionObserversEntries.PhaseNamePredictionFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CorePhaseNamePredictionObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseNamePredictionObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePhaseNamePredictionObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseNamePredictionObserversCoreObserverSubtreesOperations2
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }
}
