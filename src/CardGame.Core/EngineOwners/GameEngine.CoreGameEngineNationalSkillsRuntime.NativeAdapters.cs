#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNationalSkillsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNationalSkillsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreNationalSkillsSkillBindingIndexAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNationalSkillsSkillBindingIndexOperations
    {
        public ref readonly global::CardGame.Core.MatchSkillBindingIndex _skillBindingIndex => ref engine._skillBindingIndex;
    }
}
