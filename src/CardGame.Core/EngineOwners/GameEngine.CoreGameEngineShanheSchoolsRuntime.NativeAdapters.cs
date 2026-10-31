#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheSchoolsShanheMarketAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSchoolsShanheMarketOperations
    {
        public ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket => ref engine._shanheMarket;
    }
}
