#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheSourceMouDongZhuoChildrenCoreShanheSourceMouDongZhuoChildrenAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceMouDongZhuoChildrenCoreShanheSourceMouDongZhuoChildrenOperations
    {
        public bool SourceMouEdge(int index) => engine.ShanheSourceMouDongZhuoChildrenEntries.SourceMouEdge(index);
        public bool SourceMouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ShanheSourceMouDongZhuoChildrenEntries.SourceMouFirstChild(f, child);
        public bool SourceMouSuffix(int index) => engine.ShanheSourceMouDongZhuoChildrenEntries.SourceMouSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceMouDongZhuoChildrenCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceMouDongZhuoChildrenCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceMouDongZhuoChildrenResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceMouDongZhuoChildrenResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
