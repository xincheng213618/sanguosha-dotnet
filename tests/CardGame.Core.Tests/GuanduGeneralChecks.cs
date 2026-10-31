using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class GuanduGeneralChecks
{
    private const string Mode = "fixture:guandu-generals", Driver = "fixture:guandu-general-driver", Quiet = "fixture:guandu-general-quiet", Watch = "fixture:guandu-general-watch";

    public static void JieliangPaysBeforeForeignNormalDrawAndUsesActualTurnHandLimit()
    {
        var (g, r) = Start(["jieliang"]);
        Need(!E<GuanduJieliangPaidEvent>(g).Any(), "Jieliang cannot observe its own normal Draw phase.");
        End(g); Until(g, () => P(g)?.SkillPrompt?.SkillId == "guandu:jieliang");
        Answer(g, IsActivate); Until(g, () => Stage(g) == "jieliang-cost");
        var target = g.ResolutionStack.OfType<ProgramSkillFrame>().Last().GuanduGeneral!.TargetSeat;
        var before = V(g, 0).HandCount; var baseLimit = V(g, target).HandLimit; Cold(g, r);
        Answer(g, c => c.Parameters.GetValueOrDefault("option") == "discard");
        Until(g, () => g.CardMovements.Any(m => m.TurnNumber == E<GuanduJieliangPaidEvent>(g).Single().ActualTurn &&
            m.To == CardLocation.Hand(target) && m.Reason == CardMoveReasons.Draw));
        var paid = E<GuanduJieliangPaidEvent>(g).Single();
        Need(V(g, 0).HandCount == before - 1 && V(g, target).HandLimit == baseLimit - 1 &&
            g.CardMovements.Count(m => m.TurnNumber == paid.ActualTurn && m.To == CardLocation.Hand(target) && m.Reason == CardMoveReasons.Draw) == 1 &&
            E<TurnRuleModifierGrantedEvent>(g).Count(e => e.Modifier.Source.SkillId == "guandu:jieliang") == 1,
            "The foreign role's actual normal draw loses one card only after the owner's real discard, with exactly one scoped hand-limit penalty."); Cold(g, r);
    }

    public static void XiyingPaidRefusalBlocksTrueUseAndResponseForOnlyTheActualTurn()
    {
        var (g, r) = Start(["xiying"], reachPlay: false);
        Until(g, () => P(g)?.SkillPrompt?.SkillId == "guandu:xiying"); Answer(g, IsActivate);
        Until(g, () => Stage(g) == "xiying-cost"); Cold(g, r);
        Answer(g, c => c.Parameters.GetValueOrDefault("option") == "discard"); Play(g);
        Need(E<GuanduAllCardBanIssuedEvent>(g).Any(e => e.TargetSeat == 1), "The allied AI's genuine refusal creates an exact actual-turn all-card ban after the owner's cost.");
        var revision = g.Revision;
        var rejected = g.Submit(new UseProgramSkillCommand(0, Driver, "duel-me", [], [1], revision, P(g)!.PromptId));
        Need(!rejected.Accepted && g.Revision == revision && !E<CardUseDeclaredEvent>(g).Any(e => e.SourceSeat == 1 && e.CardKind == CardKind.Duel),
            "A banned role cannot issue a native virtual Duel through another character's selected-actor request.");
        var hp = V(g, 1).Hp; Physical(g, CardKind.Slash, [1]); Play(g);
        Need(V(g, 1).Hp == hp - 1 && !E<CardRespondedEvent>(g).Any(e => e.ResponderSeat == 1),
            "The same exact ban also prevents the damaged ally's real card response, without bypassing the ordinary Slash damage route."); Cold(g, r);
    }

    private static (GameEngine, ContentRegistry) Start(string[] skills, string[]? enemy = null, bool foreignGifts = false, bool reachPlay = true)
    {
        var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skills, enemy ?? [], foreignGifts));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 8, HumanSeat = 0, HumanRole = null,
            HumanTeamId = "team:cao", HumanGuanduRole = GuanduSeatRole.Commander, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 48 }, r);
        Accept(g, new StartGameCommand()); Until(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, P(g)!.Choices.First().ContentIds.Single(), g.Revision, P(g)!.PromptId)); if (reachPlay) Play(g); return (g, r);
    }
    private static PlayerSnapshot V(GameEngine g, int s) => g.CreateSnapshot(s).Players[s];
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 8).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static string? Stage(GameEngine g) => g.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.GuanduGeneral?.Stage;
    private static bool IsActivate(PromptChoice c) => c.Parameters.GetValueOrDefault("program-action") == "activate";
    private static void End(GameEngine g) => Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
    private static void Physical(GameEngine g, CardKind kind, IReadOnlyList<int> targets)
    { var a = g.GetHumanLegalActions().First(a => a.Kind.ToString() == kind.ToString() && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets)); Accept(g, new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId)); }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> select)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(select).Id, g.Revision)); }
    private static void Play(GameEngine g) => Until(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Until(GameEngine g, Func<bool> done)
    {
        for (var i = 0; i < 480; i++)
        {
            if (done()) return;
            if (P(g) is { PlayerSeat: 0 } p)
            {
                var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" ||
                    c.Parameters.GetValueOrDefault("response") is "pass" or "decline" or "take-damage" or "fire-attack-skip" || c.Parameters.GetValueOrDefault("option") == "decline");
                if (choice is not null) { Answer(g, c => c.Id == choice.Id); continue; }
            }
            Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException($"Guandu general boundary missing: status={g.CreateSnapshot(0).Status}, prompt={P(g)?.Kind}/{P(g)?.SkillPrompt?.SkillId}, stage={Stage(g)}.");
    }
    private static void Cold(GameEngine g, ContentRegistry r) => GuanduCardChecks.Cold(g, r);
    private static void Accept(GameEngine g, GameCommand c) => GuanduCardChecks.Accept(g, c);
    private static void Need(bool condition, string text) => GuanduCardChecks.Need(condition, text);

    private sealed class Fixture(string[] human, string[] enemy, bool foreignGifts) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-guandu-generals", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var assembly = typeof(StandardContentPackage).Assembly;
            foreach (var bundle in new[] { "guandu-activity", "guandu-events" })
            {
                using var rules = new StreamReader(assembly.GetManifestResourceStream($"CardGame.Content.Standard.SkillPrograms.{bundle}.rules.json")!);
                using var presentation = new StreamReader(assembly.GetManifestResourceStream($"CardGame.Content.Standard.SkillPrograms.{bundle}.presentation.json")!);
                var actual = SkillProgramCatalog.Load(rules.ReadToEnd(), presentation.ReadToEnd());
                foreach (var (id, program) in actual.Programs) b.AddSkill(new(id, actual.Presentations[id].Name, actual.Presentations[id].Description)
                    { Program = program, ProgramPresentation = actual.Presentations[id], Tags = id is "guandu:yongdi" or "guandu:niepan" ? SkillTag.Limited : SkillTag.None });
            }
            var json = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
             {"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"hand","query":"initialHandSize","operation":"add","value":16,"priority":0,"condition":{"kind":"always"}}],
              "activations":[
               {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
               {"id":"fire","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"fire"}]},
               {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
               {"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand","equipment"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
               {"id":"sweep","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]},
               {"id":"skip-next","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["fixture:guandu-general-skipplay"]}]},
               {"id":"place","minCards":1,"maxCards":1,"sourceZones":["hand","equipment"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"anyLivingMale","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"cost"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"cost"}]},
               {"id":"duel-me","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,"effects":[{"op":"useSelectedActorDuel","target":"owner"}]}]},
             {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw","window":"nativeDrawStarting","subject":"owner","optional":true,"effects":[{"op":"setChainedState","target":"owner","chained":true}]}]},
             {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":true,"effects":[{"op":"draw","target":"owner","amount":1},{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
             {"id":"fixture:guandu-general-skipplay","revision":1,"triggers":[{"id":"skip-once","window":"afterNormalDraw","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
            ]}
            """;
            var own = SkillProgramCatalog.Load(json, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Watch, Quiet, "fixture:guandu-general-skipplay" }.ToDictionary(id => id, id => new { name = id, description = "Compact native Guandu general fixture." }) }));
            foreach (var (id, program) in own.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = own.Presentations[id] });
            foreach (var kind in new[] { CardKind.Intoxication, CardKind.StoreGrain, CardKind.Reinforcements, CardKind.UnitedEffort, CardKind.ThunderChariot })
                b.AddCard(new("fixture:guandu-general-card-" + kind, kind.ToString(), CardCatalog.Get(kind).CategoryName, kind.ToString(), kind));
            var pools = new Dictionary<string, IReadOnlyList<string>>();
            foreach (var army in new[] { "cao", "yuan" })
            {
                var ids = Enumerable.Range(0, 9).Select(i => $"fixture:guandu-general-{army}-{i}").ToArray();
                for (var i = 0; i < ids.Length; i++)
                {
                    var skillIds = army == "cao" && i != 0 && foreignGifts ? new[] { "guandu:yuanlve" } : army == "cao" && i == 0 ? new[] { Driver, Watch, Quiet }.Concat(human.Select(s => "guandu:" + s)) :
                        new[] { Quiet }.Concat(army == "yuan" && i == 0 ? enemy.Select(s => "guandu:" + s) : []);
                    var all = skillIds.ToArray(); b.AddGeneral(new(ids[i], ids[i], "", all[0], army == "cao" ? "wei" : "qun", 8, all.Skip(1).ToArray()));
                }
                pools["team:" + army] = ids;
            }
            var cards = new[] { "standard:slash", "standard:bagua", "standard:defensive_horse", "standard:fire_attack", "standard:slash", "standard:slash" };
            const string deck = "fixture:guandu-general-deck";
            b.AddDeck(new(deck, deck, 4, 2, []) { PhysicalCards = Enumerable.Range(0, 360).Select(i => new ContentDeckPhysicalCard(cards[i % cards.Length], Suit.Heart, i % 13 + 1)).ToArray() });
            var layout = new[] { "team:cao", "team:cao", "team:cao", "team:cao", "team:yuan", "team:yuan", "team:yuan", "team:yuan" };
            b.AddMode(new(Mode, Mode, 8, 8, new Dictionary<string, int>(), deck, 6, pools.Values.SelectMany(ids => ids).ToArray(), ContentModeKind.Team,
                new Dictionary<string, int> { ["team:cao"] = 4, ["team:yuan"] = 4 })
            { GuanduActivity = new(pools, new Dictionary<string, string> { ["team:cao"] = pools["team:cao"][0], ["team:yuan"] = pools["team:yuan"][0] },
                [GuanduBattleEvent.Underdog], Enumerable.Range(0, 5).Select(_ => (IReadOnlyList<string>)layout).ToArray(), PlayerTier: 1) });
        }
    }
}
