using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheMarketRefreshedEvent(long MarketRevision, int GoldAfter) : IGameEvent;
