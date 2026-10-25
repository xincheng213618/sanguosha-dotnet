using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiHuangChongChecks
{
    private const string Hero = "mobile:shi-huang-chong", Cujun = "mobile:shi-cujun", Sili = "mobile:shi-sili",
        Dry = "fixture:shi-huang-chong-dry", Peer = "fixture:shi-huang-chong-peer",
        Deck = "fixture:shi-huang-chong-deck";

    public static void CuJunCoordinatesGiftsSlashQuotaAndDiscardClaim()
    {
        var s = Scene.Start("cujun-coordinate", 4, [],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                    new ShanheStartingCard(CardKind.Slash, Suit.Club, 12)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride:
            [
                new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6), new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 7),
                new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6), new ShanheStartingCard(CardKind.Slash, Suit.Club, 4),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 8), new ShanheStartingCard(CardKind.Slash, Suit.Club, 9),
            ]);
        s.CujunInvoke = true;
        s.Play();
        s.End();
        s.Reach(() => s.E<ShiHcCujunStartedEvent>() is [{ OwnerSeat: 0, TargetSeat: 1 }] &&
            s.E<ShiHcCujunGiftMovedEvent>() is [{ ChooserSeat: 0, TargetSeat: 1 }] &&
            s.E<ShiHcCujunSlashGrantedEvent>() is [{ ChooserSeat: 2, TargetSeat: 1 }]);
        s.Need(s.E<ShiHcCujunGiftMovedEvent>() is [{ ChooserSeat: 0, TargetSeat: 1 }] &&
            s.E<ShiHcCujunSlashGrantedEvent>() is [{ ChooserSeat: 2, TargetSeat: 1 }],
            "The hero gifted one hand card while the in-range neighbour raised the target's Slash quota.");
        s.Reach(() => s.E<ShiHcCujunClaimIssuedEvent>() is [{ OwnerSeat: 0, TargetSeat: 1, TurnNumber: 1 }] &&
            s.E<ShiHcCujunClaimSettledEvent>() is [{ OwnerSeat: 0, TargetSeat: 1 }] &&
            s.E<ShiHcCujunClaimSettledEvent>()[0].CardIds.Count == 1);
        var settled = s.E<ShiHcCujunClaimSettledEvent>()[0];
        s.Need(s.G.CardMovements.Any(m => m.CardId == settled.CardIds[0] &&
                m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(1) &&
                m.Reason.Value == "skill-program.mobile:shi-cujun.claim"),
            "The discarded damage card travelled from the discard pile to the poorer target with its claim reason.");
        s.Need(s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:shi-cujun.gift" &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(1)) == 1,
            "The gift travelled from the owner's hand to the target with its skill reason.");
        s.Need(s.V(1).HandCount == 4,
            "The target held its two dealt Slashes plus the gift and the claimed damage card.");
        s.Reach(() => s.E<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 1 && e.CardKind == CardKind.Slash) == 2 &&
            s.G.State.TurnNumber == 2);
        s.Need(s.E<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 1 && e.CardKind == CardKind.Slash) == 2,
            "The granted quota let the target play a second Slash in its very next play phase.");
        s.Cold(); s.Replay();
    }

    public static void SiLiArmedSlashIsUnrespondableAndBeishuiExpires()
    {
        var s = Scene.Start("sili-unrespondable", 4, [],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                    new ShanheStartingCard(CardKind.Slash, Suit.Club, 12),
                    new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)]);
        s.SiliActivationsLeft = 1;
        s.SiliBranch = "gain-slash";
        s.Play();
        s.UseHandCard(CardKind.Duel, 1);
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.SourceSeat == 0));
        s.Need(s.V(1).Hp == 7 && !s.E<ShiHcSiliInvokedEvent>().Any(),
            "The opening duel wounded the target while the owner's health stayed untouched.");
        s.End();
        s.Reach(() => s.E<ShiHcSiliSlashArmedEvent>() is [{ OwnerSeat: 0, CardId: > 0 }] &&
            s.V(0).Hand.Any(card => card.Id == s.E<ShiHcSiliSlashArmedEvent>()[0].CardId) &&
            s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var armed = s.E<ShiHcSiliSlashArmedEvent>()[0];
        var dodge = s.V(1).Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        s.Need(s.E<ShiHcSiliInvokedEvent>() is [{ OwnerSeat: 0, Branch: "slash" }] &&
                s.E<ShiHcSiliBeishuiArmedEvent>().Length == 1,
            "The Slash branch recorded its invocation and beishui cost exactly once.");
        s.Need(s.E<ShiHcSiliBeishuiConsumedEvent>().Length == 1 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.SkillId == Sili && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.V(0).Hp == 2,
            "Beishui expired once at the start of the owner's next turn as a single real HP loss.");
        s.UseHandCard(CardKind.Slash, 1, armed.CardId);
        s.Reach(() => s.V(1).Hp == 6);
        s.Need(s.V(1).Hp == 6 && s.Zone(dodge) == CardLocation.Hand(1),
            "The armed Slash drew blood while the target's Dodge stayed unreachable in hand.");
        s.Cold(); s.Replay();
    }

    public static void SiLiDrawGrantAddsOneCardToNextDrawPhase()
    {
        var s = Scene.Start("sili-draw-grant", 4, [],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)]);
        s.SiliActivationsLeft = 1;
        s.SiliBranch = "draw-plus";
        s.Play();
        s.UseHandCard(CardKind.Duel, 1);
        s.Reach(() => s.E<ShiHcSiliInvokedEvent>() is [{ OwnerSeat: 0, Branch: "draw" }] &&
            s.E<ShiHcSiliDrawGrantedEvent>() is [{ OwnerSeat: 0, GrantedInDrawPhase: false }]);
        s.Need(s.E<ShiHcSiliBeishuiArmedEvent>().Length == 1,
            "The draw branch recorded its invocation with its beishui cost.");
        s.End();
        s.Reach(() => s.E<ShiHcSiliDrawConsumedEvent>().Length == 1 &&
            s.E<ProgramNormalDrawAdjustedEvent>().Count(e => e.OwnerSeat == 0 && e.Adjustment == 1 && e.TotalAdjustment == 1) == 1);
        s.Need(s.E<ProgramNormalDrawAdjustedEvent>().Count(e => e.OwnerSeat == 0 && e.Adjustment == 1 && e.TotalAdjustment == 1) == 1 &&
            s.E<ShiHcSiliDrawConsumedEvent>().Length == 1,
            "The next draw phase consumed the grant and adjusted the normal draw by exactly one.");
        s.Need(s.V(0).HandCount == 1 && s.V(0).Hp == 2 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.SkillId == Sili && e.TargetSeat == 0 && e.Amount == 1) == 1,
            "The owner drew the single granted card after beishui cost one HP at the turn start.");
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
        public CardLocation Zone(int cardId) => G.CreateCardZoneDiagnostics().Single(d => d.CardId == cardId).Location;

        public bool CujunInvoke { get; set; }
        public int SiliActivationsLeft { get; set; }
        public string SiliBranch { get; set; } = "gain-slash";

        public void UseHandCard(CardKind kind, int targetSeat, int? exactCardId = null)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                V(0).Hand.Any(card => card.Kind == kind && (exactCardId is null || card.Id == exactCardId)) &&
                G.GetHumanLegalActions().Any(a => a.CardId == V(0).Hand
                    .Where(card => card.Kind == kind && (exactCardId is null || card.Id == exactCardId))
                    .Select(card => card.Id).First() && a.TargetSeats.Contains(targetSeat)));
            var cardId = V(0).Hand.Where(card => card.Kind == kind && (exactCardId is null || card.Id == exactCardId))
                .Select(card => card.Id).First();
            var a = G.GetHumanLegalActions().First(action => action.CardId == cardId && action.TargetSeats.Contains(targetSeat));
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
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Cujun or Sili &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("target-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == "target-1"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Cujun &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("gift-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("gift-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Sili &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") is "gain-slash" or "draw-plus"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == SiliBranch); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Cujun or Sili &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") is "activate" or "skip"))
                    {
                        var activate = p.SkillPrompt!.SkillId == Cujun ? CujunInvoke : SiliActivationsLeft-- > 0;
                        Answer(c => c.Parameters.GetValueOrDefault("program-action") == (activate ? "activate" : "skip"));
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.DiscardCards)
                    {
                        var slashIds = V(0).Hand.Where(card => card.Kind == CardKind.Slash).Select(card => card.Id)
                            .Intersect(p.ValidCardIds).Take(p.RequiredCardCount).ToArray();
                        var ids = slashIds.Length >= p.RequiredCardCount ? slashIds : p.ValidCardIds.Take(p.RequiredCardCount).ToArray();
                        Accept0(new DiscardCardsCommand(0, ids, p.PromptId, G.Revision));
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard)
                    { End(); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Huang Chong boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Huang Chong did not reach the expected bounded native boundary.");
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
        public string StackDump() => string.Join("\n", G.ResolutionStack.Select(f => f.GetType().Name + "#" + f.Id +
            (f is CardGame.Core.ProgramSkillFrame p ? " skill=" + p.SkillId + " trigger=" + (p.TriggerId ?? "-") + " receipt=" + (p.HuangChong?.Operation + "/" + p.HuangChong?.Stage ?? "null") : "") +
            (f is CardGame.Core.ProgramLifecycleTriggerWindowFrame w ? " window=" + w.Window + " cont=" + w.Continuation : "") +
            (f is CardGame.Core.HpChangedTriggerWindowFrame h ? " cont=" + h.Continuation + " resume=" + h.ResumeFrameId : "")));
        private void Accept0(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Huang Chong command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).MaxHp, V(i).HandCount }),
            Started = E<ShiHcCujunStartedEvent>(), Gifts = E<ShiHcCujunGiftMovedEvent>(), Grants = E<ShiHcCujunSlashGrantedEvent>(),
            Claims = E<ShiHcCujunClaimIssuedEvent>(), Settled = E<ShiHcCujunClaimSettledEvent>(),
            Invoked = E<ShiHcSiliInvokedEvent>(), Armed = E<ShiHcSiliSlashArmedEvent>(),
            Drawn = E<ShiHcSiliDrawGrantedEvent>(), Beishui = E<ShiHcSiliBeishuiArmedEvent>(),
            Recovery = E<RecoveryAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.SourceSeat, e.Amount }),
            Slashes = E<CardUseDeclaredEvent>().Where(e => e.CardKind == CardKind.Slash).Select(e => new { e.SourceSeat }),
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
        public PackageManifest Manifest { get; } = new("fixture-shi-huang-chong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Dry}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Dry] = new { name = Dry, description = "只跳过普通摸牌，保留真实出牌阶段。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Dry, "wei", 8));
            b.AddCard(new("fixture:shi-huang-chong-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-huang-chong-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-huang-chong-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 4 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-huang-chong-dodge", Suit.Heart, 6)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-huang-chong-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-huang-chong-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势黄崇实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
