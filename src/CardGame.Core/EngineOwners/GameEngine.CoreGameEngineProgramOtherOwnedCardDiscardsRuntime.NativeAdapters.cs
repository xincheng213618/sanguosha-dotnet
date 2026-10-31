#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramOtherOwnedCardDiscardsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramOtherOwnedCardDiscardsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
