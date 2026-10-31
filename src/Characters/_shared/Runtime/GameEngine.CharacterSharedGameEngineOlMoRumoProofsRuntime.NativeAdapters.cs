#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoProofsProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoProofsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSharedGameEngineOlMoRumoProofsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSharedGameEngineOlMoRumoProofsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
