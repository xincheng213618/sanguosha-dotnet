#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversCoreCompletedUndamagedTargetRevealObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreCompletedUndamagedTargetRevealObserversOperations
    {
        public bool CompletedUndamagedTargetRevealFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.CompletedUndamagedTargetRevealObserversEntries.CompletedUndamagedTargetRevealFirstChild(f, child);
        public bool CompletedUndamagedTargetRevealObserverEdge(int child) => engine.CompletedUndamagedTargetRevealObserversEntries.CompletedUndamagedTargetRevealObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversCoreTieredRoundZeroReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreTieredRoundZeroReturnsOperations
    {
        public bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.TieredRoundZeroReturnsEntries.TieredRoundZeroDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversCharacterHuanShuJunOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCharacterHuanShuJunOlOperations
    {
        public bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying) => engine.HuanShuJunOlEntries.HuanpeiDyingRescueRide(index, dying);
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreObserverSubtreesOperations
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversCoreDrawFundedDistinctBasicReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreDrawFundedDistinctBasicReturnsOperations
    {
        public bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.DrawFundedDistinctBasicReturnsEntries.DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CoreCompletedUndamagedTargetRevealObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
