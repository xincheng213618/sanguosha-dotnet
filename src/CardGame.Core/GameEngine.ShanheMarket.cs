using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ShanheCampaignSession? _shanheMarket;
    public bool HasPendingShanheMarketPurchase
{        get =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreShanheMarketProjectionRuntime>().HasPendingShanheMarketPurchase;}

    /// <summary>The expedition trader belongs to its human owner; it contains no enemy private state.</summary>
    public ShanheCampaignSnapshot? CreateShanheMarketSnapshot(int viewerSeat)
=>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreShanheMarketProjectionRuntime>().CreateShanheMarketSnapshot(viewerSeat);

    /// <summary>Trusted-host result, imported by the campaign only after its battle journal was verified.</summary>
    public ShanheBattleMarketTransfer? CreateShanheMarketTransfer()
=>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreShanheMarketProjectionRuntime>().CreateShanheMarketTransfer();
}

public sealed record ShanheMarketRefreshedEvent(long MarketRevision, int GoldAfter) : IGameEvent;
