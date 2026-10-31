#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheMarketProjectionShanheMarketOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}

internal interface ICoreShanheMarketProjectionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheMarketProjectionKernelCoreGameEngineFaultBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EnsureSessionHealthy();
}
