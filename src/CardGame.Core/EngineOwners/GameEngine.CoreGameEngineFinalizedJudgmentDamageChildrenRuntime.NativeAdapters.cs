#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreFinalizedJudgmentDamageChildrenCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedJudgmentDamageChildrenCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreFinalizedJudgmentDamageChildrenResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedJudgmentDamageChildrenResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreFinalizedJudgmentDamageChildrenContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedJudgmentDamageChildrenContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
