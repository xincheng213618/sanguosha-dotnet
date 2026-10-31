using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class SpPangTongRuntimeChecks
{
    private const string Hero = "ol:pang-tong-sp", Driver = "fixture:sp-pt-driver", Quiet = "fixture:sp-pt-quiet",
        Watch = "fixture:sp-pt-watch", Peer = "fixture:sp-pt-peer", Deck = "fixture:sp-pt-deck";

    public static void GuolunExchangeReturnsOriginalPaidChildrenCold()
    {
        var s = Scene.Start(); s.Play();
        var originalTarget = s.V(2).Hand.Select(c => c.Id).ToArray();
        s.Use("ol:guolun", "reveal-exchange", [2]);
        s.Need(s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "reveal") == true,
            "Guolun starts its real selected other hand reveal.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "reveal");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "exchange") == true);
        var revealed = s.E<GuolunRevealedEvent>().Single();
        var foreign = s.V(2).Hand.Single(c => c.Id == revealed.CardId);
        var offered = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "exchange" &&
            s.V(0).Hand.Single(card => card.Id == c.Cards.Single()).Rank != foreign.Rank);
        var own = s.V(0).Hand.Single(c => c.Id == offered.Cards.Single());
        var reward = own.Rank < foreign.Rank ? 0 : foreign.Rank < own.Rank ? 2 : (int?)null;
        s.Answer(c => c.Id == offered.Id);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SpPangTong is { Stage: "exchange-children" }));
        var paid = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SpPangTong is not null);
        s.Need(originalTarget.Contains(revealed.CardId) && paid.SpPangTong!.OwnCard == own.Id &&
            paid.SpPangTong.RevealedCard == foreign.Id && paid.PendingMovementContinuation is not null &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.guolun.exchange") == 2,
            "Both real exchange movements are already paid before the native gain child, on the original Guolun frame.");
        s.Cold(); s.Play();
        s.Need(s.V(0).Hand.Any(c => c.Id == foreign.Id) && s.V(2).Hand.Any(c => c.Id == own.Id) &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.guolun.exchange") == 2 &&
            s.E<SpPangTongSegmentPaidEvent>().Count(e => e.FrameId == paid.Id && e.Stage == "exchange-children") == 1 &&
            s.E<SpPangTongSegmentPaidEvent>().Count(e => e.FrameId == paid.Id && e.Stage == "draw-children") == (reward is null ? 0 : 1) &&
            (reward is null || s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.sp-pang-tong.draw" && m.To == CardLocation.Hand(reward.Value)) == 1),
            "Cold return retains the original paid exchange, then draws once for the frozen smaller rank, without repeating either card movement.");
    }

    public static void SongsangHealthBranchesGrantZhanjiAndExcludeItsOwnDraw()
    {
        foreach (var wounded in new[] { false, true })
        {
            var s = Scene.Start(); s.Play();
            if (wounded) { s.Use(Driver, "wound", []); s.Play(); }
            var hp = s.V(0).Hp; var max = s.V(0).MaxHp;
            s.Use(Driver, "kill", [2]);
            s.Reach(() => s.P?.SkillPrompt?.SkillId == "ol:songsang");
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play();
            s.Need(!s.V(2).IsAlive && s.V(0).Hp == hp + (wounded ? 1 : 0) &&
                s.V(0).MaxHp == max + (wounded ? 0 : 1) &&
                s.E<SongsangHealthIssuedEvent>() is [{ Wounded: var frozen }] && frozen == wounded &&
                s.V(0).Skills!.Any(skill => skill.ContentId == "ol:zhanji"),
                "Songsang resolves its actual other death, preserves the original wounded/maximum-HP branch, and really grants Zhanji.");
            var before = s.V(0).Hand.Count;
            s.Use(Driver, "draw-two", []); s.Play();
            var bonus = s.E<SpPangTongSegmentPaidEvent>().Where(e => e.Source.SkillId == "ol:zhanji").ToArray();
            s.Need(s.V(0).Hand.Count == before + 3 && bonus is [{ Stage: "draw-children", Count: 1, DrawInvocation: not null }] &&
                s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                    m.Reason.Value == "skill-program.sp-pang-tong.draw") == 1,
                "One actual own Play draw invocation of two cards earns exactly one Zhanji card; that skill's own physical draw cannot recurse.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sp-pang-tong-runtime", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"draw-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":2}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","isSkillInvocation":false,"window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"exchange-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.guolun.exchange"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = Driver, description = "真实原生伤害、失去体力和摸牌" },
                    [Quiet] = new { name = Quiet, description = "固定同桌跳过出牌阶段" },
                    [Watch] = new { name = Watch, description = "已交换后的真实获得牌子窗", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, id, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "固定同桌", "", Quiet, "wei", 20, [Watch]));
            b.AddDeck(new(Deck, "固定真实闪牌", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 72)
                .Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "SP庞统原生返回", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(v => G.CreateSnapshot(v).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string skill, string activation, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, skill, activation, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The native fixture must reach its real boundary before game end.");
                if (P?.SkillPrompt?.SkillId == Watch) Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                else if (P is not { PlayerSeat: 0 }) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (P.Kind == DecisionKind.PlayCard) Accept(new EndPlayPhaseCommand(0, G.Revision, P.PromptId));
                else if (P.Kind == DecisionKind.RescueDying) Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die");
                else if (P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected SP Pang Tong boundary " + JsonSerializer.Serialize(P));
            }
            throw new InvalidOperationException("SP Pang Tong did not reach the bounded native boundary " + JsonSerializer.Serialize(P));
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(v => SnapshotJson.Serialize(G.CreateSnapshot(v))).ToArray(),
            Frames = G.ResolutionStack, Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Moves = G.CardMovements });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold restore preserves every original paid frame, movement invoice and published child choice."); }
        public void Accept(GameCommand command) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected command."); }
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, P })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(37, 1, "sp-pang-tong-runtime", Hero, 3, 0, 0, 0, 0, [Driver], [], [],
                [new(Peer), new(Peer)], [new(Peer)], new string('a', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
}
