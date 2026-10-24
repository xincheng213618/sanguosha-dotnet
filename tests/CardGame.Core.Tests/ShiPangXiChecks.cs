using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiPangXiChecks
{
    private const string Hero = "mobile:shi-pang-xi", XuYe = "mobile:shi-xuye", KuangXiang = "mobile:shi-kuangxiang",
        Quiet = "fixture:shi-pang-xi-quiet", Keeper = "fixture:shi-pang-xi-keeper",
        Deck = "fixture:shi-pang-xi-deck", Peer = "fixture:shi-pang-xi-peer";

    public static void XuYeArmsGlobalMinimumVictimsAndDrawsTwo()
    {
        var s = Scene.Start("xuye", 3, [Keeper],
            [new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.ArmXuYePrompts = true; s.Play();
        s.PlayHandSlash(1);
        s.Reach(() => s.P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
            p.SkillPrompt?.SkillId == XuYe && p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "arm"));
        s.AnswerXuye("arm");
        s.Need(s.E<ShiPxXuyeDrawEvent>() is [{ OwnerSeat: 0, VictimSeat: 1, Drawn: 2 }] && s.V(1).HandCount == 2,
            "The armed victim draws exactly two cards.");
        s.Need(!s.E<ShiPxXuyeTopPlacedEvent>().Any(),
            "The victim is not the global maximum, so nothing is topped.");
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.G.State.CurrentSeat == 0 &&
            s.P is { Kind: DecisionKind.PlayCard });
        s.PlayHandSlash(4);
        s.Reach(() => s.E<ShiPxXuyeDrawEvent>().Length == 2);
        s.Need(s.E<ShiPxXuyeDrawEvent>() is [{ VictimSeat: 1 }, { VictimSeat: 4, Drawn: 2 }] && s.V(4).HandCount == 2,
            "The once-per-turn limit re-arms on the next turn against another minimum-hand victim.");
        s.Cold(); s.Replay();
    }

    public static void KuangxiangSwapPersistsUntilNextPlayStart()
    {
        var s = Scene.Start("kuangxiang", 3, [Keeper],
            [new Enemy(8, [Quiet]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 4),
                    new ShanheStartingCard(CardKind.Slash, Suit.Club, 5),
                    new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6),
                    new ShanheStartingCard(CardKind.Slash, Suit.Club, 7)] },
                new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 4); s.Play();
        s.ActivateKuangxiang(1);
        s.Need(s.E<ShiPxKuangxiangSwappedEvent>() is [{ OwnerSeat: 0, TargetSeat: 1 } swap] &&
            swap.OwnerTookIds.Count == 4 && swap.TargetTookIds.Count == 4 &&
            s.V(0).HandCount == 4 && s.V(1).HandCount == 4,
            "The equal-hand swap exchanges all four cards both ways.");
        s.Need(s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:shi-kuangxiang.swap-give") == 4 &&
            s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:shi-kuangxiang.swap-take") == 4,
            "The swap moved four cards in each direction with its own reasons.");
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.End();
        s.Reach(() => s.G.State.TurnNumber >= 6 && s.G.State.CurrentSeat == 0 &&
            s.P is { Kind: DecisionKind.PlayCard });
        s.Need(s.E<ShiPxKuangxiangClearedEvent>() is [{ OwnerSeat: 0 }] && s.E<ShiPxKuangxiangRiderEvent>().Length == 0,
            "The owner's next play phase start sweeps the untouched swap before any rider.");
        s.Cold(); s.Replay();
    }

    public static void KuangxiangRiderOpensSelfXuyeAfterSpendingEverySwap()
    {
        var s = Scene.Start("kuangxiang", 3, [Keeper],
            [new Enemy(8, [Quiet]) { StartingHand = [new ShanheStartingCard(CardKind.Slash, Suit.Club, 4),
                    new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)] },
                new Enemy(8, [Quiet]), new Enemy(8, [Quiet]), new Enemy(8, [Quiet])],
            openingCards: 2,
            openingOverride: [new ShanheStartingCard(CardKind.Slash, Suit.Club, 4),
                new ShanheStartingCard(CardKind.Slash, Suit.Club, 5)]); s.ArmRiderPrompts = true; s.Play();
        s.ActivateKuangxiang(1);
        s.Need(s.E<ShiPxKuangxiangSwappedEvent>() is [{ OwnerSeat: 0, TargetSeat: 1 } swap] &&
            swap.OwnerTookIds.Count == 2 && swap.TargetTookIds.Count == 2 &&
            s.V(0).HandCount == 2 && s.V(1).HandCount == 2,
            "The two-card swap exchanges both hands.");
        var swappedIn = s.E<ShiPxKuangxiangSwappedEvent>()[0].OwnerTookIds;
        s.PlayHandSlash(1, swappedIn);
        s.Need(s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 1 && e.Amount == 1),
            "The swapped-in Slash strikes the unguarding partner.");
        s.PlayDuelAndFight(1, swappedIn);
        s.Reach(() => s.E<ShiPxKuangxiangRiderEvent>().Length > 0 && s.V(0).HandCount == 2);
        s.Need(s.E<ShiPxKuangxiangRiderEvent>() is [{ OwnerSeat: 0, Side: "self", SelfDrawn: 2 }] &&
            s.V(0).HandCount == 2,
            "Spending both swapped cards opens the self-targeted XuYe and draws two.");
        s.Reach(() => s.P is { PlayerSeat: 0 } bonus && bonus.Kind == DecisionKind.ProgramTrigger &&
            bonus.SkillPrompt?.SkillId == KuangXiang && bonus.Choices.Count > 0 &&
            bonus.Choices.All(c => c.Cards.Count == 1));
        s.AnswerTopPick();
        s.Need(s.E<ShiPxXuyeTopPlacedEvent>() is [{ VictimSeat: 0 }] &&
            s.G.CardMovements.Any(m => m.To == CardLocation.DrawPile && m.From.Zone == CardZoneKind.Hand &&
                m.From.OwnerSeat == 0),
            "The picked hand card moved onto the draw pile as the strict global maximum.");
        s.Need(s.E<ShiPxKuangxiangClearedEvent>().Length == 0,
            "The rider consumed the swap, so the sweep has nothing left to clear.");
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

        public bool ArmXuYePrompts { get; set; }
        public bool ArmRiderPrompts { get; set; }
        public bool FightDuelsWithSlashes { get; set; }

        public void ActivateKuangxiang(int targetSeat)
        {
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Offered(KuangXiang));
            Accept0(new UseProgramSkillCommand(0, KuangXiang, "kuangxiang-swap", [], [targetSeat], G.Revision, P!.PromptId));
        }

        public void AnswerXuye(string branch)
        {
            if (P is not { PlayerSeat: 0 } p || p.Kind != DecisionKind.ProgramTrigger ||
                p.SkillPrompt?.SkillId != XuYe) return;
            var choice = p.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == branch);
            Accept0(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
        }

        public void AnswerTopPick()
        {
            Reach(() => P is { PlayerSeat: 0 } p && p.Kind == DecisionKind.ProgramTrigger &&
                p.SkillPrompt?.SkillId == KuangXiang && p.Choices.Count > 0 && p.Choices.All(c => c.Cards.Count == 1));
            var choice = P!.Choices[0];
            Accept0(new AnswerPromptCommand(P!.PlayerSeat, P!.PromptId, choice.Id, G.Revision));
        }

        public void PlayHandSlash(int targetSeat, IReadOnlyList<int>? amongIds = null)
        {
            var slashId = V(0).Hand.Where(card => card.Kind == CardKind.Slash &&
                    (amongIds is null || amongIds.Contains(card.Id)))
                .Select(card => card.Id).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat)));
            var a = G.GetHumanLegalActions().First(a => a.CardId == slashId && a.TargetSeats.Contains(targetSeat));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }

        public void PlayDuelAndFight(int targetSeat, IReadOnlyList<int> swappedIn)
        {
            var duelId = V(0).Hand.Where(card => card.Kind == CardKind.Duel && swappedIn.Contains(card.Id))
                .Select(card => card.Id).First();
            Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                G.GetHumanLegalActions().Any(a => a.CardId == duelId && a.TargetSeats.Contains(targetSeat)));
            var a = G.GetHumanLegalActions().First(a => a.CardId == duelId && a.TargetSeats.Contains(targetSeat));
            Accept0(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
            Reach(() => P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == KuangXiang ||
                E<ShiPxKuangxiangRiderEvent>().Length > 0);
        }

        public void End() => Accept0(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 2200; i++)
            {
                if (ready()) return;
                if (P is { } p)
                {
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (p.PlayerSeat == 0 && p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == XuYe &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pass"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == (ArmXuYePrompts ? "arm" : "pass")); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId == KuangXiang &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pass"))
                    { Answer(c => c.Parameters.GetValueOrDefault("branch") == (ArmRiderPrompts ? "arm" : "pass")); continue; }
                    if (p.PlayerSeat == 0 && p.SkillPrompt?.SkillId is XuYe or KuangXiang)
                        throw new InvalidOperationException("Unexpected Pang Xi boundary. " + Dump());
                    if (p.PlayerSeat == 0 && p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying)
                    {
                        if (FightDuelsWithSlashes && p.Kind == DecisionKind.RespondSlash &&
                            p.Choices.Any(c => c.Cards.Count > 0))
                            Answer(c => c.Cards.Count > 0);
                        else
                            Answer(c => c.Cards.Count == 0);
                        continue;
                    }
                    if (p.PlayerSeat == 0 && p.Kind != DecisionKind.PlayCard && p.SkillPrompt?.SkillId is not (XuYe or KuangXiang) &&
                        p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "end-play"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "end-play"); continue; }
                    if (p.PlayerSeat == 0) throw new InvalidOperationException("Unexpected Pang Xi boundary. " + Dump());
                }
                Need(G.State.Winner == Winner.None, "The bounded real scene must retain its participants.");
                Accept0(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Pang Xi did not reach the expected bounded native boundary.");
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
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Pang Xi command."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        private string Dump() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Winner, P,
            Players = Enumerable.Range(0, 5).Select(i => new { i, V(i).Hp, V(i).HandCount }),
            Draw = E<ShiPxXuyeDrawEvent>(), Top = E<ShiPxXuyeTopPlacedEvent>(), Swap = E<ShiPxKuangxiangSwappedEvent>(),
            Rider = E<ShiPxKuangxiangRiderEvent>(), Cleared = E<ShiPxKuangxiangClearedEvent>(),
            Damage = E<DamageAppliedEvent>().Select(e => new { e.TargetSeat, e.Amount }),
            Moves = G.CardMovements.TakeLast(40).Select(m => new { m.CardId, From = m.From.Zone, FromSeat = m.From.OwnerSeat, To = m.To.Zone, ToSeat = m.To.OwnerSeat, Reason = m.Reason.Value }),
            Last = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });

        public static Scene Start(string nodeId, int heroHp, string[] heroExtras, Enemy[] enemies, int openingCards = 4,
            ShanheStartingCard[]? openingOverride = null)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] opening = openingOverride ?? Enumerable.Range(0, openingCards)
                .Select(i => i == openingCards - 1
                    ? new ShanheStartingCard(CardKind.Duel, Suit.Spade, 6)
                    : new ShanheStartingCard(CardKind.Slash, Suit.Club, 4 + i % 9)).ToArray();
            ShanheCombatantDefinition Combatant(int seat, Enemy enemy) => new(Peer, Hp: enemy.Hp, ExtraSkillIds: enemy.Extras)
            { StartingHand = enemy.StartingHand };
            var setup = new ShanheBattleSetup(96, 1, nodeId, Hero, heroHp, heroHp - 3, 0, 0, 0, heroExtras, opening, [],
                enemies.Select((enemy, index) => Combatant(index + 1, enemy)).ToArray(), [], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry);
            var scene = new Scene(game, registry); scene.Accept0(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-pang-xi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                {"id":"no-play","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Keeper}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":10,"priority":0}]}
            ]}
            """;
            var presentations = new Dictionary<string, object>
            {
                [Quiet] = new { name = Quiet, description = "安静真实回合并跳过普通摸牌出牌。" },
                [Keeper] = new { name = Keeper, description = "保持手牌。" },
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静真实同桌", "", Quiet, "wei", 8));
            b.AddCard(new("fixture:shi-pang-xi-slash", "杀", "基本牌", "同体杀实体", CardKind.Slash));
            b.AddCard(new("fixture:shi-pang-xi-duel", "决斗", "锦囊牌", "同体决斗实体", CardKind.Duel));
            b.AddDeck(new(Deck, "固定真实混合牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 150)
                .Select(i => i % 3 == 0
                    ? new ContentDeckPhysicalCard("fixture:shi-pang-xi-duel", Suit.Spade, 6)
                    : new ContentDeckPhysicalCard("fixture:shi-pang-xi-slash", Suit.Club, 5)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "势庞羲实际检查", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
