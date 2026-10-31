#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramTargetResolversProgramTargetResolversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTargetResolversProgramTargetResolversOperations
    {
        public ref global::CardGame.Core.ProgramTargetResolverRegistry? _programTargetResolvers => ref engine._programTargetResolvers;
    }
}
