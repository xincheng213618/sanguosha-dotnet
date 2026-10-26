using System.Text.Json;
using CardGame.Core;

internal static class WushuangAiDrawPreparationChecks
{
    private static readonly WushuangBuildProfile Plain = new(0, 0, 0, false, 0);
    private static readonly LegalAction Slash = new(LegalActionKind.Slash, 1, 1, "native Slash");
    private static readonly LegalAction Draw = new(LegalActionKind.DrawTwo, 2, 0, "native Draw2");
    private static readonly LegalAction End = new(LegalActionKind.EndPlay, null, null, "end");
    private static readonly LegalAction[] Ordinary = [Slash, Draw, End];

    public static void DrawBeforeSpendingTheOnlyNonLethalSlashWithoutGuessingNewCards()
    {
        var view = View();
        var reviewed = Choose(view, AiDecisionPolicy.WushuangRefinementVersion, Ordinary);
        Need(reviewed.Action == Draw && Choose(view, AiDecisionPolicy.WushuangLossReviewVersion, Ordinary).Action == Slash,
            "The current ordinary Draw2 can expand this turn's choices before the owner spends its sole nonlethal native Slash; policy12 keeps its historical attack-first order.");
        var slash = reviewed.Thought.Candidates.Single(c => c.Action == Slash);
        var draw = reviewed.Thought.Candidates.Single(c => c.Action == Draw);
        Need(draw.Score > slash.Score && draw.Score - slash.Score is > 5.99 and < 6.01,
            "Only a bounded six-point ordering preference is added above the already useful current attack, not hypothetical drawn damage.");
        foreach (var restricted in new[] { view with { DrawPileCount = 1, DiscardPileCount = 0 }, view with { CurrentSeat = 1 } })
            Need(!Planned(Choose(restricted, AiDecisionPolicy.WushuangRefinementVersion, Ordinary).Thought),
                "Insufficient public draw supply and another seat's phase cannot promise the preparation.");
        Need(!Planned(Choose(view, AiDecisionPolicy.WushuangRefinementVersion, [Draw, End]).Thought) &&
            !Planned(Choose(view, AiDecisionPolicy.WushuangRefinementVersion, Ordinary, remaining: 0).Thought) &&
            !Planned(Choose(view, AiDecisionPolicy.WushuangRefinementVersion, Ordinary, remaining: 2).Thought),
            "The plan requires an independently published attack and exactly one scarce ordinary use; it invents neither a future Slash nor a current quota.");
        var defensive = Own(view, player => player with { Skills = [new("actual visible conversion", "only this trick is a response material")
        { ViewAsOpportunities = [new(CardKind.Dodge, [CardKind.DrawTwo], [], false, true)] }] });
        Need(!Planned(Choose(defensive, AiDecisionPolicy.WushuangRefinementVersion, Ordinary).Thought),
            "A native Draw2 that is the owner's only visible Dodge material retains its existing dynamic reserve cost instead of being promoted over it.");
        var foreignDraw = new LegalAction(LegalActionKind.DrawTwo, 2, 1, "foreign Draw2");
        Need(!Planned(Choose(view, AiDecisionPolicy.WushuangRefinementVersion, [Slash, foreignDraw, End]).Thought),
            "A Draw2 with a foreign beneficiary is not promoted as if it certainly expands this owner's hand.");
        var other = view with { Wushuang = null };
        Need(JsonSerializer.Serialize(Choose(other, 12, Ordinary).Thought) ==
            JsonSerializer.Serialize(Choose(other, AiDecisionPolicy.WushuangRefinementVersion, Ordinary).Thought),
            "Other modes preserve candidates, reasons and jitter exactly for this new Wushuang-only preference.");
    }

    public static void DrawPreparationPreservesPublicLethalWineAndEquipmentLethal()
    {
        var version = AiDecisionPolicy.WushuangRefinementVersion;
        var lethal = Other(View(), player => player with { Hp = 1 });
        Need(Choose(lethal, version, Ordinary).Action == Slash && !Planned(Choose(lethal, version, Ordinary).Thought),
            "A currently lethal legal Slash is used directly instead of gambling on a preparatory draw.");
        var drunk = Own(View(), player => player with { HasAlcoholEffect = true });
        Need(Choose(drunk, version, Ordinary).Action == Slash && !Planned(Choose(drunk, version, Ordinary).Thought),
            "An already active wine bonus is completed before reopening a draw-first plan.");
        var armed = View([Card(1, CardKind.Slash), Card(2, CardKind.DrawTwo), Card(3, CardKind.GudingBlade)], enemyHp: 2);
        var equip = new LegalAction(LegalActionKind.Equip, 3, null, "equip Guding");
        Need(Choose(armed, version, [Slash, Draw, equip, End]).Action == equip,
            "The public empty-hand Guding preparation that reaches lethal remains above the small draw-before-nonlethal-Slash preference.");
        var hurt = Own(View([Card(1, CardKind.Slash), Card(2, CardKind.DrawTwo), Card(3, CardKind.Peach)]), player => player with { Hp = 2 });
        var heal = new LegalAction(LegalActionKind.Peach, 3, 0, "recover");
        Need(!Planned(Choose(hurt, version, [Slash, Draw, heal, End]).Thought),
            "Available healing at a dangerous visible blood line is not demoted by the new preparatory draw preference.");
        var skill = new LegalAction(LegalActionKind.UseProgramSkill, null, null, "public lethal skill", MinTargetCount: 1, MaxTargetCount: 1)
        {
            ProgramSkillId = "fixture:public-lethal", ProgramActivationId = "damage", SelectableTargetSeats = [1],
            ProgramAiHint = new(0, 0, 0, 0, 0, 0, false, false)
                { TargetHealthEffects = [new(PublicHealthEffectKind.Damage, 4)] }
        };
        Need(!Planned(Choose(View(), version, [Slash, Draw, skill, End]).Thought),
            "A currently published skill's visible lethal target payment also disables speculative preparation.");
    }

    private static bool Planned(AiThoughtRecord thought) => thought.Candidates.Any(candidate => candidate.Action.Kind == LegalActionKind.DrawTwo &&
        candidate.Reason.Contains("先用无中扩展", StringComparison.Ordinal));
    private static (LegalAction Action, AiThoughtRecord Thought) Choose(GameSnapshot view, int policy,
        IReadOnlyList<LegalAction> actions, int remaining = 1) => new SimpleAiBrain(0, 71, policy).ChoosePlay(view, actions, 1,
        (_, seat) =>
        {
            var target = view.Players.Single(player => player.Seat == seat);
            var payment = PublicDamagePrediction.Calculate(target.Hp, target.Armor, 1, maxHp: target.MaxHp);
            return payment with { Wushuang = new(payment, null, false, false, 0, remaining) };
        });
    private static GameSnapshot Own(GameSnapshot view, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == 0 ? change(player) : player).ToArray() };
    private static GameSnapshot Other(GameSnapshot view, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == 1 ? change(player) : player).ToArray() };
    private static CardSnapshot Card(int id, CardKind kind) => new(id, kind, Suit.Spade, 7, "visible", "7");
    private static GameSnapshot View(IReadOnlyList<CardSnapshot>? hand = null, int enemyHp = 4)
    {
        hand ??= [Card(1, CardKind.Slash), Card(2, CardKind.DrawTwo)];
        return new(null, 0, EngineStatus.Running, Winner.None, 3, 0, TurnPhase.Play, 80, 0,
            [new(0, "owner", false, null, false, "fixture:draw-owner", "", "", 4, 4, true, hand.Count, hand)
                { TeamId = "blue", WushuangBuild = Plain },
             new(1, "opponent", false, null, false, "fixture:draw-opponent", "", "", enemyHp, 4, true, 0, [])
                { TeamId = "red", WushuangBuild = Plain }], null)
        { ModeKind = ContentModeKind.Team, Wushuang = new(0, 1, 0, 0, 0, [], [], false, 1, 0,
            WushuangMode.TimeLimitMilliseconds, 1) { RemainingTurns = 12 } };
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
