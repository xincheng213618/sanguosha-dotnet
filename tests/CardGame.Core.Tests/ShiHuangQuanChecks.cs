using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiHuangQuanChecks
{
    private const string Hero = "mobile:shi-huang-quan", Cuixian = "mobile:shi-cuixian",
        Dry = "fixture:shi-huang-quan-dry", Keeper = "fixture:shi-huang-quan-keeper",
        Deck = "fixture:shi-huang-quan-deck", Peer = "fixture:shi-huang-quan-peer";
    private static readonly CardLocation PileOne = new(CardZoneKind.PrivatePersistentPile, 1, "mobile-shi-huang-quan.cuixian");

    public static void CuixianDamagedUsePlacesAndReclaimsHalf()
    {
        var s = Scene.Start("cuixian-reclaim", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 6)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 7)]);
        s.Play();
        s.PlayHandSlash(1);
        s.Reach(() => s.E<ShiQuanCuixianPlacedEvent>() is [{ OwnerSeat: 0, HolderSeat: 1, UseId: > 0 }]);
        var placed = s.E<ShiQuanCuixianPlacedEvent>()[0];
        s.Need(placed.CardIds.Count == 2 &&
                s.G.CardMovements.Count(m => placed.CardIds.Contains(m.CardId) &&
                    m.To == PileOne && m.Reason.Value == "skill-program.mobile:shi-cuixian.place") == 2,
            "The two hands each paid one face-up cuixian card onto the holder's general.");
        s.Need(s.G.CardMovements.Count(m => placed.CardIds[0] == m.CardId && m.From == CardLocation.Hand(0)) == 1 &&
                s.G.CardMovements.Count(m => placed.CardIds[1] == m.CardId && m.From == CardLocation.Hand(1)) == 1,
            "The placement took the owner's pick and one of the holder's hidden hand slots.");
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.SourceSeat == 0 &&
                e.SourceCardUseResolutionId == placed.UseId) && s.E<ShiQuanCuixianClaimedEvent>() is [{ OwnerSeat: 0 }]);
        var claimed = s.E<ShiQuanCuixianClaimedEvent>()[0];
        s.Need(claimed.CardIds.Count == 1 && claimed.CardIds[0] == placed.CardIds[0] &&
                s.G.CardMovements.Any(m => m.CardId == claimed.CardIds[0] &&
                    m.From == PileOne && m.To == CardLocation.Hand(0) &&
                    m.Reason.Value == "skill-program.mobile:shi-cuixian.claim"),
            "The damaged use reclaimed the first half of the holder's pile in placement order.");
        s.Need(s.V(1).HandCount == 0,
            "The holder's remaining cuixian card stays parked on their general.");
        s.Cold(); s.Replay();
    }

    public static void CuixianTurnStartReturnsDamageCardsAndDuelsTheHolder()
    {
        var s = Scene.Start("cuixian-duel", 4, [Keeper],
            [new Enemy(8, [Dry]) { StartingHand = [new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)] },
                new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 7)]);
        s.Play();
        s.PlayHandSlash(1);
        s.Reach(() => s.E<ShiQuanCuixianPlacedEvent>() is [{ HolderSeat: 1 }] &&
            s.E<ShiQuanCuixianClaimedEvent>().Length == 1);
        s.Need(s.G.CreateCardZoneDiagnostics().Single(d => d.CardId == s.E<ShiQuanCuixianPlacedEvent>()[0].CardIds[1]).Location == PileOne,
            "Only the holder's own cuixian card stayed parked before their turn.");
        s.End();
        s.Reach(() => s.E<ShiQuanCuixianChosenEvent>() is [{ OwnerSeat: 0, HolderSeat: 1, Branch: "give-damage", DuelUseId: > 0 }]);
        var chosen = s.E<ShiQuanCuixianChosenEvent>()[0];
        s.Need(s.G.CardMovements.Any(m => chosen.CardIds.Count == 1 && m.CardId == chosen.CardIds[0] &&
                m.From == PileOne && m.To == CardLocation.Hand(1) &&
                m.Reason.Value == "skill-program.mobile:shi-cuixian.give"),
            "The holder took back the damage cuixian card at their turn start.");
        s.Reach(() => s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.SourceSeat == 0 &&
                e.SourceCardUseResolutionId == chosen.DuelUseId));
        s.Need(s.E<DamageAppliedEvent>().Count(e => e.SourceCardUseResolutionId == chosen.DuelUseId) == 1 &&
                s.E<DamageAppliedEvent>().Single(e => e.SourceCardUseResolutionId == chosen.DuelUseId) is
                    { TargetSeat: 1, SourceSeat: 0, Amount: 1 },
            "The virtual Duel punished the holder for exactly one damage from the owner.");
        s.Cold(); s.Replay();
    }

    public static void GushiGrantsOncePerZoneAndClaimsADamageCard()
    {
        var s = Scene.Start("gushi-zone", 4, [Keeper],
            [new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry]), new Enemy(8, [Dry])],
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 5)]);
        s.Play();
        s.PlayHandSlash(1);
        s.Reach(() => s.E<ShiQuanGushiZoneUsedEvent>() is [{ OwnerSeat: 0, Zone: CardZoneKind.Hand }] &&
            s.E<ShiQuanGushiCardClaimedEvent>() is [{ OwnerSeat: 0, FromZone: CardZoneKind.DrawPile }]);
        var claimed = s.E<ShiQuanGushiCardClaimedEvent>()[0];
        s.Need(s.G.CardMovements.Any(m => m.CardId == claimed.CardId && m.To == CardLocation.Hand(0) &&
                m.Reason.Value == "skill-program.mobile:shi-gushi.gain"),
            "The empty hand zone granted one deck damage card with its skill reason.");
        s.Need(s.V(0).HandCount == 1,
            "The claimed damage card rests in the owner's hand for an unlimited later use.");
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.Need(s.E<ShiQuanGushiZoneUsedEvent>().Count(e => e.Zone == CardZoneKind.Hand) == 1 &&
                s.E<ShiQuanGushiZoneUsedEvent>().Count(e => e.OwnerSeat == 0) == 1,
            "The per-game hand-zone lock stopped every later emptying from re-arming GuShi.");
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
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is Cuixian or "mobile:shi-gushi" &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") is "activate" or "skip"))
                    { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Cuixian &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("own-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("own-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Cuixian &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("theirs-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("theirs-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == Cuixian &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") is "take-nondamage" or "give-damage"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") is "take-nondamage" or "give-damage"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == "mobile:shi-gushi" &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") is "gain" or "replace"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == "gain"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == "mobile:shi-gushi" &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("holder-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("holder-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == "mobile:shi-gushi" &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "confirm"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == "confirm"); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == "mobile:shi-gushi" &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("pick-", StringComparison.Ordinal) == true))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch")?.StartsWith("pick-", StringComparison.Ordinal) == true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    { Answer(c => c.Cards.Count == 0); continue; }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Huang Quan boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Huang Quan did not reach the expected bounded native boundary.");
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Huang Quan command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).MaxHp, V(i).HandCount }),
            Placed = E<ShiQuanCuixianPlacedEvent>(), Claimed = E<ShiQuanCuixianClaimedEvent>(), Chosen = E<ShiQuanCuixianChosenEvent>(),
            Zones = E<ShiQuanGushiZoneUsedEvent>(), Gains = E<ShiQuanGushiCardClaimedEvent>(),
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
        public PackageManifest Manifest { get; } = new("fixture-shi-huang-quan", new(1, 0, 0));
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
            b.AddCard(new("fixture:shi-huang-quan-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-huang-quan-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddCard(new("fixture:shi-huang-quan-dodge", "闪", "基本牌", "同体闪实体", CardKind.Dodge));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 4 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-huang-quan-dodge", Suit.Heart, 6)
                    : i % 3 == 0
                        ? new ContentDeckPhysicalCard("fixture:shi-huang-quan-duel", Suit.Spade, 6)
                        : new ContentDeckPhysicalCard("fixture:shi-huang-quan-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势黄权实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
