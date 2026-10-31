#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreFirstSlashDuelBenefitObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFirstSlashDuelBenefitObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreFirstSlashDuelBenefitObserversCoreFirstSlashDuelBenefitObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFirstSlashDuelBenefitObserversCoreFirstSlashDuelBenefitObserversOperations
    {
        public bool FirstSlashDuelFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.FirstSlashDuelBenefitObserversEntries.FirstSlashDuelFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreFirstSlashDuelBenefitObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFirstSlashDuelBenefitObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
