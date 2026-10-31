#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramShanheSourceWarElephantKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramShanheSourceWarElephantKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CoreProgramShanheSourceWarElephantCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramShanheSourceWarElephantCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreProgramShanheSourceWarElephantResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramShanheSourceWarElephantResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramShanheSourceWarElephantKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramShanheSourceWarElephantKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }
}
