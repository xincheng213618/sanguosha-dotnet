using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class AnniversaryMaTieChecks
{
    private const string Hero = "anniversary:ma-tie", Quxian = "anniversary:quxian", Quiet = "fixture:ma-tie-quiet", Watch = "fixture:ma-tie-reward-watch", Deck = "fixture:ma-tie-deck";
    private static string Peer(int seat) => "fixture:ma-tie-peer-" + seat;
    private static string? Branch(PromptChoice c) => c.Parameters.GetValueOrDefault("branch");

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.State.Players.Count).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().FirstOrDefault(f => f.AnniversaryMaTie is { RewardParent: 0 });
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Quxian && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Menu(AnniversaryMaTieStage stage) => Reach(() => Root?.AnniversaryMaTie?.Stage == stage && P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "anniversary-ma-tie") == true);
        public void Watch() => Reach(() => P?.SkillPrompt?.SkillId == AnniversaryMaTieChecks.Watch);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Card(int id, int[] targets)
        {
            var a = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == id && a.TargetSeats.SequenceEqual(targets) && a.Kind != LegalActionKind.Recast);
            Need(a is not null, "The fixed physical card has its exact canonical native target action.");
            Accept(new PlayCardCommand(0, id, a!.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> match)
        {
            var p = P!; var c = p.Choices.FirstOrDefault(match); Need(p.PlayerSeat == 0 && c is not null, "The current human has the exact original published Ma Tie choice.");
            Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision));
        }
        public void Accept(GameCommand command)
        {
            try { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native Ma Tie command."); }
            catch (Exception e) { throw new InvalidOperationException(e.Message + Dump(), e); }
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 200; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification) { Answer(c => c.Cards.Count == 0); continue; }
                    throw new InvalidOperationException("Unexpected Ma Tie native human boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The short fixed Ma Tie scene retains its real participants."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Ma Tie did not reach its bounded native continuation.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, G.State.Players.Count).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold preserves the original unpublished native choice or already-paid reward child exactly."); }
        public void Replay() { var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry); Need(before == State(), "The two actual boundaries, native requested uses, rewards, HP payments and directed distance replay exactly."); }
        public void Need(bool yes, string text) { if (!yes) throw new InvalidOperationException(text + Dump()); }
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, P, Receipt = Root?.AnniversaryMaTie,
            Views = Enumerable.Range(0, G.State.Players.Count).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }), Last = G.Events.TakeLast(12).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
        public static Scene Start(bool terminal = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(terminal));
            var setup = new ShanheBattleSetup(7, 1, "anniversary-ma-tie", Hero, 4, 0, 0, 0, 0, [AnniversaryMaTieChecks.Watch, "ol:weijing"],
                [new(CardKind.IronChain, Suit.Club, 12)], [],
                terminal ? [new(Peer(1), Hp: 1) { StartingHand = [] }] : [new(Peer(2), Hp: 12) { StartingHand = [] }, new(Peer(3), Hp: 12) { StartingHand = [] }],
                terminal ? [] : [new(Peer(1), Hp: 12) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] }], new string('d', 64));
            var g = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = terminal ? 2 : 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(bool terminal = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-ma-tie", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "ol:lu-zhi-wei");
            if (terminal) CharacterFixtureContent.Register(b, "ol:du-fu-ren");
            var c = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-reward","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.anniversary:quxian.used-slash-draw"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Quiet] = new { name = "固定安静同桌", description = "跳过主动出牌，保留真实回合流程。" },
                [Watch] = new { name = "真实驱险摸牌子窗", description = "在原生摸牌实际付款后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            for (var i = 1; i < 4; i++) b.AddGeneral(new(Peer(i), "固定安静同桌", "", Quiet, "wei", 12));
            b.AddDeck(new(Deck, "驱险固定实体杀牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 8)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "马铁真实双边界与原生用杀", terminal ? 2 : 4, terminal ? 2 : 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: terminal ? [Hero, Peer(1)] : [Hero, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = terminal ? 1 : 2, [ShanheBattleSetup.EnemyTeamId] = terminal ? 1 : 2 }));
        }
    }
}
