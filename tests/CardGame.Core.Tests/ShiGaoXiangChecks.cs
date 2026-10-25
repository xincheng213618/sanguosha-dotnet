using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiGaoXiangChecks
{
    private const string Hero = "mobile:shi-gao-xiang", Chiyuan = "mobile:shi-chiyuan", Erdi = "mobile:shi-erdi",
        Dry = "fixture:shi-gao-xiang-dry", Keeper = "fixture:shi-gao-xiang-keeper",
        Deck = "fixture:shi-gao-xiang-deck", Peer = "fixture:shi-gao-xiang-peer";

    public static void ChiyuanDamagedSlashHealsTheReceiver()
    {
        var s = Scene.Start("chiyuan-heal", 4, [Keeper],
            [new Enemy(3, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.Play();
        s.PlayHandSlash(1);
        s.Need(s.V(1).Hp == 2,
            "The opening slash wounded the future receiver to two.");
        s.ActivateChiyuan(s.V(0).Hand.First(c => c.Kind == CardKind.Slash).Id);
        s.Reach(() => s.E<ShiZxChiyuanGivenEvent>() is [{ OwnerSeat: 0, ReceiverSeat: 1 }] &&
            s.E<ShiZxChiyuanSlashIssuedEvent>() is [{ TargetSeat: 2, UseId: > 0 }]);
        var issued = s.E<ShiZxChiyuanSlashIssuedEvent>()[0];
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2 && e.SourceCardUseResolutionId == issued.UseId) &&
            s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 1 && e.Amount == 1));
        s.Need(s.G.CardMovements.Any(m => m.CardId == s.E<ShiZxChiyuanGivenEvent>()[0].CardId &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(1) &&
                m.Reason.Value == "skill-program.mobile:shi-chiyuan.give"),
            "The gift travelled from the owner's hand to the receiver with its skill reason.");
        s.Need(s.E<DamageAppliedEvent>().Any(e => e.SourceCardUseResolutionId == issued.UseId &&
                e.TargetSeat == 2 && e.SourceSeat == 0 && e.Amount == 1) && s.V(2).Hp == 7,
            "The virtual Slash hit the range-threatened target for one damage.");
        s.Need(s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 1 && e.Amount == 1) && s.V(1).Hp == 3,
            "The damaged branch recovered the receiver to full.");
        s.Need(!s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0),
            "The damaged branch spared the owner the retaliation Slash.");
        s.Cold(); s.Replay();
    }

    public static void ChiyuanUndamagedSlashRetaliatesAgainstTheOwner()
    {
        var s = Scene.Start("chiyuan-revenge", 4, [Keeper],
            [new Enemy(3, [Dry]), new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)]);
        s.Play();
        s.PlayHandSlash(1);
        s.ActivateChiyuan(s.V(0).Hand.First(c => c.Kind == CardKind.Slash).Id);
        s.Reach(() => s.E<ShiZxChiyuanSlashIssuedEvent>() is [{ TargetSeat: 2, UseId: > 0 }]);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0 && e.SourceSeat == 2));
        s.Need(!s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2),
            "The dodging target kept the rescue Slash from drawing blood.");
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0 && e.SourceSeat == 2));
        s.Need(s.V(0).Hp == 3,
            "The undamaged branch made the target use a virtual Slash back at the owner.");
        s.Need(s.V(1).Hp == 2 && !s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 1),
            "The receiver stayed wounded without the heal branch.");
        s.Cold(); s.Replay();
    }

    public static void ErdiComparesSimultaneousDiscardCounts()
    {
        var s = Scene.Start("erdi-compare", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6)] },
                new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 7),
                    new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 8), new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 9)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5)]);
        s.ErdiInvokeBranch = "activate";
        s.Play();
        s.End();
        s.Reach(() => s.E<ShiZxErdiStartedEvent>() is [{ OwnerSeat: 0, TargetSeat: 2, Turn: 1 }] &&
            s.E<ShiZxErdiDiscardedEvent>() is [{ Seat: 0, Count: 2 }, { Seat: 2, Count: 1 }] &&
            s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2 && e.SourceSeat == 0));
        s.Need(s.E<ShiZxErdiDiscardedEvent>() is [{ Seat: 0, Count: 2 }, { Seat: 2, Count: 1 }] &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:shi-erdi.discard") == 3,
            "The owner's two cards beat the opponent's single simultaneous discard.");
        s.Need(s.V(0).Hp == 4 && s.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 0 && e.Amount == 1) &&
            s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 2 && e.SourceSeat == 0 && e.Amount == 1) && s.V(2).Hp == 7,
            "Winning the comparison recovered the lost HP and punished the target.");
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.Need(s.V(0).HandCount == 0 && s.V(2).HandCount == 2,
            "The settled turn left no owner cards and two opponent cards.");
        s.ErdiOwnerPicks = false;
        s.End();
        s.Reach(() => s.E<ShiZxErdiStartedEvent>().Count(e => e.Turn == 6) == 1 &&
            s.E<ShiZxErdiDiscardedEvent>().Length == 4);
        s.Need(s.E<ShiZxErdiDiscardedEvent>() is [{ Seat: 0, Count: 2 }, { Seat: 2, Count: 1 }, { Seat: 0, Count: 0 }, { Seat: 2, Count: 1 }],
            "The second comparison recorded zero owner discards against one opponent discard.");
        s.Need(s.V(0).Hp == 3 && s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 2 && e.SourceSeat == 0 && e.Amount == 1) == 1 &&
            s.V(0).HandCount == 2,
            "Losing the comparison kept the skill draw and drew the opponent's single count without a heal.");
        s.Cold(); s.Replay();
    }

    private sealed record Enemy(int Hp, string[] Extras)
    {
        public ShanheStartingCard[] StartingHand { get; init; } = [];
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Offered(string skill) =>
            G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == skill);

        public string ErdiInvokeBranch { get; set; } = "activate";
        public string ErdiTargetBranch { get; set; } = "seat-2";
        public bool ErdiOwnerPicks { get; set; } = true;

        public void ActivateChiyuan(int cardId)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Offered(Chiyuan));
            Accept0(new UseProgramSkillCommand(0, Chiyuan, "chiyuan-gift", [cardId], [], G.Revision, P!.PromptId));
        }

        public void PlayHandSlash(int targetSeat)
        {
            var slashId = V(0).Hand.Where(card => card.Kind == CardKind.Slash)
                .Select(card => card.Id).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat)));
            var a = G.GetHumanLegalActions().First(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }

        public void End()
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Accept0(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 2600; i++)
            {
                if (ready()) return;
                if (P is { } p)
                {
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Chiyuan or Erdi &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("seat-", StringComparison.Ordinal) == true))
                    {
                        var isErdi = p.SkillPrompt.SkillId == Erdi;
                        Answer(c => isErdi
                            ? c.Parameters.GetValueOrDefault("branch") == ErdiTargetBranch
                            : c.Parameters.GetValueOrDefault("branch")?.StartsWith("seat-", StringComparison.Ordinal) == true);
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Erdi &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") is "activate" or "skip"))
                    { Answer(c => c.Parameters.GetValueOrDefault("program-action") == ErdiInvokeBranch); continue; }
                    if (p.PlayerSeat == 0 && ErdiOwnerPicks && p.SkillPrompt?.SkillId == Erdi &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("pick-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("pick-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Erdi &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "confirm"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == "confirm"); continue; }
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Gao Xiang boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Gao Xiang did not reach the expected bounded native boundary.");
        }

        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "The expected prompt choice must be published.");
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }

        public void Cold()
        { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold restore preserves exact phase identity and ledger facts."); }
        public void Replay()
        {
            var before = State(); G = GameReplay.Replay(G.CreateCheckpoint().Options, G.AcceptedCommands, registry);
            Need(State() == before, "The short actual command journal reproduces the activations exactly.");
        }
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Dump()); }
        private void Accept0(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Gao Xiang command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).MaxHp, V(i).HandCount }),
            Given = E<ShiZxChiyuanGivenEvent>(), Issued = E<ShiZxChiyuanSlashIssuedEvent>(),
            Erdi = E<ShiZxErdiStartedEvent>(), Discarded = E<ShiZxErdiDiscardedEvent>(),
            Recovery = E<RecoveryAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.SourceSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies,
            ShanheStartingCard[]? openingOverride = null, int maxTurns = 9)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = openingOverride ?? Enumerable.Range(0, 4)
                .Select(i => i == 3
                    ? new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)
                    : new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: enemy.Extras)
            { StartingHand = enemy.StartingHand };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, 0, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = maxTurns }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-gao-xiang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Dry}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Keeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Dry] = new { name = Dry, description = "只跳过普通摸牌，保留真实出牌阶段。" },
                [Keeper] = new { name = Keeper, description = "保持手牌。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Dry, "wei", 8));
            b.AddCard(new("fixture:shi-gao-xiang-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-gao-xiang-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-gao-xiang-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 4 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-gao-xiang-dodge", Suit.Heart, 6)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-gao-xiang-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-gao-xiang-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势高翔实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
