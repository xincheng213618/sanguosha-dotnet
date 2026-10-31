#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHalfHandPhaseDebtReceiptsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtReceiptsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreHalfHandPhaseDebtReceiptsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHalfHandPhaseDebtReceiptsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
