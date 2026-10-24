using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryGuanPingChecks
{
    private const string Hero = "boundary:guan-ping", Jieyong = "boundary:jieyong", Longyin = "classic:longyin",
        Friend = "fixture:boundary-guan-ping-friend", Peer = "fixture:boundary-guan-ping-peer",
        Quiet = "fixture:boundary-guan-ping-quiet", Driver = "fixture:boundary-guan-ping-driver", Hold = "fixture:boundary-guan-ping-gain";

    public static void UniqueRedHandUsePlayedAndLongyinPaidCold()
    {
        var s = Scene.Start(); s.Play();
        var red = s.V().Hand.Single(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Heart).Id;
        var gift = s.V().Hand.Single(c => c.Kind == CardKind.DrawTwo).Id;
        var horse = s.V().Equipment.Single().Id;
        var blackSlash = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        const int victim = 3; // Adjacent enemy: ordinary Slash range still applies to Jieyong.
        s.Need(s.G.GetHumanLegalActions().All(a => a.ConversionSource?.SkillId != Jieyong), "Two red Hand entities do not satisfy last-red Jieyong.");
        s.Drive("give-red", [1]); s.Select(gift); s.Play();
        var converted = s.G.GetHumanLegalActions().Where(a => a.CardId == red && a.ConversionSource?.SkillId == Jieyong).ToArray();
        s.Need(converted.Any(a => a.TargetSeats.SequenceEqual([victim])), "The unique red conversion must expose an ordinary in-range enemy target: " + JsonSerializer.Serialize(converted));
        var attack = converted.Single(a => a.TargetSeats.SequenceEqual([victim]));
        s.Need(s.V().Hand.Count == 3 && s.V().Hand.Count(c => c.Suit is Suit.Heart or Suit.Diamond) == 1 &&
            s.G.GetHumanLegalActions().All(a => a.CardId != horse || a.ConversionSource?.SkillId != Jieyong),
            "One red Hand card is convertible with two black Hand cards and a red Equipment card; equipment itself is not Jieyong material.");
        s.Native(attack); s.Offer(Longyin); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Select(horse);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        var mother = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Longyin);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Action?.PhysicalCards.Any(c => c.CardId == red) == true);
        s.Need(s.Zone(horse) == CardLocation.DiscardPile && s.V(victim).Hp == 8 && s.Facts<CardUseDebitRefundedEvent>().Length == 1 &&
            s.Facts<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == 1 &&
            use.Action!.ConversionChain.Any(c => c.SkillId == Jieyong) && mother.WindowContext?.CardUse?.ParentCardUseFrameId == use.Id,
            "The genuine converted red Slash keeps its original native Use, HE Longyin payment, once-refunded debit and actual red-card draw before target damage.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(victim).Hp == 7 && s.G.CardMovements.Count(m => m.CardId == horse && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.Facts<CardUseDebitRefundedEvent>().Length == 1 && s.G.GetHumanLegalActions().Any(a => a.CardId == blackSlash && a.Kind == LegalActionKind.Slash && a.ConversionSource is null),
            "Cold finishes the actual paid Longyin child once, then resolves the original Slash; the ordinary next Slash is legal because its debit was refunded.");
        var nextRed = s.V().Hand.Single(c => c.Suit is Suit.Heart or Suit.Diamond).Id;
        s.Drive("duel", [1, 0]); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        s.Answer(c => c.Cards.SequenceEqual([nextRed]) && Conversion(c) == Jieyong); s.Play();
        s.Need(s.Facts<DuelResponseEvent>().Any(e => e.ResponderSeat == 0 && e.UsedSlash) &&
            s.Facts<CardActionAcceptedEvent>().Any(e => e.Action.Type == CardActionType.Response && e.Action.ActorSeat == 0 && e.Action.EffectiveKind == CardKind.Slash && e.Action.ConversionChain.Any(c => c.SkillId == Jieyong)) &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Longyin && e.Activated && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == nextRed && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The next unique red card really plays Slash in the original Duel, pays once and adds no Use-triggered Longyin invocation.");
        s.Drive("draw", []); s.Play();
        s.Need(s.G.GetHumanLegalActions().Any(a => a.ConversionSource?.SkillId == Jieyong), "A later real gain restores the unique-red conversion without a turn or phase quota.");
        s.Drive("draw", []); s.Play();
        s.Need(s.G.GetHumanLegalActions().All(a => a.ConversionSource?.SkillId != Jieyong), "A second real red gain immediately removes Jieyong's dynamic permission.");
    }

    private static string? Conversion(PromptChoice c) => c.Parameters.GetValueOrDefault("conversion-skill-id") ?? c.Parameters.GetValueOrDefault("conversion-skill");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Select(int id) { Reach(() => P?.Choices.Any(c => c.Cards.SequenceEqual([id])) == true); Answer(c => c.Cards.SequenceEqual([id])); }
        public void Drive(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, BoundaryGuanPingChecks.Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Native(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 120; i++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human boundary in native Guan Ping fixture.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native Guan Ping fixture exceeded its fixed boundary steps.");
        }
        public void Cold()
        {
            var checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves Longyin's original paid Use child and Jieyong's exact native physical conversion."); G = restored;
        }
        private void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected Guan Ping fixture command."); }
        public void Need(bool value, string message)
        {
            if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "ol-boundary-guan-ping", Hero, 8, 4, 0, 0, 0, [Quiet, Driver, Hold],
                [new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Diamond, 8), new(CardKind.Slash, Suit.Club, 6), new(CardKind.Dodge, Suit.Spade, 4)],
                [new(CardKind.DefensiveHorse, Suit.Heart, 3)],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
                [new(Friend, Hp: 8) { StartingHand = [] }], new string('f', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-guan-ping", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:guan-ping");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Hold}}","revision":1,"triggers":[{"id":"gain-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-child","options":[{"id":"continue"}]}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"give-red","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"cardKinds":["drawTwo"],"count":1,"destination":"selectedTargetHand","awaitMovementTriggers":true}]},
                {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]},
                {"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]}
              ]}
            ]}
            """;
            var presentations = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Driver }) presentations[id] = new { name = id, description = "原生唯红转换与龙吟实际流程。" };
            presentations[Hold] = new { name = Hold, description = "龙吟已支付并摸牌后的原母帧子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Friend, "界关平同伴", "", Quiet, "shu", 8));
            b.AddGeneral(new(Peer, "界关平同桌", "", Quiet, "wei", 8));
            const string deck = "fixture:boundary-guan-ping-deck";
            b.AddDeck(new(deck, "固定红闪库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 9)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "OL界关平原生转换", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Friend, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
