using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class ShenwuNpcChecks
{
    private const string Driver = "fixture:shenwu-npc-driver", Quiet = "fixture:shenwu-npc-quiet", Mode = "fixture:shenwu-npc-mode";
    private const string NonLockedReward = "fixture:shenwu-npc-nonlocked-reward";

    public static void LuanchangOwnStartAndActualEndKeepNativeAoeParents()
    {
        var (g, registry) = Start(["shenwu:luanchang", "shenwu:mingwan"], [], beginPlay: false);
        Until(g, () => g.ResolutionStack.OfType<CardUseFrame>().Any(f => f.VirtualOrdinaryTrickOrigin is not null));
        var start = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.VirtualOrdinaryTrickOrigin is not null);
        Require(start.CardKind == CardKind.BarbarianAssault && start.CardId == 0 &&
            g.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WindowContext?.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow),
            "Luanchang's material-free Nanman is a real native child of its actual turn-start binding.");
        Cold(g, registry); Play(g);
        Require(E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == start.Id) == 1 && E<GroupResponseEvent>(g).Count(e => e.ResolutionId == start.Id && e.UsedResponse) == 3,
            "The turn-start assault consumes the three real Slash responses and returns once.");
        Require(!E<ShenwuMingwanVictimRecordedEvent>(g).Any(), "The answered assault causes no damage and cannot arm Mingwan.");
        foreach (var peer in new[] { 1, 2, 3 }) { Use(g, "recover", peer); Play(g); }
        Slash(g, 1); Play(g);
        Require(E<ShenwuMingwanVictimRecordedEvent>(g).Single().VictimSeat == 1, "An actual play-phase Slash arms Mingwan before the turn ends.");
        var damageBeforeEnd = E<DamageAppliedEvent>(g).Count();
        Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
        Until(g, () => g.ResolutionStack.OfType<CardUseFrame>().Any(f => f.VirtualOrdinaryTrickOrigin is not null));
        var end = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.VirtualOrdinaryTrickOrigin is not null);
        Require(end.CardKind == CardKind.ArrowBarrage && end.CardId == 0 && end.TargetSeats.SequenceEqual([1, 2, 3]) && E<TurnEndedEvent>(g).Last().ActorSeat == 0 &&
            g.ResolutionStack.OfType<DeferredTurnEndFrame>().Any(f => f.OwnerSeat == 0) &&
            g.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WindowContext?.Window == SkillProgramTriggerWindow.AfterTurnEnded),
            "Luanchang's native arrows are issued after actual turn completion, while retaining the deferred-turn parent.");
        Cold(g, registry);
        Until(g, () => E<CardUseFinishedEvent>(g).Any(e => e.ResolutionId == end.Id));
        Require(E<DamageAppliedEvent>(g).Skip(damageBeforeEnd).Count(e => e.SourceSeat == 0 && e.Amount == 1) == 3 &&
            !g.CardMovements.Any(m => m.To == CardLocation.Hand(0) && m.Reason.Value.Contains("DrawShenwuMingwanUse", StringComparison.Ordinal)),
            "After the actual TurnEnded, Mingwan no longer restricts the arrows' three recipients or draws for the outside-turn use.");
        Cold(g, registry);
    }

    private static (GameEngine, ContentRegistry) Start(string[] ownerSkills, string[] peerSkills, string card = "standard:slash", bool beginPlay = true, bool godPeers = false, bool minionPeer = false, bool expanded = false, Suit suit = Suit.Heart)
    {
        var packages = new List<IGameContentPackage> { new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShenwuActivityPackage() };
        if (expanded) packages.Add(new StandardShenwu2020ActivityPackage());
        packages.Add(new Fixture(ownerSkills, peerSkills, card, godPeers, minionPeer, suit));
        var registry = ContentRegistry.Build(packages.ToArray());
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = "team:blue", ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(g, new StartGameCommand()); Until(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, "fixture:shenwu-npc-actor", g.Revision, P(g)!.PromptId));
        if (beginPlay) Play(g);
        return (g, registry);
    }
    private static void Use(GameEngine g, string id, int target) => Accept(g, new UseProgramSkillCommand(0, Driver, id, [], [target], g.Revision, P(g)!.PromptId));
    private static void Slash(GameEngine g, int target)
    {
        var a = g.GetHumanLegalActions().First(a => a.CardId is { } id && V(g, 0).Hand.Any(c => c.Id == id && c.Kind == CardKind.Slash) && a.TargetSeats.SequenceEqual([target]));
        Accept(g, new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId, a.PlayedCardKind, a.TargetCardId));
    }
    private static PlayerSnapshot V(GameEngine g, int seat) => g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(seat => g.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static void Play(GameEngine g) => Until(g, () => P(g) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Answer(GameEngine g, Func<PromptChoice, bool> predicate)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, g.Revision)); }
    private static void Until(GameEngine g, Func<bool> condition)
    {
        for (var i = 0; i < 220; i++)
        {
            if (condition()) return;
            if (P(g) is { PlayerSeat: 0 } human && human.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass")) Answer(g, c => c.Parameters.GetValueOrDefault("response") == "pass");
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Shenwu NPC fixture missed its native boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static void Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 4).All(s => SnapshotJson.Serialize(g.CreateSnapshot(s)) == SnapshotJson.Serialize(restored.CreateSnapshot(s))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(restored.CardMovements), "NPC rule choices, events and native movements replay from accepted commands.");
    }
    private static void Accept(GameEngine g, GameCommand command) { var result = g.Submit(command); Require(result.Accepted, result.Error?.Message ?? "NPC command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(string[] ownerSkills, string[] peerSkills, string card, bool godPeers, bool minionPeer, Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("shenwu-npc-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var woundedPrelude = ownerSkills.Contains("shenwu:luanchang")
                ? ",{\"id\":\"wounded-native-response-fixture\",\"window\":\"gameStarting\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":2}]}"
                : "";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"viewAs":[{"id":"real-material-slash","inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false},{"id":"fixed-dismantlement","inputKinds":[],"inputSuits":[],"outputKind":"dismantlement","forPlay":true,"forResponse":false,"singleCardTrickUse":true}],"activations":[
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                {"id":"one-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"fire","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"fire"}]},
                {"id":"foreign-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":2,"sourceRef":{"kind":"selectedTarget"} }]},
                {"id":"foreign-one-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1,"sourceRef":{"kind":"selectedTarget"} }]},
                {"id":"recover","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"selectedTarget","amount":2}]},
                {"id":"turn-over","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"turnOver","target":"selectedTarget"}]},
                {"id":"discard","minCards":1,"maxCards":1,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipment"}]}]},
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"skip-play","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}{{{woundedPrelude}}}]},
              {"id":"{{{NonLockedReward}}}","revision":1,"triggers":[{"id":"after-damage-draw","window":"afterDamageApplied","subject":"owner","optional":false,"damageOccurrence":"perDamage","effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, NonLockedReward }.ToDictionary(id => id, id => new { name = id, description = "Native Shenwu NPC rule fixture." }) }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "Native Shenwu NPC rule fixture.") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new("fixture:shenwu-npc-actor", "NPC rule actor", "", Driver, AdditionalSkillIds: ownerSkills));
            for (var i = 1; i < 4; i++) b.AddGeneral(new($"fixture:shenwu-npc-peer-{i}", $"NPC rule peer {i}", "", Quiet, FactionId: godPeers ? "god" : "qun", AdditionalSkillIds: peerSkills));
            b.AddDeck(new("fixture:shenwu-npc-deck", "Real uniform cards and weapon", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 180).Select(i => new ContentDeckPhysicalCard(i < 160 ? card : "standard:qinggang_sword", suit, 7)).ToArray() });
            b.AddMode(new(Mode, "Native NPC rule fixture", 4, 4, new Dictionary<string, int>(), "fixture:shenwu-npc-deck", 4,
                ["fixture:shenwu-npc-actor", minionPeer ? "shenwu:zhuyin-left" : "fixture:shenwu-npc-peer-1", "fixture:shenwu-npc-peer-2", "fixture:shenwu-npc-peer-3"], ContentModeKind.Team,
                new Dictionary<string, int> { ["team:blue"] = minionPeer ? 1 : 2, ["team:red"] = minionPeer ? 3 : 2 }));
        }
    }
}
