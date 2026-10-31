#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversCoreHalfHandPhaseDebtDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversCoreHalfHandPhaseDebtDamageObserversOperations
    {
        public bool HalfHandPaidDamageObserverEdge(int index) => engine.HalfHandPhaseDebtDamageObserversEntries.HalfHandPaidDamageObserverEdge(index);
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversCoreDyingSuitsAndHistoricalUseObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversCoreDyingSuitsAndHistoricalUseObserversOperations
    {
        public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DyingSuitsAndHistoricalUseObserversEntries.DyingSuitsStructuralEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreLastDamageSourceReciprocityObserversCoreProgramChildResolutionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreLastDamageSourceReciprocityObserversCoreProgramChildResolutionOperations
    {
        public bool ProgramChildResolutionEdge(int index) => engine.ProgramChildResolutionEntries.ProgramChildResolutionEdge(index);
    }
}
