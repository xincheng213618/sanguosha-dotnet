#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhaoYunBoundaryMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoYunBoundaryMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoYunBoundaryMobilePendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoYunBoundaryMobilePendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoYunBoundaryMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoYunBoundaryMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoYunBoundaryMobileModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoYunBoundaryMobileModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }
}
