#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreMatchingRecastObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchingRecastObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreMatchingRecastObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchingRecastObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreMatchingRecastObserversSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchingRecastObserversSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreMatchingRecastObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchingRecastObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreMatchingRecastObserversCoreMatchingRecastObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchingRecastObserversCoreMatchingRecastObserversOperations
    {
        public bool MatchingRecastFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.MatchingRecastObserversEntries.MatchingRecastFirstChild(f, child);
    }
}
