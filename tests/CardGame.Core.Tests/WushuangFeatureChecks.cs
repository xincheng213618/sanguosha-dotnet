using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void OptionalFeatureOwnsMatchProgressAndRecovery()
    {
        var ordinary = GameEngine.CreateStandard(new GameOptions
        {
            PlayerCount = 5, ModeId = "identity:standard-5", HumanSeat = 0, HumanRole = Role.Lord, Seed = 17
        }, StandardContentRegistry.Create());
        Require(Feature(ordinary) is null && ordinary.CreateWushuangSnapshot(0) is null &&
            ordinary.WushuangRemainingMilliseconds is null,
            "An ordinary table has no shop feature, economy state or duel clock.");
        var revision = ordinary.Revision;
        Require(!ordinary.Submit(new BuyWushuangOfferCommand(0, "absent", null, revision)).Accepted &&
            ordinary.Revision == revision && Feature(ordinary) is null,
            "Submitting a disabled feature's command cannot allocate its progress or commit a revision.");

        var (first, registry) = Start(clock: false);
        var (second, _) = Start(clock: false);
        var firstFeature = Feature(first) ?? throw new InvalidOperationException("The first duel has no feature.");
        var secondFeature = Feature(second) ?? throw new InvalidOperationException("The second duel has no feature.");
        Require(!ReferenceEquals(firstFeature, secondFeature) && !ReferenceEquals(firstFeature.Players, secondFeature.Players),
            "Each selected duel owns exactly its own optional feature and player progress store.");
        Drive(first, () => Shop(first) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        var oldTactics = firstFeature.TacticIds(0);
        Buy(first, Shop(first).Offers[0].Id);
        Play(first);
        Require(oldTactics.Count == 0 && firstFeature.TacticIds(0).Count == 1 && secondFeature.TacticIds(0).Count == 0,
            "Acquiring a tactic changes only its owning match and never mutates a previously projected collection.");
        Accept(first, new TickWushuangClockCommand(1234, first.Revision));
        Require(firstFeature.ElapsedMilliseconds == 1234 && secondFeature.ElapsedMilliseconds == 0,
            "The trusted native clock updates the owning feature without a second engine-side clock state.");
        var restored = GameReplay.Restore(first.CreateCheckpoint(), registry);
        Require(Feature(restored) is { } restoredFeature && !ReferenceEquals(restoredFeature, firstFeature) &&
            restoredFeature.ElapsedMilliseconds == firstFeature.ElapsedMilliseconds &&
            restoredFeature.TacticIds(0).SequenceEqual(firstFeature.TacticIds(0)) &&
            SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(first.CreateSnapshot(0)),
            "Current command replay reconstructs a fresh feature and its exact published state without serializing a second state store.");

        static WushuangModeFeature? Feature(GameEngine engine) =>
            ((MatchModeRuntime)typeof(GameEngine).GetField("_modeRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(engine)!).Find<WushuangModeFeature>();
    }
}
