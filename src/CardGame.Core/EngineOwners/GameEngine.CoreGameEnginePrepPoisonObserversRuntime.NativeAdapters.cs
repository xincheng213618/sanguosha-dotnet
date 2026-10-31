#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePrepPoisonObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePrepPoisonObserversCoreOrderedPrintedSkillLossObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversCoreOrderedPrintedSkillLossObserversOperations
    {
        public bool OrderedPrintedSkillLossObserverEdge(int child) => engine.OrderedPrintedSkillLossObserversEntries.OrderedPrintedSkillLossObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CorePrepPoisonObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversCoreObserverSubtreesOperations2
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CorePrepPoisonObserversCoreProgramChildResolutionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversCoreProgramChildResolutionOperations
    {
        public bool ProgramChildResolutionEdge(int index) => engine.ProgramChildResolutionEntries.ProgramChildResolutionEdge(index);
    }

    [ProgramCapability]
    private sealed class CorePrepPoisonObserversKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CorePrepPoisonObserversResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePrepPoisonObserversCorePrepPoisonObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrepPoisonObserversCorePrepPoisonObserversOperations
    {
        public bool PrepPoisonFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.PrepPoisonObserversEntries.PrepPoisonFirstChild(root, child);
    }
}
