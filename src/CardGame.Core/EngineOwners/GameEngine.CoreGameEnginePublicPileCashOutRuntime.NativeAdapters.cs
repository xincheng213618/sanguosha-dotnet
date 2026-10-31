#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePublicPileCashOutContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePublicPileCashOutKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CorePublicPileCashOutCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutCoreObserverSubtreesOperations2
    {
        public bool HasPaidDamageObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasPaidDamageObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CorePublicPileCashOutObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class CorePublicPileCashOutCorePublicPileCashOutAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutCorePublicPileCashOutOperations
    {
        public bool PublicPileCashOutFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.PublicPileCashOutEntries.PublicPileCashOutFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CorePublicPileCashOutCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePublicPileCashOutResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCashOutResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
