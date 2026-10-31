#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkContinuationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkContinuationsOperations
    {
        public bool IsRecipientCategoryObserverEdge(int child) => engine.RecipientCategoryMarkContinuationsEntries.IsRecipientCategoryObserverEdge(child);
        public bool RecipientCategoryObserverSuffix(int rootIndex) => engine.RecipientCategoryMarkContinuationsEntries.RecipientCategoryObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreRecipientCategoryMarkContinuationsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientCategoryMarkContinuationsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkObserversOperations2
    {
        public bool RecipientCategoryMarkFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.RecipientCategoryMarkObserversEntries.RecipientCategoryMarkFirstChild(f, child);
    }
}
