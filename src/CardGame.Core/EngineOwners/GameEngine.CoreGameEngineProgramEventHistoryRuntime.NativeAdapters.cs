#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramEventHistoryProgramHistoryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramEventHistoryProgramHistoryOperations
    {
        public ref global::CardGame.Core.ProgramEventHistoryIndex? _programHistory => ref engine._programHistory;
    }
}
