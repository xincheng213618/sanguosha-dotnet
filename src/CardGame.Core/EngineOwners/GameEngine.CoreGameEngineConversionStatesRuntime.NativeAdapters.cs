#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreConversionStatesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreConversionStatesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreConversionStatesSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreConversionStatesSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }
}
