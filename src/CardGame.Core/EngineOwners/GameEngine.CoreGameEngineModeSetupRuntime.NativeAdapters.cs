#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreModeSetupContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreModeSetupModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreModeSetupKernelCoreGameEngineModeSetupAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupKernelCoreGameEngineModeSetupOperations
    {
        public global::CardGame.Core.NativeOwners.ParticipantSetupContext CreateParticipantSetupContext() => engine.ComposeNativeParticipantSetupContext();
    }

    [ProgramCapability]
    private sealed class CoreModeSetupParticipantSetupAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupParticipantSetupOperations
    {
        public ref global::CardGame.Core.MatchParticipantSetup? _participantSetup => ref engine._participantSetup;
    }
}
