#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreResolutionIdentityBoundaryResolutionSchedulerAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreResolutionIdentityBoundaryResolutionSchedulerOperations
    {
        public ref readonly global::CardGame.Core.ResolutionScheduler _resolutionScheduler => ref engine._resolutionScheduler;
    }
}
