#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOrderedPrintedSkillLossObserversCoreOrderedPrintedSkillLossObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOrderedPrintedSkillLossObserversCoreOrderedPrintedSkillLossObserversOperations
    {
        public bool OrderedPrintedSkillLossFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.OrderedPrintedSkillLossObserversEntries.OrderedPrintedSkillLossFirstChild(root, child);
        public bool OrderedPrintedSkillLossObserverEdge(int child) => engine.OrderedPrintedSkillLossObserversEntries.OrderedPrintedSkillLossObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CoreOrderedPrintedSkillLossObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOrderedPrintedSkillLossObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreOrderedPrintedSkillLossObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOrderedPrintedSkillLossObserversCoreObserverSubtreesOperations2
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreOrderedPrintedSkillLossObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOrderedPrintedSkillLossObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
