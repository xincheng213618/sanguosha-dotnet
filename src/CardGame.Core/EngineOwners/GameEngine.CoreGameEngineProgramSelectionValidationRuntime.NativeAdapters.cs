#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramSelectionValidationProgramSelectionValidatorsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationProgramSelectionValidatorsOperations
    {
        public ref global::CardGame.Core.ProgramSelectionValidationRegistry? _programSelectionValidators => ref engine._programSelectionValidators;
    }
}
