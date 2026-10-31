#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDamageJudgmentSuitPaymentObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageJudgmentSuitPaymentObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreDamageJudgmentSuitPaymentObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageJudgmentSuitPaymentObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDamageJudgmentSuitPaymentObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageJudgmentSuitPaymentObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
