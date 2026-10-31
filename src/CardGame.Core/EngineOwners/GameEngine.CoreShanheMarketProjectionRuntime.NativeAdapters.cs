#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheMarketProjectionShanheMarketAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketProjectionShanheMarketOperations
    {
        public ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket => ref engine._shanheMarket;
    }

    [ProgramCapability]
    private sealed class CoreShanheMarketProjectionResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketProjectionResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreShanheMarketProjectionKernelCoreGameEngineFaultBoundaryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheMarketProjectionKernelCoreGameEngineFaultBoundaryOperations
    {
        public void EnsureSessionHealthy() => engine.EnsureSessionHealthy();
    }
}
