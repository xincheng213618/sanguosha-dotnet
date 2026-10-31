#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreForeignSelectedCardSlashReturnsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignSelectedCardSlashReturnsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreForeignSelectedCardSlashReturnsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignSelectedCardSlashReturnsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreForeignSelectedCardSlashReturnsCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignSelectedCardSlashReturnsCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreForeignSelectedCardSlashReturnsCoreForeignSelectedCardSlashReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignSelectedCardSlashReturnsCoreForeignSelectedCardSlashReturnsOperations
    {
        public bool SelectedForeignCardSlashFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.ForeignSelectedCardSlashReturnsEntries.SelectedForeignCardSlashFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CoreForeignSelectedCardSlashReturnsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignSelectedCardSlashReturnsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
