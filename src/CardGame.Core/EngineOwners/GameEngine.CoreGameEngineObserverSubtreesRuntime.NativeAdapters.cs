#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreObserverSubtreesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntBoolAdapter : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntBoolOperations
    {
        public bool Invoke(global::System.Func<int, bool> __nativeReceiver, int arg2) => __nativeReceiver.Invoke(arg2);
        public CoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntBoolAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineCardGameCoreProgramSkillFrameCardGameCoreResolutionFrameBoolAdapter : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineCardGameCoreProgramSkillFrameCardGameCoreResolutionFrameBoolOperations
    {
        public bool Invoke(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> __nativeReceiver, global::CardGame.Core.ProgramSkillFrame arg2, global::CardGame.Core.ResolutionFrame arg3) => __nativeReceiver.Invoke(arg2, arg3);
        public CoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineCardGameCoreProgramSkillFrameCardGameCoreResolutionFrameBoolAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCoreTieredRoundZeroReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCoreTieredRoundZeroReturnsOperations
    {
        public bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.TieredRoundZeroReturnsEntries.TieredRoundZeroDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCoreDyingSuitsAndHistoricalUseObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCoreDyingSuitsAndHistoricalUseObserversOperations
    {
        public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DyingSuitsAndHistoricalUseObserversEntries.DyingSuitsStructuralEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntCardGameCoreDyingFrameBoolAdapter : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntCardGameCoreDyingFrameBoolOperations
    {
        public bool Invoke(global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? __nativeReceiver, int arg2, global::CardGame.Core.DyingFrame arg3) => __nativeReceiver.Invoke(arg2, arg3);
        public CoreObserverSubtreesNativeSystemFuncCardGameCoreGameEngineIntCardGameCoreDyingFrameBoolAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCoreDrawFundedDistinctBasicReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCoreDrawFundedDistinctBasicReturnsOperations
    {
        public bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.DrawFundedDistinctBasicReturnsEntries.DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
        public bool IsPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCharacterHuanShuJunOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCharacterHuanShuJunOlOperations
    {
        public bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying) => engine.HuanShuJunOlEntries.HuanpeiDyingRescueRide(index, dying);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCoreProgramChildResolutionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCoreProgramChildResolutionOperations
    {
        public bool ProgramChildResolutionEdge(int index) => engine.ProgramChildResolutionEntries.ProgramChildResolutionEdge(index);
    }

    [ProgramCapability]
    private sealed class CoreObserverSubtreesCoreHalfHandPhaseDebtDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreObserverSubtreesCoreHalfHandPhaseDebtDamageObserversOperations2
    {
        public bool HalfHandPaidDamageObserverEdge(int index) => engine.HalfHandPhaseDebtDamageObserversEntries.HalfHandPaidDamageObserverEdge(index);
    }
}
