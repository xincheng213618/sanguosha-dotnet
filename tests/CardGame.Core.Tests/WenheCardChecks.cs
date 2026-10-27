using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WenheCardChecks
{
    private const string Driver = "fixture:wenhe-card-driver", Quiet = "fixture:wenhe-card-quiet", Watch = "fixture:wenhe-card-watch";
    private const string Owner = "fixture:wenhe-card-owner", Mode = "fixture:wenhe-card-mode";

    public static void StarShiftUsesRealSettingsChildrenAndOneCounterspell()
    {
        {
            var (g, registry) = Start(CardKind.StarShift, watch: "health");
            Use(g, "hurt", 0); Play(g);
            var observedChild = false;
            for (var attempt = 0; attempt < 3 && !observedChild; attempt++)
            {
                var sourceBefore = V(g, 0).Hp; var targetBefore = V(g, 1).Hp;
                var recoveries = E<RecoveryAppliedEvent>(g).Count(); var losses = E<ProgramSkillHpLostEvent>(g).Count();
                var card = Physical(g, CardKind.StarShift, [1]);
                Until(g, () => P(g)?.SkillPrompt?.SkillId == Watch || IsPlay(g));
                if (P(g)?.SkillPrompt?.SkillId == Watch)
                {
                    observedChild = true;
                    var use = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.StarShift);
                    Need(use.WenheCardEffect is { Applied: true, Stage: WenheCardEffectStage.Children } &&
                        g.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location == CardLocation.Processing &&
                        !E<CardUseFinishedEvent>(g).Any(e => e.ResolutionId == use.Id),
                        "The true HP-setting observer child retains the frozen random result and original paid trick before completion.");
                    Cold(g, registry);
                }
                Play(g);
                var plan = E<WenheCardPlanIssuedEvent>(g).Last(e => e.Kind == CardKind.StarShift);
                Need(plan.SourceHpBefore == sourceBefore && plan.TargetHpBefore == targetBefore &&
                    V(g, 0).Hp == plan.SourceHpAfter && V(g, 1).Hp == plan.TargetHpAfter &&
                    V(g, 0).Hp + V(g, 1).Hp == sourceBefore + targetBefore &&
                    V(g, 0).Hp is >= 1 && V(g, 0).Hp <= V(g, 0).MaxHp && V(g, 1).Hp is >= 1 && V(g, 1).Hp <= V(g, 1).MaxHp &&
                    E<RecoveryAppliedEvent>(g).Count() == recoveries && E<ProgramSkillHpLostEvent>(g).Count() == losses &&
                    g.CardMovements.Count(m => m.CardId == card && m.To == CardLocation.DiscardPile) == 1,
                    "StarShift conserves actual current HP within both maxima, uses Setting rather than recovery/loss and pays its original card once.");
                Cold(g, registry);
            }
            Need(observedChild, "The fixed small fixture actually exercises a changed-HP native observer return, not only an unchanged random split.");
        }
        {
            var (g, registry) = Start(CardKind.StarShift, counter: true);
            Use(g, "hurt", 0); Play(g); var hp = g.CreateSnapshot(0).Players.Select(p => p.Hp).ToArray();
            var card = Physical(g, CardKind.StarShift, [1]); Until(g, () => P(g) is { Kind: DecisionKind.Nullification, PlayerSeat: 0 }); Cold(g, registry);
            var counter = P(g)!.Choices.First(c => c.Parameters.GetValueOrDefault("response") == "nullification").Cards.Single();
            Answer(g, c => c.Parameters.GetValueOrDefault("response") == "nullification" && c.Cards.SequenceEqual([counter])); Play(g);
            Need(g.CreateSnapshot(0).Players.Select(p => p.Hp).SequenceEqual(hp) && !E<WenheCardPlanIssuedEvent>(g).Any() &&
                E<NullificationResolvedEvent>(g).Any(e => e.EffectCardKind == CardKind.StarShift && e.EffectNullified) &&
                new[] { card, counter }.All(id => g.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.DiscardPile) == 1),
                "One genuine native counterspell cancels the complete single-target StarShift effect before any random plan or HP assignment."); Cold(g, registry);
        }
    }

    private static (GameEngine, ContentRegistry) Start(CardKind kind, string? watch = null, bool counter = false, bool equipment = false, bool extraTarget = false)
    {
        IGameContentPackage[] packages = [new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
            .. (kind == CardKind.WenheChaos ? new IGameContentPackage[] { new StandardWenheActivityPackage() } : []),
            new Fixture(kind, watch, counter, equipment, extraTarget)];
        var registry = ContentRegistry.Build(packages);
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = "team:allies", ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(g, new StartGameCommand()); Until(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, Owner, g.Revision, P(g)!.PromptId)); Play(g); return (g, registry);
    }
    internal static GameEngine CreateAiFixture(CardKind kind) => Start(kind).Item1;
    private static PlayerSnapshot V(GameEngine g, int seat) => g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static bool IsPlay(GameEngine g) => P(g) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
    private static void Play(GameEngine g) => Until(g, () => IsPlay(g));
    private static void Use(GameEngine g, string id, int? target = null) => Accept(g, new UseProgramSkillCommand(0, Driver, id, [], target is { } seat ? [seat] : [], g.Revision, P(g)!.PromptId));
    private static int Physical(GameEngine g, CardKind kind, IReadOnlyList<int> targets)
    {
        var action = g.GetHumanLegalActions().First(a => a.CardId is { } id && a.ConversionSource is null &&
            (a.PlayedCardKind ?? V(g, 0).Hand.SingleOrDefault(c => c.Id == id)?.Kind) == kind && a.TargetSeats.SequenceEqual(targets));
        Accept(g, new PlayCardCommand(0, action.CardId!.Value, targets, g.Revision, P(g)!.PromptId, action.PlayedCardKind, action.TargetCardId)); return action.CardId.Value;
    }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> select) { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(select).Id, g.Revision)); }
    private static void Until(GameEngine g, Func<bool> condition)
    {
        for (var i = 0; i < 160; i++)
        {
            if (condition()) return;
            var p = P(g);
            if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die"))
                Answer(g, c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
            else if (p is { PlayerSeat: 0 } && p.SkillPrompt?.SkillId == Watch)
                Answer(g, c => c.Parameters.GetValueOrDefault("program-action") != "activate");
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Wenhe card fixture missed its actual boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static void Cold(GameEngine g, ContentRegistry registry)
    {
        var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(s => SnapshotJson.Serialize(g.CreateSnapshot(s)) == SnapshotJson.Serialize(copy.CreateSnapshot(s))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(copy.CardMovements), "Wenhe's private plans, original payments and typed native children cold-replay from real commands.");
    }
    private static void Accept(GameEngine g, GameCommand command) { var result = g.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Wenhe card command rejected."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(CardKind kind, string? watch, bool counter, bool equipment, bool extraTarget) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("wenhe-card-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var equip = equipment ? "{\"id\":\"equip\",\"window\":\"gameStarting\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"useRandomDeckEquipment\",\"target\":\"owner\",\"targetRef\":{\"kind\":\"owner\"},\"resultBind\":\"equipment\"}]}" : "";
            var watchTrigger = watch == "move" ? "\"window\":\"cardsMoved\",\"sourceZones\":[\"hand\"],\"movementOccurrence\":\"perOwnerBatch\"," :
                watch == "recover" ? "\"window\":\"afterHpRecovered\"," : watch == "damage" ? "\"window\":\"afterDamageApplied\",\"damageOccurrence\":\"perDamage\"," : "\"window\":\"afterHealthChanged\",";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"small-card-hand","query":"initialHandSize","operation":"add","value":8,"priority":0,"condition":{"kind":"always"}}],"triggers":[{{{equip}}}],"activations":[
                {"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":2}]},
                {"id":"hurt-all","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHpParticipants","target":"owner","targetKind":"anyLiving","amount":2}]}
              ]},
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{{{equip + (equipment ? "," : "")}}}{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"watch",{{{watchTrigger}}}"subject":"{{{(watch == "damage" ? "any" : "owner")}}}","optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
              {"id":"fixture:wenhe-card-no-armor","revision":1,"triggers":[{"id":"no-armor","window":"gameStarting","subject":"owner","priority":-100,"optional":false,"effects":[{"op":"alterEquipmentSlots","target":"owner","equipmentSlots":["armor"],"amount":0}]}]}
            ]}
            """;
            var ids = new[] { Driver, Quiet, Watch, "fixture:wenhe-card-no-armor" };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = ids.ToDictionary(id => id, id => new { name = id, description = "Small actual Wenhe card fixture." }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, "Small actual Wenhe fixture.") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            foreach (var cardKind in kind == CardKind.WenheChaos ? Array.Empty<CardKind>() : new[] { CardKind.StarShift, CardKind.BeamSwap, CardKind.HandRedistribution, CardKind.WenheChaos })
                b.AddCard(new($"fixture:wenhe-{cardKind.ToString().ToLowerInvariant()}", CardCatalog.Get(cardKind).DisplayName, "锦囊", "Actual Wenhe card.", cardKind));
            var ownerSkills = (kind == CardKind.WenheChaos ? new[] { "wenhe:chaos-resolution", Watch } : watch is null ? [] : new[] { Watch })
                .Concat(extraTarget ? new[] { "ol:yidian" } : []).ToArray();
            b.AddGeneral(new(Owner, Owner, "", Driver, "wei", 4, ownerSkills));
            string[] peers = ["fixture:wenhe-card-peer-1", "fixture:wenhe-card-peer-2"];
            foreach (var peer in peers) b.AddGeneral(new(peer, peer, "", Quiet, "wei", 4, watch is "recover" or "health" ? [Watch] : []));
            const string boss = "fixture:wenhe-card-boss", deck = "fixture:wenhe-card-deck";
            b.AddGeneral(new(boss, boss, "", Quiet, "qun", 4, equipment ? ["fixture:wenhe-card-no-armor", Watch] : watch == "health" ? [Watch] : []));
            b.AddDeck(new(deck, "Native small Wenhe deck", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 180).Select(i => new ContentDeckPhysicalCard(equipment && i >= 160 ? "classic:silver-lion" :
                counter && i % 3 == 0 ? "standard:nullification" : kind == CardKind.WenheChaos ? (i % 3 == 0 ? "wenhe:chaos" : "standard:slash") :
                $"fixture:wenhe-{kind.ToString().ToLowerInvariant()}", Suit.Heart, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, Mode, 4, 4, new Dictionary<string, int>(), deck, 3, [Owner, .. peers, boss], ContentModeKind.Team,
                new Dictionary<string, int> { ["team:allies"] = 3, ["team:boss"] = 1 })
            { BossActivity = new([Owner, .. peers], [new("cards", [boss])], 1, []) });
        }
    }
}
