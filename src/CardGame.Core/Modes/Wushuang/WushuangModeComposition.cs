namespace CardGame.Core;

internal sealed partial class MatchModeRuntime
{
    internal WushuangModeFeature? Wushuang => Find<WushuangModeFeature>();
}

public sealed partial class GameEngine
{
    // The composition boundary is the only place that selects optional mode
    // services. Common command and turn execution depend on their contracts.
    private MatchModeRuntime CreateMatchModeRuntime()
    {
        if (_modeDefinition.Id == WushuangMode.ModeId)
            return new(CreateWushuangModeFeature(), new WushuangModeRuntimeAdapter(this));
        if (_modeDefinition.NianActivity is not null)
            return new(new NianModeCommandAdapter(this));
        return MatchModeRuntime.Empty;
    }

    private sealed class WushuangModeRuntimeAdapter(GameEngine engine) :
        IMatchModeCommandHandler, IMatchModeOpeningBoundary, IMatchModeTurnIntermission
    {
        public IReadOnlyList<Type> CommandTypes { get; } = Array.AsReadOnly<Type>(
        [
            typeof(TickWushuangClockCommand), typeof(BuyWushuangOfferCommand),
            typeof(RerollWushuangShopCommand), typeof(UpgradeWushuangShopCommand),
            typeof(FinishWushuangShopCommand), typeof(ToggleWushuangShopLockCommand)
        ]);

        public CommandResult Submit(GameCommand command) => command switch
        {
            TickWushuangClockCommand clock => engine.SubmitWushuangClock(clock),
            BuyWushuangOfferCommand purchase => engine.SubmitBuyWushuangOffer(purchase),
            RerollWushuangShopCommand refresh => engine.SubmitRerollWushuangShop(refresh),
            UpgradeWushuangShopCommand upgrade => engine.SubmitUpgradeWushuangShop(upgrade),
            FinishWushuangShopCommand finish => engine.SubmitFinishWushuangShop(finish),
            ToggleWushuangShopLockCommand toggle => engine.SubmitToggleWushuangShopLock(toggle),
            _ => throw new InvalidOperationException("Unregistered Wushuang command reached its mode adapter.")
        };

        public bool TryBeginOpening() => engine.TryBeginWushuangOpening();
        public bool TryBeginIntermission(int endedSeat, bool skipped) =>
            engine.TryBeginWushuangShopping(engine._players[endedSeat], skipped);
    }

    private sealed class NianModeCommandAdapter(GameEngine engine) : IMatchModeCommandHandler
    {
        public IReadOnlyList<Type> CommandTypes { get; } = Array.AsReadOnly<Type>([typeof(TickNianClockCommand)]);
        public CommandResult Submit(GameCommand command) => command is TickNianClockCommand clock
            ? engine.SubmitNianClock(clock)
            : throw new InvalidOperationException("Unregistered Nian command reached its mode adapter.");
    }
}
