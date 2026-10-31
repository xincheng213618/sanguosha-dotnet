#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheMarketApplicationCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketApplicationCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreShanheMarketApplicationKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketApplicationKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CoreShanheMarketApplicationShanheMarketAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketApplicationShanheMarketOperations
    {
        public ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket => ref engine._shanheMarket;
    }

    [ProgramCapability]
    private sealed class CoreShanheMarketApplicationKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketApplicationKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreShanheMarketApplicationResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketApplicationResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
