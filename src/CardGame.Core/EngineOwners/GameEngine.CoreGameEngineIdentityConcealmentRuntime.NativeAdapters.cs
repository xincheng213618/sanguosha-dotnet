#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreIdentityConcealmentKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIdentityConcealmentKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreIdentityConcealmentKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIdentityConcealmentKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CoreIdentityConcealmentContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIdentityConcealmentContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreIdentityConcealmentResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIdentityConcealmentResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreIdentityConcealmentKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIdentityConcealmentKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
