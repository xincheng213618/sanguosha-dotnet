#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreExtraEffectsAndRankGuessObserversCoreExtraEffectsAndRankGuessObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraEffectsAndRankGuessObserversCoreExtraEffectsAndRankGuessObserversOperations
    {
        public bool ExtraEffectsAndRankGuessFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.ExtraEffectsAndRankGuessObserversEntries.ExtraEffectsAndRankGuessFirstChild(root, child);
    }
}
