using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceWarElephantNativeChecks
{
    private const string Driver = "fixture:war-elephant-driver", Quiet = "fixture:war-elephant-quiet";
    private const string Owner = "fixture:war-elephant-owner", Peer = "fixture:war-elephant-peer", Elephant = "fixture:war-elephant-enemy";
    private const string Deck = "fixture:war-elephant-deck";
    private const string Shouyi = StandardShanheSourceWarElephantPackage.ShouyiSkillId;
    private const string Xiongshou = StandardShanheSourceWarElephantPackage.XiongshouSkillId;

    public static void RealTrickRangeAllSlashDamageAndTurnoverProtection()
    {
        var ordinary = Scene.Start(ownerSkills: false);
        Need(!ordinary.G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Snatch && a.TargetSeats.SequenceEqual([3])),
            "The fixed six-seat ordinary setup cannot Snatch the opposite seat at distance three.");
        var s = Scene.Start(ownerSkills: true);
        var steal = s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Snatch && a.TargetSeats.SequenceEqual([3]));
        var before = s.V(3).HandCount;
        s.Accept(new PlayCardCommand(0, steal.CardId!.Value, steal.TargetSeats, s.G.Revision, s.P!.PromptId, steal.PlayedCardKind, steal.TargetCardId));
        s.Play();
        var stolen = s.G.CardMovements.Where(m => m.From == CardLocation.Hand(3) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Snatch).ToArray();
        Need(s.V(3).HandCount == before - 1 && stolen.Length == 1 &&
            s.G.CardMovements.Count(m => m.CardId == stolen[0].CardId && m.From == CardLocation.Processing &&
                m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.SnatchFinished) == 1 &&
            s.V(0).Hand.Any(card => card.Id == stolen[0].CardId),
            "Actual source673 also permits and completes a distant non-Slash trick after source1136 leaves that distance above one.");
        foreach (var kind in new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash })
        {
            var action = s.SlashAction(kind);
            Need(s.V(1).Hp > s.V(0).Hp && s.G.PredictPublicActionDamage(0, action, 1).DamageAmount == 2,
                "Source1136 adds one to the complete Slash family even against a healthier target, with accurate public prediction.");
            if (kind == CardKind.ColdSlash) continue;
            var hp = s.V(1).Hp;
            s.Accept(new PlayCardCommand(0, action.CardId!.Value, [1], s.G.Revision, s.P!.PromptId, action.PlayedCardKind));
            s.Play();
            Need(s.V(1).Hp == hp - 2 && s.E<DamageAppliedEvent>().Last() is { SourceSeat: 0, TargetSeat: 1, Amount: 2 },
                "The actual paid physical normal or elemental Slash has the same unconditional extra damage.");
        }
        s.Use("flip"); s.Play();
        Need(!s.V(0).IsFaceDown, "Source1136 prevents an actual program turnover without changing the original source skill.");
        ordinary.Use("flip"); ordinary.Play();
        Need(ordinary.V(0).IsFaceDown, "The same real turnover still succeeds without source1136.");
    }

    public static void FirstLargeDamagePerActualTurnCounterSlashReturnsOriginalAttack()
    {
        var s = Scene.Start(ownerSkills: false, enemyElephant: true);
        var turn = s.G.State.TurnNumber;
        s.Use("hurt-one", [1]); s.Play();
        Need(Counters(s) == 0 && s.V(1).Hp == 29, "An actual one-point hit does not spend the first greater-than-one damage trigger.");
        var original = s.PhysicalSlash();
        s.Until(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
        var counter = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.ShanheWarElephantCounterSlash is not null);
        var receipt = counter.ShanheWarElephantCounterSlash!;
        Need(counter.OwnerSeat == 1 && counter.WindowContext is { SourceSeat: 0, TargetSeat: 1, Amount: 2 } &&
            receipt.SourceSeat == 0 && receipt.SuspendedAttackId == original &&
            s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == original) &&
            s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == receipt.ChildUseId && f.SourceSeat == 1),
            "The counter is a real native Slash child on the original damaged owner's frame while the paid parent attack stays suspended.");
        s.Play();
        Need(Counters(s) == 1 && s.V(0).Hp == 18 && s.V(1).Hp == 27 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == original) == 1 && s.G.ResolutionStack.Count == 0,
            "The first actual two-point hit counters for two, then returns and finishes its original paid physical Slash once.");
        var second = s.PhysicalSlash(); s.Play();
        Need(Counters(s) == 1 && s.V(1).Hp == 25 && s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == second) == 1,
            "The next actual two-point hit in the same turn still resolves normally without another counter.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.State.TurnNumber > turn && s.G.ResolutionStack.Count == 0);
        var next = s.PhysicalSlash(); s.Play();
        Need(Counters(s) == 2 && s.V(0).Hp == 16 && s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == next) == 1,
            "Advancing through real intervening turns refreshes the counter and again preserves the original attack's completion.");
    }

    private static int Counters(Scene s) => s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Xiongshou);
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 6).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected elephant native command."); }
        public void Use(string activation, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public LegalAction SlashAction(CardKind kind) => G.GetHumanLegalActions().First(a => a.CardId is { } id && V(0).Hand.Any(c => c.Id == id && c.Kind == kind) && a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1]));
        public long PhysicalSlash()
        {
            var a = SlashAction(CardKind.Slash);
            Accept(new PlayCardCommand(0, a.CardId!.Value, [1], G.Revision, P!.PromptId, a.PlayedCardKind));
            return E<CardUseDeclaredEvent>().Last(e => e.CardId == a.CardId).ResolutionId;
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The short elephant check must stay inside a live battle.");
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" ||
                        c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        (prompt.Kind == DecisionKind.SelectTargetCard ? prompt.Choices.FirstOrDefault() : null);
                    Need(choice is not null, "Unexpected elephant prompt. " + Diagnostic());
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native elephant flow did not reach its real action boundary. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
            Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null,
            Players = new[] { V(0), V(1), V(3) }.Select(p => new { p.Seat, p.GeneralId, p.Hp, p.HandCount }),
            Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Recent = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool ownerSkills, bool enemyElephant = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(enemyElephant));
            var setup = new ShanheBattleSetup(81, 1, "source-elephant-native", Owner, 20, 0, 0, 0, 0, [],
                // Retain a real Dodge so the nested counter publishes its response
                // boundary; choose take-damage there instead of auto-finishing it.
                [new(CardKind.Snatch, Suit.Spade, 3), new(CardKind.FireSlash, Suit.Heart, 4), new(CardKind.ThunderSlash, Suit.Spade, 5), new(CardKind.ColdSlash, Suit.Club, 6), new(CardKind.Dodge, Suit.Heart, 7)],
                [], Enumerable.Range(1, 5).Select(seat => new ShanheCombatantDefinition(seat == 1 && enemyElephant ? Elephant : Peer)).ToArray(), [], new string('e', 64))
                { TacticSkillIds = ownerSkills ? [Shouyi, Xiongshou] : [] };
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 6, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 14 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(bool enemyElephant) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-war-elephant", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            string Read(string part)
            {
                using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-source-war-elephant-2453." + part + ".json")!;
                using var reader = new StreamReader(stream); return reader.ReadToEnd();
            }
            var source = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
            foreach (var (id, program) in source.Programs) b.AddSkill(new(id, source.Presentations[id].Name, source.Presentations[id].Description)
                { Program = program, ProgramPresentation = source.Presentations[id], Tags = SkillTag.Locked });
            var extraDamage = enemyElephant ? ",\"damageModifiers\":[{\"id\":\"incoming-two\",\"cardKinds\":[\"slash\"],\"amount\":1,\"condition\":\"always\"}]" : "";
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,
                    "modifiers":[{"id":"repeated-real-slash","query":"slashLimit","operation":"unlimited","value":0,"priority":0}],
                    "activations":[
                      {"id":"hurt-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                      {"id":"flip","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"turnOver","target":"owner"}]}]{{{extraDamage}}}},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"wait","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "原生战象测试操作", description = "真实用牌与小额伤害" }, [Quiet] = new { name = "静守", description = "跳过出牌" } } });
            var fixture = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in fixture.Programs) b.AddSkill(new(id, fixture.Presentations[id].Name, fixture.Presentations[id].Description)
                { Program = program, ProgramPresentation = fixture.Presentations[id] });
            var cold = CardCatalog.Get(CardKind.ColdSlash);
            b.AddCard(new("fixture:elephant-cold", cold.DisplayName, cold.CategoryName, cold.Description, CardKind.ColdSlash));
            b.AddGeneral(new(Owner, "原生操作", "supporter", Driver, "shu", 20));
            b.AddGeneral(new(Peer, "静守目标", "supporter", Quiet, "wei", 30));
            b.AddGeneral(new(Elephant, "原生凶兽", "supporter", Quiet, "wei", 30, [Shouyi, Xiongshou]));
            b.AddDeck(new(Deck, "固定无闪实体牌堆", 4, 2, [new("standard:slash", 128)]));
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "战象原生六人阵", 6, 6, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer, Elephant], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 5 }));
        }
    }
}
