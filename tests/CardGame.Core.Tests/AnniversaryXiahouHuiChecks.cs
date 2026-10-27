using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static partial class AnniversaryXiahouHuiChecks
{
    private const string Hero = "anniversary:xiahou-hui", Dujun = "anniversary:dujun", Jikun = "anniversary:jikun",
        Driver = "fixture:xiahou-hui-driver", Counter = "fixture:xiahou-hui-counter", Watch = "fixture:xiahou-hui-watch",
        Quiet = "fixture:xiahou-hui-quiet", Peer = "fixture:xiahou-hui-peer", Partner = "fixture:xiahou-hui-partner", Deck = "fixture:xiahou-hui-deck";
    private static string? Key(PromptChoice c) => c.Parameters.GetValueOrDefault("program-action");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.XiahouHui is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Offer(string skill, string binding) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => Key(c) == "activate" && c.Parameters.GetValueOrDefault("binding-id") == binding));
        public bool PreparePair(string invoke, string skip)
        {
            Reach(() => P?.SkillPrompt?.SkillId == Dujun && P.Choices.Any(c => Key(c) == "activate" && c.Parameters.GetValueOrDefault("binding-id") is { } id && (id == invoke || id == skip)));
            var skippedEarly = P!.Choices.Any(c => Key(c) == "activate" && c.Parameters.GetValueOrDefault("binding-id") == skip);
            if (skippedEarly) Skip();
            Offer(Dujun, invoke); Activate(); Menu(); return skippedEarly;
        }
        public void CompletePair(string skipped, bool skippedEarly) { if (!skippedEarly) { Offer(Dujun, skipped); Skip(); } }
        public void Menu() => Reach(() => Root?.XiahouHui?.Stage == XiahouHuiStage.Recipient && P?.Choices.Any(c => Key(c) == "anniversary-xiahou-hui") == true);
        public void GiftChild() => Reach(() => P?.SkillPrompt?.SkillId == Watch && Root?.XiahouHui?.Stage == XiahouHuiStage.MovementChildren);
        public void DriverDone() => Reach(() => P?.SkillPrompt?.SkillId == Driver && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "done"));
        public void Activate() => Answer(c => Key(c) == "activate");
        public void Skip() => Answer(c => Key(c) == "skip");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") is "continue" or "done");
        public void Use(string id, int[]? cards = null) => Accept(new UseProgramSkillCommand(0, Driver, id, cards ?? [], [], G.Revision, P!.PromptId));
        public int Answers(string skill, string binding, string action) => G.AcceptedCommands.OfType<AnswerPromptCommand>().Count(c => c.Choice.Value.StartsWith($"program-trigger.{action}.{skill}.{binding}.", StringComparison.Ordinal));
        public void Card(int id, int? target = null)
        {
            var a = G.GetHumanLegalActions().First(a => a.CardId == id && (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "The actual original Xiahou Hui choice must be published.");
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 320; i++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectFaction }) { Answer(_ => true); continue; }
                if (P is { PlayerSeat: 0 }) throw new InvalidOperationException("Unexpected Xiahou Hui native boundary. " + Dump());
                Need(G.State.Winner == Winner.None, "The short original native scenario retains all participants."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Xiahou Hui did not reach her bounded native continuation.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "The exact already-paid native whole-gift, hidden drawn entities and movement child cold-restore identically."); }
        public void Replay() { var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry); Need(State() == before, "The original partner, four first-damage roles, drawn-card gifts and cumulative random obtain thresholds replay identically."); }
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, P, Root = Root?.XiahouHui,
            Views = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }), Loss = E<XiahouHuiLossRecordedEvent>(),
            Last = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + Dump()); }
        private void Accept(GameCommand c) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected genuine Xiahou Hui command."); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(47, 1, "anniversary-xiahou-hui", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
                [new(CardKind.Crossbow, Suit.Spade, 5), new(CardKind.Slash, Suit.Heart, 6), new(CardKind.Dodge, Suit.Club, 7), new(CardKind.Dodge, Suit.Diamond, 8)], [],
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [new(Partner) { StartingHand = [] }], new string('e', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, AiPolicyVersion = AiDecisionPolicy.CurrentVersion, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-xiahou-hui", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var catalog = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[
                {"id":"discard-one","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                {"id":"stock-and-discard","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":10},{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]}],
                "triggers":[{"id":"owner-ending-watch","window":"playEnding","subject":"owner","optional":false,"isSkillInvocation":false,"priority":100,"effects":[{"op":"chooseOption","target":"owner","resultBind":"done","options":[{"id":"done"}]}]}]},
              {"id":"{{{Counter}}}","revision":1,"triggers":[{"id":"two-real-partner-slashes","window":"playEnding","subject":"owner","turnOwnerScope":"otherLiving","optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,"priority":0,"effects":[
                {"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"useVirtualSlash","target":"selectedTarget"},{"op":"useVirtualSlash","target":"selectedTarget"}]}]},
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"real-gift-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.anniversary:dujun.give-drawn"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实失牌及原回合停点", description = "原生弃置、摸牌和实际出牌阶段结束。", optionLabels = new Dictionary<string, string> { ["done"] = "继续原回合" } },
                [Counter] = new { name = "所选角色的两次原生杀", description = "实际使用两张零实体杀，正常进入响应与伤害。" },
                [Quiet] = new { name = "安静同桌", description = "保留真实回合，跳过主动出牌。" },
                [Watch] = new { name = "实际整包交牌子窗", description = "已付实际交牌后的原生移动观察。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "真实安静同桌", "", Quiet, "wei", 10));
            b.AddGeneral(new(Partner, "真实笃君同伴", "", Quiet, "wei", 10) { AdditionalSkillIds = [Counter] });
            b.AddDeck(new(Deck, "固定原生摸牌库存", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "夏侯徽真实首伤与失牌五阈值", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer, Partner], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
