using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class HellKingChecks
{
    private const string Driver = "fixture:hell-king-driver", Quiet = "fixture:hell-king-quiet", Mode = "fixture:hell-king-mode";

    public static void DeathEffectsRecalculateExtremeHpAndPreserveNativeSubtrees()
    {
        {
            var (g, registry) = Start(["classic:wuhun"], ["hell:huoxing"]); var boss = Peer(g);
            Use(g, "peer-kill-owner", boss);
            Until(g, () => P(g)?.SkillPrompt?.SkillId == "classic:wuhun" && P(g)!.Choices.Any(c => c.Targets.SequenceEqual([boss])));
            Cold(g, registry); Answer(g, c => c.Targets.SequenceEqual([boss]));
            Until(g, () => E<ProgramBindingResolvedEvent>(g).Any(e => e.SkillId == "hell:huoxing" && e.Activated && e.Completed));
            Require(!V(g, 0).IsAlive && !V(g, boss).IsAlive &&
                E<ProgramSkillCauseDeathDeclaredEvent>(g).Single() is { SkillId: "classic:wuhun", SourceSeat: 0 } direct && direct.TargetSeat == boss &&
                E<JudgmentResolvedEvent>(g).Single(e => e.Reason == "skill.wuhun.death") is { CardKind: CardKind.Slash } &&
                E<DamageAppliedEvent>(g).Count(e => e.SourceSeat == boss && e.Amount == 1 && e.Nature == DamageNature.Fire && e.TargetSeat != 0 && e.TargetSeat != boss) == 2,
                $"A true attributed lethal hit invokes Wuhun's real nightmare target and non-Peach judgment, directly kills the king, then completes both Huoxing fire-damage children while retaining the original death tree. Boss={boss}; alive={V(g, 0).IsAlive}/{V(g, boss).IsAlive}; direct={JsonSerializer.Serialize(E<ProgramSkillCauseDeathDeclaredEvent>(g))}; damage={JsonSerializer.Serialize(E<DamageAppliedEvent>(g))}."); Cold(g, registry);
        }
        {
            var (g, registry) = Start([], ["hell:pingdeng"]); var boss = Peer(g);
            Use(g, "kill", boss); Until(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "hell-king-extreme") == true); Cold(g, registry);
            Until(g, () => E<HellKingChoiceRecordedEvent>(g).Any(e => e.OwnerSeat == boss && e.ResultBind == "second-victim"));
            Play(g);
            var first = E<HellKingChoiceRecordedEvent>(g).Single(e => e.OwnerSeat == boss && e.ResultBind == "first-victim");
            var second = E<HellKingChoiceRecordedEvent>(g).Single(e => e.OwnerSeat == boss && e.ResultBind == "second-victim");
            var damage = E<DamageAppliedEvent>(g).Where(e => e.SourceSeat == boss).ToArray();
            Require(!V(g, boss).IsAlive && first.TargetSeat != second.TargetSeat && damage.Length == 2 && damage[0].TargetSeat == first.TargetSeat && damage[0].Amount == 2 &&
                damage[1].TargetSeat == second.TargetSeat && damage[1].Amount == 1 && damage.All(e => e.Nature is DamageNature.Fire or DamageNature.Thunder),
                "Pingdeng keeps the true dead skill source, completes the first native damage subtree, then recalculates the highest living HP before its independent second random element."); Cold(g, registry);
        }
        {
            var (g, registry) = Start([], ["hell:wangxiang"], equipAtStart: true); var boss = Peer(g);
            var originals = g.CreateSnapshot(0).Players.Where(p => p.Seat != boss).SelectMany(p => p.Equipment.Select(c => (Seat: p.Seat, c.Id))).ToArray();
            Require(originals.Length == 3, "Every surviving fixture participant has a real original equipped card.");
            Use(g, "kill", boss); Play(g);
            Require(originals.All(item => g.CardMovements.Any(m => m.CardId == item.Id && m.From == CardLocation.Equipment(item.Seat) && m.To == CardLocation.DiscardPile)) &&
                g.CreateSnapshot(0).Players.Where(p => p.Seat != boss).All(p => p.Equipment.Count == 0),
                "Wangxiang's death discards every other participant's actual equipment through native movements."); Cold(g, registry);
        }
        {
            var (g, registry) = Start([], ["hell:bingfeng"]); var boss = Peer(g);
            Use(g, "kill", boss); Play(g);
            Require(V(g, 0).IsFaceDown && E<CharacterStateChangedEvent>(g).Any(e => e.Change.TargetSeat == 0 && e.Change.Window == SkillProgramTriggerWindow.CharacterTurnedOver),
                "Icebound identifies the real killer from the death context and turns that originally face-up character over natively."); Cold(g, registry);
        }
    }

    private static (GameEngine, ContentRegistry) Start(string[] ownerSkills, string[] peerSkills, string card = "standard:slash", Suit suit = Suit.Heart, bool equipAtStart = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardGhostActivityPackage(), new StandardHellActivityPackage(),
            new Fixture(ownerSkills, peerSkills, card, suit, equipAtStart));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = "team:blue", ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry);
        Accept(g, new StartGameCommand()); Until(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, "fixture:hell-king-actor", g.Revision, P(g)!.PromptId)); Play(g); return (g, registry);
    }
    private static void Use(GameEngine g, string id, int? target = null) => Accept(g, new UseProgramSkillCommand(0, Driver, id, [], target is { } seat ? [seat] : [], g.Revision, P(g)!.PromptId));
    private static int Peer(GameEngine g) => g.CreateSnapshot(0).Players.Single(p => p.GeneralId == "fixture:hell-king-peer-1").Seat;
    private static PlayerSnapshot V(GameEngine g, int seat) => g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static void Play(GameEngine g) => Until(g, () => P(g) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Answer(GameEngine g, Func<PromptChoice, bool> predicate) { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, g.Revision)); }
    private static void Until(GameEngine g, Func<bool> condition)
    {
        for (var i = 0; i < 250; i++)
        {
            if (condition()) return;
            if (P(g) is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass")) Answer(g, c => c.Parameters.GetValueOrDefault("response") == "pass");
            else if (P(g) is { PlayerSeat: 0 } card && card.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "hand-repayment")) Answer(g, c => c.Parameters.GetValueOrDefault("repayment") == "give");
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Hell king fixture missed the actual boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static void Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 4).All(s => SnapshotJson.Serialize(g.CreateSnapshot(s)) == SnapshotJson.Serialize(restored.CreateSnapshot(s))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(restored.CardMovements), "All private views, random invoices and native material movements reproduce from accepted commands.");
    }
    private static void Accept(GameEngine g, GameCommand command) { var result = g.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Hell command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(string[] ownerSkills, string[] peerSkills, string card, Suit suit, bool equipAtStart) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("hell-king-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var equipment = equipAtStart ? ",{\"op\":\"useRandomDeckEquipment\",\"target\":\"owner\",\"targetRef\":{\"kind\":\"owner\"},\"resultBind\":\"initial-equipment\"}" : "";
            var growth = "{\"id\":\"native-health\",\"window\":\"gameStarting\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"changeMaximumHp\",\"target\":\"owner\",\"amount\":8},{\"op\":\"recover\",\"target\":\"owner\",\"amount\":8}" + equipment + "]}";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"cardPolicies":[{"id":"fixture-distance","kind":"ignoreUseDistance","cardKinds":["slash","fireSlash","thunderSlash"]}],"triggers":[{{{growth}}}],"activations":[
                {"id":"one-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                {"id":"fire","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2,"nature":"fire"}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"peer-kill-owner","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":20}]},
                {"id":"lose-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]},
                {"id":"lose-ten","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":10}]},
                {"id":"recover","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"selectedTarget","amount":2}]},
                {"id":"draw-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":2}]},
                {"id":"give","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true}]}
              ]},
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{{{growth}}},{"id":"skip-play","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, id => new { name = id, description = "Small actual Hell king rule fixture." }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, "Small actual Hell king rule fixture.") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new("fixture:hell-king-actor", "Hell king actor", "", Driver, "qun", BaseHp: 12, AdditionalSkillIds: ownerSkills));
            for (var i = 1; i < 4; i++) b.AddGeneral(new($"fixture:hell-king-peer-{i}", $"Hell king peer {i}", "", Quiet, "qun", BaseHp: 12, AdditionalSkillIds: i == 1 ? peerSkills : []));
            b.AddDeck(new("fixture:hell-king-deck", "Real uniform materials", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 180).Select(i => new ContentDeckPhysicalCard(i < 160 ? card : "standard:crossbow", suit, 7)).ToArray() });
            b.AddMode(new(Mode, "Actual Hell king fixture", 4, 4, new Dictionary<string, int>(), "fixture:hell-king-deck", 4,
                ["fixture:hell-king-actor", "fixture:hell-king-peer-1", "fixture:hell-king-peer-2", "fixture:hell-king-peer-3"], ContentModeKind.Team,
                new Dictionary<string, int> { ["team:blue"] = 2, ["team:red"] = 2 }));
        }
    }
}
