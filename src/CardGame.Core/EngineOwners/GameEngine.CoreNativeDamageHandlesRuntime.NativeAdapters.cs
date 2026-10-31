#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeDamageHandlesModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDamageHandlesModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CoreNativeDamageHandlesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDamageHandlesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeDamageHandlesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDamageHandlesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
