using CardGame.Core;
using CardGame.Modes.Standard;

internal static class ModeInitialEquipmentPlanChecks
{
    public static void HulaoSelectionKeepsNativeSlotsAndKindThenEntityRandomOrder()
    {
        var random = new ScriptedRandom();
        var content = new EquipmentHost();
        var resolves = 0;
        var hosts = new ModeHostRegistry().Register<IModeSetupRandom>(() => random)
            .Register<IModeSetupEquipmentHost>(() => { resolves++; return content; });
        var mode = new ContentModeDefinition("fixture:hulao-equipment-plan", "虎牢选装", 4, 4,
            new Dictionary<string, int>()) { HulaoActivity = new() };
        var setup = (IMatchModeSetupFeature)new StandardModeSetupFactory().Create(new ModeFeatureCreationContext(
            new GameOptions { PlayerCount = 4, HumanSeat = 0 }, mode, ContentRegistry.Build(), hosts));
        Require(resolves == 0 && setup.InitialEquipmentCompletionFacts.SequenceEqual([typeof(HulaoInitialEquipmentPlacedEvent)]),
            "The pure setup constructor freezes its completion ledger without resolving equipment or consuming random state.");
        Card[] deck = [new(40, CardKind.WushuangHalberd, Suit.Spade, 12), new(41, CardKind.WushuangHalberd, Suit.Heart, 6),
            new(42, CardKind.RedCottonRobe, Suit.Diamond, 1), new(43, CardKind.LionBelt, Suit.Club, 2),
            new(44, CardKind.PurpleGoldCrown, Suit.Club, 1), new(45, CardKind.Slash, Suit.Spade, 7)];
        ModeSeatSetupFacts[] seats = [new(3, false, Role.TeamB, "team:boss", null, "fixture:boss", true)];
        var plan = setup.PrepareSelectedEquipment(deck, seats)!;
        Require(resolves == 1 && random.Maximums.SequenceEqual([1, 2, 2, 1, 1, 1]) &&
            plan.Placements.SequenceEqual([new ModeInitialEquipmentPlacement(3, 41), new(3, 43), new(3, 44)]) &&
            plan.CompletionFacts is [HulaoInitialEquipmentPlacedEvent { BossSeat: 3 }] &&
            plan.Cards.SequenceEqual(deck) && plan.Placements is IList<ModeInitialEquipmentPlacement> { IsReadOnly: true },
            "Hulao chooses by the installed slot definition, then kind and physical entity; unchosen cards remain in the original immutable deck recipe.");
        try
        {
            _ = setup.PrepareSelectedEquipment(deck, [seats[0] with { IsAlive = false }]);
            throw new InvalidOperationException("A dead Boss was accepted for initial equipment.");
        }
        catch (InvalidOperationException ex) when (ex.Message == "Hulao initial equipment requires the selected living Boss.") { }
        Require(random.Maximums.Count == 6,
            "An invalid Boss is rejected before another random selection or equipment-host request.");
    }

    private sealed class ScriptedRandom : IModeSetupRandom
    {
        internal List<int> Maximums { get; } = [];
        private readonly Queue<int> _values = new([0, 1, 1, 0, 0, 0]);
        public int Next(int exclusiveMax) { Maximums.Add(exclusiveMax); return _values.Dequeue(); }
        public void Shuffle<T>(IList<T> values) => throw new InvalidOperationException("A selected-equipment plan cannot reshuffle its physical deck.");
    }
    private sealed class EquipmentHost : IModeSetupEquipmentHost
    {
        public EquipmentDefinition Definition(CardKind kind) => new(kind, kind.ToString(), kind switch
        {
            CardKind.WushuangHalberd => EquipmentSlot.Weapon,
            CardKind.RedCottonRobe or CardKind.LionBelt => EquipmentSlot.Armor,
            CardKind.PurpleGoldCrown => EquipmentSlot.Treasure,
            _ => throw new InvalidOperationException("Only this match's bound equipment may enter the equipment query.")
        }, "fixture");
        public EquipmentSkillBinding? SkillBinding(CardKind kind) => kind == CardKind.Slash ? null :
            new(kind, $"hulao:{kind}", "equipment:hulao");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
