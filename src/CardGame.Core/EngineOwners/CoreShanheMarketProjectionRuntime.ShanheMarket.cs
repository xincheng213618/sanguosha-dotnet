#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreShanheMarketProjectionRuntime
{
    internal bool HasPendingShanheMarketPurchase
    {
        get
        {
            _kernelCoreGameEngineFaultBoundaryOperations.EnsureSessionHealthy();
            return _resolutionStackOperations._resolutionStack.OfType<ShanheMarketPurchaseFrame>().Any();
        }
    }

    internal ShanheCampaignSnapshot? CreateShanheMarketSnapshot(int viewerSeat)
    {
        _kernelCoreGameEngineFaultBoundaryOperations.EnsureSessionHealthy();
        return viewerSeat == 0 && _coreMatchDefinitionOptionsAccess._options.HumanSeat == 0 ? _shanheMarketOperations._shanheMarket?.CreateBattleMarketSnapshot(_coreMatchParticipantsPlayersAccess._players[0].Hp, _coreMatchParticipantsPlayersAccess._players[0].MaxHp) : null;
    }

    internal ShanheBattleMarketTransfer? CreateShanheMarketTransfer()
    {
        _kernelCoreGameEngineFaultBoundaryOperations.EnsureSessionHealthy();
        return _shanheMarketOperations._shanheMarket?.CreateBattleMarketTransfer();
    }
}
