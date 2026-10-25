using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileDingFengChecks
{
    private const string Mode = "identity:mobile-ding-feng", Owner = "fixture:mobile-ding-feng-owner", Driver = "fixture:mobile-ding-feng-materials";
    public static void PaidDistanceAllowsShortRangeTargetAndExpires()
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, r);
        PendingDecision? P() => Enumerable.Range(0, 4).Select(i => g.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(P())); }
        void Accept(GameCommand c) { var result = g.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Ding Feng command."); }
        void Choose(Func<PromptChoice, bool> predicate) { var p = P()!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, g.Revision)); }
        void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 160 && !ready(); i++)
            {
                if (P() is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), g.Revision, p.PromptId)); continue; }
                if (P() is { } prompt)
                {
                    Need(prompt.Kind != DecisionKind.PlayCard, "The bounded driver must not pass an unrelated Play phase.");
                    var choice = prompt.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0 && (c.Id.Value.Contains("skip", StringComparison.Ordinal) || c.Id.Value.Contains("pass", StringComparison.Ordinal))) ?? prompt.Choices.First();
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, g.Revision));
                }
                else Accept(new AdvanceOneStepCommand(g.Revision));
            }
            Need(ready(), "Missing bounded mobile Ding Feng boundary.");
        }
        Accept(new StartGameCommand()); Reach(() => P() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(g.GetCombatDistance(0, 2) == 2 && g.GetCombatDistance(2, 0) == 2, "The opposite fixed seat begins at physical distance two in both directions.");
        var dodge = g.CreateSnapshot(0).Players[0].Hand.First(c => c.Kind == CardKind.Dodge).Id;
        Accept(new UseProgramSkillCommand(0, Driver, "gift", [dodge], [1], g.Revision, P()!.PromptId));
        Reach(() => P() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(g.CreateSnapshot(1).Players[1].Hand.Single().Id == dodge, "A real one-card gift equips the human responder with its exact Dodge entity.");
        var cost = g.CreateSnapshot(0).Players[0].Hand.First(c => c.Kind == CardKind.Slash).Id;
        Accept(new UseProgramSkillCommand(0, "mobile:fenxun", "discard-fix-distance", [cost], [2], g.Revision, P()!.PromptId));
        Reach(() => P() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(g.GetCombatDistance(0, 2) == 1 && g.GetCombatDistance(2, 0) == 2 && g.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile) == 1 &&
            g.Events.Count(e => e.Payload is MobileFenxunDistanceGrantedEvent) == 1 && !g.GetLegalActions(0).Any(a => a.ProgramSkillId == "mobile:fenxun"),
            "One actual discard pays the directed fixed distance and the real Play-phase quota once.");
        var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), r);
        Need(copy.GetCombatDistance(0, 2) == 1 && copy.GetCombatDistance(2, 0) == 2 && copy.CardMovements.SequenceEqual(g.CardMovements), "Accepted-command restoration preserves the paid directed distance."); g = copy;
        var slash = g.GetLegalActions(0).First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1]));
        Accept(new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, g.Revision, P()!.PromptId, slash.PlayedCardKind));
        Reach(() => P()?.SkillPrompt?.SkillId == "mobile:duanbing" && P().Choices.Any(c => c.Targets.SequenceEqual([2])));
        Choose(c => c.Targets.SequenceEqual([2]));
        Reach(() => P() is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
        Need(g.ResolutionStack.OfType<CardUseFrame>().Last(f => f.CardAttack is not null).CardAttack!.RequiredDodgeResponses == 1, "Mobile short range does not inherit OL's two-Dodge requirement.");
        var primaryHp = g.CreateSnapshot(1).Players[1].Hp;
        Choose(c => c.Cards.SequenceEqual([dodge]));
        Reach(() => P() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(g.CreateSnapshot(1).Players[1].Hp == primaryHp && g.CardMovements.Any(m => m.CardId == dodge && m.To == CardLocation.DiscardPile),
            "One real Dodge fully cancels the primary distance-one target, without a second response.");
        Need(g.Events.Select(e => e.Payload).OfType<ShortRangeSlashTargetResolvedEvent>().Single(e => e.Added).Receipt!.AddedTargetSeat == 2,
            "The actually paid distance authorizes the extra native short-range target.");
        var paid = g.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile);
        Accept(new EndPlayPhaseCommand(0, g.Revision, P()!.PromptId)); Reach(() => P() is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        Need(g.GetCombatDistance(0, 2) == 2 && g.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile) == paid &&
            !g.Events.Any(e => e.Payload is FixedDistanceOneTurnGrantedEvent), "The grant expires with its actual turn and creates no OL ending discard debt.");
    }
    private static string Peer(int seat) => "fixture:mobile-ding-feng-peer-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-ding-feng", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:ding-feng");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
             "modifiers":[{"id":"eight-owned-entities","query":"initialHandSize","operation":"add","value":8,"priority":0}],
             "activations":[{"id":"gift","minCards":1,"maxCards":1,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"sourceZones":["hand"],
               "effects":[{"op":"giveSelected","target":"selectedTarget","amount":1}]}]}]}
            """;
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Driver] = new { name = "真实固定材料", description = "完整牌堆先发给拥有者，再通过真实交牌给响应者一张闪。" } } }));
            b.AddSkill(new(Driver, "真实固定材料", "初始材料与原生交牌") { Program = c.Programs[Driver] });
            b.AddGeneral(new(Owner, "移动丁奉真实费用", "supporter", "mobile:duanbing", "wu", 8, ["mobile:fenxun", Driver]));
            for (var i = 1; i < 4; i++) b.AddGeneral(new(Peer(i), "固定同桌", "supporter", "standard:none", "wei", 8));
            const string deck = "fixture:mobile-ding-feng-deck";
            b.AddDeck(new(deck, "固定八张杀闪实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 4).SelectMany(_ => new[] {
                new ContentDeckPhysicalCard("standard:slash", Suit.Heart, 7), new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2) }).ToArray() });
            b.AddMode(new(Mode, "移动丁奉实际回合", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
