using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileCaoYingChecks
{
    private const string Hero = "mobile:cao-ying", Ling = "mobile:lingren", Fu = "mobile:fujian";
    private const string Owner = "fixture:mobile-cao-ying-owner", Peer = "fixture:mobile-cao-ying-peer-";
    private const string Driver = "fixture:mobile-cao-ying-driver", Extra = "fixture:mobile-cao-ying-extra";
    private const string DrawWatch = "fixture:mobile-cao-ying-draw-watch";
    private const string Prevent = "fixture:mobile-cao-ying-prevent", Mode = "identity:classic-mobile-cao-ying";

    public static void PhaseQuotaOriginalCardOnlyNoRefundAndNativeTemporarySkills()
    {
        var s = Scene.Start(); s.Play(); var turn = s.G.State.TurnNumber; var targetHp = s.V(1).Hp;
        s.Slash(1); s.Guess(1);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == DrawWatch);
        s.Need(s.Root?.CaoYing is { Stage: CaoYingStage.DrawChildren, Correct: 3, DrawCount: 2 } &&
            !s.Has("classic:jianxiong") && !s.Has("classic:xingshang") && !s.E<ProgramTurnSkillsGrantedEvent>().Any(e => e.SkillId == Ling),
            "Mobile draws its two actual cards first: while a paid gain child remains active, neither temporary skill has been granted.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        var first = s.E<CaoYingGuessResolvedEvent>().Single();
        s.Need(first.CorrectCount == 3 && first.Policy == CaoYingRulePolicy.MobileOrdinary && first.PhaseInstanceId is > 0 &&
            s.V(1).Hp == targetHp && !s.E<DamageAppliedEvent>().Any(),
            "The first genuine Slash guesses all three categories correctly, but its original damage is natively prevented.");
        s.Need(s.E<ProgramTurnSkillsGrantedEvent>().Single(e => e.SkillId == Ling) is { NextActualTurnStartExpiry: { } expiry } grant &&
            grant.GrantedSkillIds.SequenceEqual(["classic:jianxiong", "classic:xingshang"]) && expiry.GrantedTurnNumber == turn && expiry.RecipientSeat == 0 &&
            s.Has("classic:jianxiong") && s.Has("classic:xingshang") && !s.Has("ol:jianxiong"),
            "Mobile's exact ordinary Jianxiong and Xingshang are granted through the native next-start skill-source receipt.");

        s.Slash(1); s.Play();
        var afterSecond = s.E<DamageAppliedEvent>().Single();
        s.Need(s.E<CaoYingGuessResolvedEvent>().Length == 1 && afterSecond.Amount == 1 && afterSecond.SourceCardUseResolutionId != first.UseId &&
            s.V(1).Hp == targetHp - 1 && s.E<CardUseDebitRecordedEvent>().Length == 2 && s.E<CardUseDebitRefundedEvent>().Length == 0,
            "The same actual phase cannot guess again, a later Slash cannot inherit the prevented original card's bonus, and neither use is refunded.");

        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == turn && s.Has("classic:jianxiong") && s.Has("classic:xingshang"),
            "The real inserted Play returns to normal Play within the same turn without expiring the temporary skills.");
        s.Slash(1); s.Guess(1); s.Play();
        var guesses = s.E<CaoYingGuessResolvedEvent>(); var last = guesses.Last();
        s.Need(guesses.Length == 2 && last.PhaseInstanceId != first.PhaseInstanceId && last.TurnNumber == first.TurnNumber &&
            s.E<DamageAppliedEvent>().Any(e => e.SourceCardUseResolutionId == last.UseId && e.TargetSeat == 1 && e.Amount == 2) &&
            s.V(1).Hp == targetHp - 3 && s.E<CardUseDebitRecordedEvent>().Length == 3 && s.E<CardUseDebitRefundedEvent>().Length == 0,
            "A distinct actual Play grants a new guessing opportunity and its own original-card bonus, while retaining every native Slash debit.");
        var grants = s.E<ProgramTurnSkillsGrantedEvent>().Where(e => e.SkillId == Ling).ToArray();
        s.Need(grants.Length == 2 && grants.Select(e => e.GrantSourceId).Distinct().Count() == 2,
            "Each all-correct activation owns an independent temporary source instead of overwriting the earlier issued skills.");
        s.End(); s.Reach(() => s.G.State.TurnNumber > turn && s.G.State.CurrentSeat == 0 && s.P is { Kind: DecisionKind.PlayCard });
        s.Need(!s.Has("classic:jianxiong") && !s.Has("classic:xingshang") && !s.Has("ol:jianxiong") &&
            s.E<ProgramNextActualTurnStartSkillGrantExpiredEvent>().Count(e => e.RecipientSeat == 0 &&
                grants.Any(g => g.GrantSourceId == e.Grant.SourceId)) == 4,
            "Both issued skill sources survive ending and expire exactly at the owner's next genuine TurnStarted boundary.");
    }

    public static void EndingMinimumRandomSubsetPrivateCold()
    {
        // One fixed-seed probe discovers the next native random target, without searching seeds.
        // Giving cards consumes no randomness, so the real run makes this exact target uniquely largest.
        var probe = Scene.Start(inspection: true); probe.Play(); probe.End(); probe.Reach(() => probe.Stage(CaoYingStage.Inspection));
        var selected = probe.Root!.CaoYing!.Target!.Value;
        var s = Scene.Start(inspection: true); s.Play(); var turn = s.G.State.TurnNumber;
        s.Need(s.E<CaoYingInspectionIssuedEvent>().Length == 0, "Ordinary mobile Fujian does not inspect in preparation.");
        var gift = s.V(0).Hand.Take(2).Select(c => c.Id).ToArray();
        s.Submit(new UseProgramSkillCommand(0, Driver, "give-two", gift, [selected], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.V(0).HandCount == 2 && s.V(selected).HandCount == 6 && Enumerable.Range(1, 3).All(i => s.V(i).HandCount > 2),
            "A real native gift makes the owner the two-card minimum and the future random inspection target uniquely largest.");
        s.End(); s.Reach(() => s.Stage(CaoYingStage.Inspection));
        var frame = s.Root!; var receipt = frame.CaoYing!; var cards = receipt.Cards.Select(c => c.Id).ToArray();
        s.Need(frame.SkillId == Fu && receipt.Target == selected && receipt.InspectionMinimum == 2 && cards.Length == 2 && cards.Distinct().Count() == 2 &&
            cards.All(id => s.V(selected).Hand.Any(c => c.Id == id)) && s.V(selected).HandCount > cards.Length &&
            s.E<CaoYingInspectionIssuedEvent>() is [{ CardCount: 2 }],
            "Ending Fujian includes the uniquely largest other character and samples exactly the true minimum number of actual hand entities.");
        s.Need(s.G.CreateSnapshot(0).PrivateRevealedCards?.Select(c => c.Id).SequenceEqual(cards) == true &&
            Enumerable.Range(1, 3).All(v => s.G.CreateSnapshot(v).PrivateRevealedCards is null && s.G.CreateSnapshot(v).PendingDecision is null),
            "Only the owner receives the frozen private two-card sample; no observer sees the private inspection window.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "done");
        s.Reach(() => s.G.State.TurnNumber > turn && s.G.State.CurrentSeat == 0 && s.P is { Kind: DecisionKind.PlayCard });
        s.Need(s.E<CaoYingInspectionIssuedEvent>().Length == 1 && s.G.CreateSnapshot(0).PrivateRevealedCards is null,
            "Cold completes the same original ending inspection once, removes its private view and never adds a preparation inspection.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.CaoYing is not null);
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Has(string id) => V(0).Skills?.Any(s => s.Id == id) == true;
        public bool Stage(CaoYingStage stage) => Root?.CaoYing?.Stage == stage && P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "cao-ying") == true;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Submit(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Slash(int target)
        {
            var action = G.GetHumanLegalActions().FirstOrDefault(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([target]));
            Need(action?.CardId is not null, "Missing genuine physical Slash.");
            Submit(new PlayCardCommand(0, action!.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind));
        }
        public void Guess(int target)
        {
            Reach(() => P?.SkillPrompt?.SkillId == Ling && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => Stage(CaoYingStage.Target)); Answer(c => c.Targets.SequenceEqual([target]));
            Reach(() => Stage(CaoYingStage.Guess)); Answer(c => c.Parameters.GetValueOrDefault("branch") == "guess-1");
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "Missing actual native Cao Ying choice.");
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Mobile Cao Ying command rejected.");
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded mobile Cao Ying fixture unexpectedly ended.");
                var p = P;
                if (p is { Kind: DecisionKind.SelectGeneral }) Submit(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer + p.PlayerSeat, G.Revision, p.PromptId));
                else if (Stage(CaoYingStage.Inspection)) Answer(c => c.Parameters.GetValueOrDefault("branch") == "done");
                else if (p is { Kind: DecisionKind.DiscardCards }) Submit(new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision));
                else if (p is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 }) Submit(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId));
                else if (p is not null && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue")) Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                else if (p is not null && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded actual mobile Cao Ying scene missed its expected boundary.");
        }
        private string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold retains the exact original inspection frame, random target and sample, every private view and accepted command.");
        }
        public void Need(bool ok, string reason)
        {
            if (!ok) throw new InvalidOperationException(reason + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
                Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null,
                Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }),
                Frames = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name, Receipt = (f as ProgramSkillFrame)?.CaoYing }),
                Guesses = E<CaoYingGuessResolvedEvent>(), Grants = E<ProgramTurnSkillsGrantedEvent>(), Inspections = E<CaoYingInspectionIssuedEvent>()
            }));
        }
        public static Scene Start(bool inspection = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(inspection));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 12
            }, registry);
            var scene = new Scene(registry, game); scene.Submit(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool inspection) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-cao-ying", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "classic:cao-cao", "classic:cao-pi");
            var definitions = new List<JsonNode>
            {
                JsonNode.Parse("""
                {"id":"fixture:mobile-cao-ying-driver","revision":1,"modifiers":[{"id":"real-slash-quota","query":"slashLimit","operation":"add","value":4,"priority":0},{"id":"keep-native-hand","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[{"id":"give-two","minCards":2,"maxCards":2,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"giveSelected","target":"selectedTarget","amount":2}]}]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:mobile-cao-ying-extra","revision":1,"triggers":[{"id":"first-actual-extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:mobile-cao-ying-prevent","revision":1,"triggers":[{"id":"prevent-first-real-damage","window":"beforeDamageApplied","subject":"damageTarget","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"preventCurrentDamage","target":"owner"}]}]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:mobile-cao-ying-draw-watch","revision":1,"triggers":[{"id":"native-draw-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.cao-ying.draw"],"movementOccurrence":"perBatch","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-paid","options":[{"id":"continue"}]}]}]}
                """)!
            };
            var labels = definitions.ToDictionary(n => n["id"]!.GetValue<string>(), n => (object)(n["id"]!.GetValue<string>() == DrawWatch
                ? new Dictionary<string, object> { ["name"] = "真实摸牌子窗", ["description"] = "已摸两张、尚未授技", ["optionLabels"] = new Dictionary<string, string> { ["continue"] = "继续" } }
                : new Dictionary<string, object> { ["name"] = "真实原生边界", ["description"] = "实际额外阶段、交牌、防伤" }));
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "普通移动曹婴原生边界", "supporter", inspection ? Fu : Ling, "wei", 4,
                inspection ? [Driver] : [Fu, Driver, Extra, DrawWatch], GeneralGender.Female));
            for (var i = 1; i < 4; i++) builder.AddGeneral(new(Peer + i, "固定其他角色", "supporter", "standard:none", "wei", 12,
                !inspection && i == 1 ? [Prevent] : []));
            const string deck = "fixture:mobile-cao-ying-deck";
            builder.AddDeck(new(deck, "固定真实基本牌", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(Mode, "移动曹婴真实技能与私看", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer + 1, Peer + 2, Peer + 3]));
        }
    }
}
