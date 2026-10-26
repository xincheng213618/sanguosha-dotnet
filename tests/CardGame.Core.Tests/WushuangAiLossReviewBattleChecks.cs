using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WushuangAiLossReviewBattleChecks
{
    private static readonly WushuangBuildProfile Plain = new(0, 0, 0, false, 0);

    public static void CurrentDuelRecastsEmptyChainButPreservesOwnedSynergyAndLegacyThoughts()
    {
        var view = View([Card(1, CardKind.IronChain)], Plain);
        var actions = new[] { new LegalAction(LegalActionKind.IronChain, 1, 1, "chain opponent"),
            new LegalAction(LegalActionKind.Recast, 1, null, "recast"),
            new LegalAction(LegalActionKind.IronChain, 1, null, "chain both", TargetSeats: [0, 1]), End() };
        Need(Choose(view, 12, actions).Action.Kind == LegalActionKind.Recast &&
            Choose(view, 11, actions).Action.Kind == LegalActionKind.IronChain,
            "With both players initially unchained and no owned synergy, neither chaining only the opponent nor chaining both may bypass recasting; policy11 retains its action.");
        var registry = StandardWushuangContentRegistry.Create();
        var actual = WushuangBuildProfile.Read([registry.Skills["ol:shen-liu-bei-jieying"].Program!]);
        Need(actual.HasChainSynergy, "The actual current Jieying chain/hand-limit program advertises a real chain interaction.");
        var acquired = Own(view, player => player with { WushuangBuild = actual,
            Skills = [new("acquired", "visible program") { ContentId = "ol:shen-liu-bei-jieying" }] });
        Need(Choose(acquired, 12, actions).Action.Kind == LegalActionKind.IronChain,
            "An actually acquired chain program preserves useful chaining instead of applying a roster-name exclusion.");
        Need(JsonSerializer.Serialize(Choose(view, 11, actions).Thought) == JsonSerializer.Serialize(Choose(acquired, 11, actions).Thought),
            "The new profile hint leaves policy11 candidates, reasons and jitter consumption unchanged.");
        Need(Choose(Own(view, player => player with { WushuangBuild = null }), 12, actions).Action.Kind == LegalActionKind.IronChain &&
            Choose(view with { Wushuang = null }, 12, actions).Action.Kind == LegalActionKind.IronChain,
            "Unknown build context and other modes retain the historical decision.");
        var chained = Own(view, player => player with { IsChained = true });
        Need(Choose(chained, 12, [new(LegalActionKind.IronChain, 1, 0, "unchain self"), actions[1], End()]).Action.TargetSeat == 0,
            "Removing the owner's chain remains a useful defensive action.");
        Need(!JsonSerializer.Serialize(Plain).Contains("HasChainSynergy", StringComparison.Ordinal),
            "The default optional hint does not add a field to historical plain build snapshots.");
    }

    public static void CurrentDuelPreparesIndependentGudingLethalAndRejectsUnsupportedPlans()
    {
        var view = View([Card(1, CardKind.Slash), Card(2, CardKind.GudingBlade)], Plain);
        var actions = new[] { new LegalAction(LegalActionKind.Slash, 1, 1, "slash"),
            new LegalAction(LegalActionKind.Equip, 2, null, "equip Guding"), End() };
        PublicDamagePrediction One(LegalAction _, int __) => Payment(1);
        Need(Choose(view, 12, actions, One).Action.Kind == LegalActionKind.Equip &&
            Choose(view, 11, actions, One).Action.Kind == LegalActionKind.Slash,
            "Against a public empty-hand two-HP opponent, policy12 equips before spending its only ordinary Slash; policy11 attacks first.");
        var equipped = Own(view, player => player with { Hand = [Card(1, CardKind.Slash)], HandCount = 1,
            Equipment = [Card(2, CardKind.GudingBlade)] });
        Need(Choose(equipped, 12, [actions[0], End()], (_, _) => Payment(2)).Action.Kind == LegalActionKind.Slash && Payment(2).IsLethal,
            "After the real equipment settlement a fresh legal Slash decision reaches the public two-damage lethal.");
        foreach (var unsupported in new[] {
            view with { Wushuang = null },
            Own(view, player => player with { Equipment = [Card(3, CardKind.QinggangSword)] }),
            Other(view, player => player with { HandCount = 1 }),
            Other(view, player => player with { IncomingDamageCap = 1 }),
            Other(view, player => player with { Equipment = [Card(3, CardKind.SilverLion)] }) })
            Need(Choose(unsupported, 12, actions, One).Action.Kind != LegalActionKind.Equip,
                "Other modes, replacing an existing weapon, hidden responses, damage caps and armor cannot receive this narrow lethal preparation bonus.");
        foreach (var unsupported in new[] { Payment(1, fixedDamage: true), Payment(1, remaining: 0), Payment(1, reduction: 1), Payment(0) })
            Need(Choose(view, 12, actions, (_, _) => unsupported).Thought.Candidates.Single(candidate => candidate.Action.Kind == LegalActionKind.Equip)
                .Reason.Contains("先装备古锭刀", StringComparison.Ordinal) == false,
                "Fixed damage, spent Slash quota, finite reduction and prevented damage cannot fabricate a lethal weapon plan.");
        Need(Choose(view, 12, [actions[1], End()], One).Thought.Candidates.All(candidate =>
            !candidate.Reason.Contains("先装备古锭刀", StringComparison.Ordinal)),
            "Equipment does not invent an independent legal follow-up attack.");
    }

    private static PublicDamagePrediction Payment(int amount, bool fixedDamage = false, int remaining = 1, int reduction = 0)
    {
        var payment = PublicDamagePrediction.Calculate(2, 0, amount, maxHp: 4);
        return payment with { Wushuang = new(payment, null, false, fixedDamage, reduction, remaining) };
    }
    private static (LegalAction Action, AiThoughtRecord Thought) Choose(GameSnapshot view, int policy,
        IReadOnlyList<LegalAction> actions, Func<LegalAction, int, PublicDamagePrediction>? prediction = null) =>
        new SimpleAiBrain(0, 71, policy).ChoosePlay(view, actions, 1, prediction);
    private static GameSnapshot Own(GameSnapshot view, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == 0 ? change(player) : player).ToArray() };
    private static GameSnapshot Other(GameSnapshot view, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == 1 ? change(player) : player).ToArray() };
    private static LegalAction End() => new(LegalActionKind.EndPlay, null, null, "end");
    private static CardSnapshot Card(int id, CardKind kind) => new(id, kind, Suit.Spade, 7, "visible", "7");
    private static GameSnapshot View(IReadOnlyList<CardSnapshot> hand, WushuangBuildProfile build) =>
        new(null, 0, EngineStatus.Running, Winner.None, 3, 0, TurnPhase.Play, 80, 0,
            [new(0, "owner", false, null, false, "fixture:unchanged-owner", "", "", 4, 4, true, hand.Count, hand)
                { TeamId = "blue", WushuangBuild = build },
             new(1, "opponent", false, null, false, "fixture:opponent", "", "", 2, 4, true, 0, [])
                { TeamId = "red", WushuangBuild = Plain }], null)
        { ModeKind = ContentModeKind.Team, Wushuang = new(0, 1, 0, 0, 0, [], [], false, 1, 0,
            WushuangMode.TimeLimitMilliseconds, 1) { RemainingTurns = 12 } };
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
