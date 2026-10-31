#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhuRongBoundaryMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhuRongBoundaryMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhuRongBoundaryMobileModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhuRongBoundaryMobileModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CharacterZhuRongBoundaryMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhuRongBoundaryMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhuRongBoundaryMobilePendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhuRongBoundaryMobilePendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }
}
