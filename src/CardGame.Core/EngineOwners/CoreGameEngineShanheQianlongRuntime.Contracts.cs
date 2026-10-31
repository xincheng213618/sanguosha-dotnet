#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheQianlongShanheMarketOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}
