#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeSkillInvocationObserversCoreNativeEquipmentInvocationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSkillInvocationObserversCoreNativeEquipmentInvocationsOperations
    {
        public bool NativeEquipmentObserverEdge(int index) => engine.NativeEquipmentInvocationsEntries.NativeEquipmentObserverEdge(index);
    }

    [ProgramCapability]
    private sealed class CoreNativeSkillInvocationObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSkillInvocationObserversCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreNativeSkillInvocationObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSkillInvocationObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
