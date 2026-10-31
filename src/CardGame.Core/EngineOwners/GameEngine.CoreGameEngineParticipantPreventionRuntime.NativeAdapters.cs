#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreParticipantPreventionContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantPreventionContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreParticipantPreventionResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantPreventionResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
