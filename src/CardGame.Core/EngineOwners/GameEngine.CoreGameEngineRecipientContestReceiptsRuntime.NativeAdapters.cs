#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRecipientContestReceiptsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientContestReceiptsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreRecipientContestReceiptsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientContestReceiptsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
