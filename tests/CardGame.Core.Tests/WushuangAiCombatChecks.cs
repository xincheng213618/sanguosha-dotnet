using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WushuangAiCombatChecks
{
    private const string Human = "fixture:wushuang-ai-human", Peer = "fixture:wushuang-ai-peer",
        Driver = "fixture:wushuang-ai-driver", Quiet = "fixture:wushuang-ai-quiet";
    private static readonly Lazy<ContentRegistry> Source = new(StandardWushuangContentRegistry.Create);

    public static void FixedDamageKeepsWineUnlessDrunkenSlashCannotBeResistedCold()
    {
        var s = Scene.Start(913, 2210); s.EmptyPeer(); s.Obtain("slash"); s.Obtain("alcohol");
        var before = s.State(); var slash = s.Action(LegalActionKind.Slash); var old = s.G.PredictPublicActionDamage(0, slash, 1);
        var again = s.G.PredictPublicActionDamage(0, slash, 1);
        Need(before == s.State() && old == again && old.HpLossAmount == 1 && old.Wushuang?.TargetPayment.HpLossAmount == 2,
            "The optional public summary adds the genuine fixed-two prediction without changing legacy fields, commands, RNG, receipts or inventory.");
        Need(s.Choose(10).Action.Kind == LegalActionKind.Alcohol && s.Choose(11).Action.Kind == LegalActionKind.Slash,
            "With fixed damage and no Drunken Fist, the new actual choice uses Slash before consuming a wine that cannot increase damage.");
        s.AssertHistoricalAndOtherMode(); s.Cold(); var hp = s.V(1).Hp; var use = s.Play(s.Action(LegalActionKind.Slash)); s.PlayBoundary();
        Need(s.V(1).Hp == hp - 2 && s.E<DamageAppliedEvent>().Last(damage => damage.SourceSeat == 0 && damage.TargetSeat == 1).Amount == 2 &&
            s.E<CardUseFinishedEvent>().Count(done => done.ResolutionId == use) == 1 && s.V(0).Hand.Any(card => card.Kind == CardKind.Alcohol),
            "The accepted physical Slash really pays fixed damage two, finishes once and retains the real wine.");
        s.Cold();

        var drunk = Scene.Start(19, 2001); drunk.EmptyPeer(); drunk.EmptyOwn(); drunk.Obtain("slash"); drunk.Obtain("alcohol"); drunk.Obtain("dodge");
        var dodge = drunk.V(0).Hand.Single(card => card.Kind == CardKind.Dodge).Id; drunk.Give(dodge);
        Need(drunk.V(1).Hand.Any(card => card.Id == dodge) && drunk.Choose(11).Action.Kind == LegalActionKind.Alcohol,
            "A real enemy Dodge remains hidden from the brain; the published Drunken Fist synergy still chooses wine before the independent Slash.");
        drunk.Play(drunk.Action(LegalActionKind.Alcohol)); drunk.PlayBoundary();
        var drunkSlash = drunk.Action(LegalActionKind.Slash); var payment = drunk.G.PredictPublicActionDamage(0, drunkSlash, 1);
        Need(payment.Wushuang is { DrunkenSlashUnrespondable: true } && payment.Wushuang.TargetPayment.HpLossAmount >= 2,
            "The already consumed wine makes this current published Slash unrespondable without guessing the enemy's hand.");
        drunk.Cold(); hp = drunk.V(1).Hp; use = drunk.Play(drunk.Action(LegalActionKind.Slash)); drunk.PlayBoundary();
        Need(drunk.V(1).Hp == hp - payment.Wushuang!.TargetPayment.HpLossAmount && drunk.V(1).Hand.Any(card => card.Id == dodge) &&
            !drunk.E<ResponseRequestedEvent>().Any(request => request.SourceSeat == 0 && request.TargetSeat == 1 && request.IncomingCard == CardKind.Slash) &&
            drunk.E<CardUseFinishedEvent>().Count(done => done.ResolutionId == use) == 1,
            "The genuine drunken native Slash cannot be canceled by the actual retained Dodge, and its paid use completes once. " +
            JsonSerializer.Serialize(new { HpBefore = hp, HpAfter = drunk.V(1).Hp, payment.Wushuang, DodgeRetained = drunk.V(1).Hand.Any(card => card.Id == dodge),
                UseId = use, Finishes = drunk.E<CardUseFinishedEvent>().Where(done => done.ResolutionId == use).Take(4),
                Requests = drunk.E<ResponseRequestedEvent>().TakeLast(8), Damage = drunk.E<DamageAppliedEvent>().TakeLast(8),
                Uses = drunk.E<WushuangCombatUseFactEvent>().TakeLast(8) }));
        drunk.Cold();
    }

    public static void ThornsAvoidsSelfDefeatButDoesNotInventRetaliationAfterDeathCold()
    {
        var s = Scene.Start(92, 2154, ownHp: 1, legacyEnemyTactic: 2116); s.EmptyPeer(); s.Obtain("slash");
        Need(s.V(1).WushuangTacticIds!.Contains(2116), "The historical configured opening must acquire the enemy's real Thorns before its native mechanism is exercised.");
        var prediction = s.G.PredictPublicActionDamage(0, s.Action(LegalActionKind.Slash), 1).Wushuang;
        Need(prediction is { TargetPayment.IsLethal: false, RetaliationIfTargetSurvives.IsLethal: true } &&
            s.Choose(10).Action.Kind == LegalActionKind.Slash && s.Choose(11).Action.Kind == LegalActionKind.EndPlay,
            "At one HP, a nonlethal Slash into the enemy's public Thorns is an actual self-defeat risk, so the new brain retains the card.");
        s.Cold(); s.Play(s.Action(LegalActionKind.Slash)); s.Finish();
        Need(!s.V(0).IsAlive && s.V(1).IsAlive && s.G.State.WinnerTeamId == WushuangMode.AiTeamId &&
            s.E<DamageAppliedEvent>().Any(damage => damage.SourceSeat == 1 && damage.TargetSeat == 0 && damage.Amount == 1),
            "Executing the old choice through the real native damage and Thorns children actually defeats the one-HP source.");
        s.Cold();

        var finish = Scene.Start(92, 2154, ownHp: 1, enemyHp: 1, legacyEnemyTactic: 2116); finish.EmptyPeer(); finish.Obtain("slash");
        prediction = finish.G.PredictPublicActionDamage(0, finish.Action(LegalActionKind.Slash), 1).Wushuang;
        Need(prediction is { TargetPayment.IsLethal: true, RetaliationIfTargetSurvives: null } && finish.Choose(11).Action.Kind == LegalActionKind.Slash,
            "An actual finishing hit must not invent a Thorns return after the victim dies.");
        finish.Cold(); finish.Play(finish.Action(LegalActionKind.Slash)); finish.Finish();
        Need(finish.V(0).IsAlive && !finish.V(1).IsAlive && finish.G.State.WinnerTeamId == WushuangMode.PlayerTeamId &&
            !finish.E<DamageAppliedEvent>().Any(damage => damage.SourceSeat == 1 && damage.TargetSeat == 0),
            "The real finishing Slash ends the victim before a Thorns return, preserving the source and its win.");
        finish.Cold();
    }

    public static void FiniteReductionUsesPublicReceiptsAndIndependentSlashBudgetCold()
    {
        var s = Scene.Start(31, 2003, legacyEnemyTactic: 2196); s.EmptyOwn(); s.EmptyPeer(); s.Obtain("slash");
        Need(s.V(0).Hand is [{ Kind: CardKind.Slash }] && s.V(1).WushuangTacticIds!.Contains(2196) &&
            s.Choose(11).Action.Kind == LegalActionKind.EndPlay,
            "One available Slash cannot get through the enemy's two public per-round reductions, so the actual choice keeps it.");
        s.Obtain("slash"); s.Obtain("slash");
        var burstChoice = s.Choose(11);
        Need(s.V(0).Hand.Count == 3 && s.V(0).Hand.All(card => card.Kind == CardKind.Slash) && burstChoice.Action.Kind == LegalActionKind.Slash,
            "Three independent actual Slash entities and the fixture's real extra Slash quota can exhaust two finite reductions instead of refusing all aggression. " +
            JsonSerializer.Serialize(new { Old = s.Choose(10).Thought.Candidates.Take(8), Current = burstChoice.Thought.Candidates.Take(8),
                Prediction = s.G.PredictPublicActionDamage(0, s.Action(LegalActionKind.Slash), 1).Wushuang }));
        var hp = s.V(1).Hp;
        for (var hit = 0; hit < 3; hit++)
        {
            var action = s.Action(LegalActionKind.Slash); var prediction = s.G.PredictPublicActionDamage(0, action, 1).Wushuang!;
            Need(prediction.TargetPayment.HpLossAmount == (hit < 2 ? 0 : 1) && prediction.IncomingReductionSegmentsRemaining == Math.Max(0, 2 - hit),
                "Each next published action reads the real remaining allowance, including zero-damage consumption.");
            s.Cold(); s.Play(s.Action(LegalActionKind.Slash)); s.PlayBoundary();
            Need(s.V(1).Hp == hp - (hit < 2 ? 0 : 1) && s.E<WushuangIncomingReductionIssuedEvent>().Count(receipt => receipt.TargetSeat == 1) == hit + 1,
                "The native damage hook spends each allowance exactly once, even when no HP is lost.");
        }
        s.Cold();
    }

    public static void NativeIncomingTacticsKeepUnsafeInjuryDefenseAndLegacyFieldsCold()
    {
        foreach (var (seed, tactic, suit, damage) in new[]
        {
            (19, 2001, Suit.Spade, 1), (913, 2210, Suit.Spade, 2), (103, 2045, Suit.Heart, 3)
        })
        {
            var s = Scene.Start(seed, tactic, enemyHp: 3, slashSuit: suit, enemyYiji: true);
            s.EmptyOwn(); s.EmptyPeer(); s.Obtain("dodge");
            s.Give(s.V(0).Hand.Single().Id); s.Obtain("slash");
            var use = s.Play(s.Action(LegalActionKind.Slash)); s.DodgeBoundary();
            Need(s.E<WushuangCombatUseFactEvent>().Single(fact => fact.CardUseFrameId == use) is { SlashOrdinal: 1 } receipt &&
                receipt.Suit == suit && s.V(1).WushuangBuild is { InjuryDrawPerDamagePoint: 2 },
                "The native response owns the real declared public suit/ordinal and the opponent's actual current injury-draw skill.");
            var state = s.State(); var prediction = s.Incoming();
            Need(state == s.State() && prediction == s.Incoming() && prediction.HpLossAmount == 1 &&
                prediction.Wushuang?.TargetPayment.HpLossAmount == damage,
                "The real pending response adds exact native fixed-two/Heart-plus-two payment while preserving legacy fields and all native RNG, quota, payment and frame state.");
            var view = s.G.CreateSnapshot(1);
            (bool UseDodge, bool UseBagua, AiThoughtRecord Thought) Choose(int policy, PublicDamagePrediction p) =>
                new SimpleAiBrain(1, 917, policy).ChooseDodgeResponse(view, 0, 1, false, true,
                    incomingPrediction: p, legalBaguaAvailable: false);
            var current = Choose(11, prediction);
            Need(current.UseDodge == (damage != 1) && !current.UseBagua,
                "Only a genuine safe one-damage Slash permits injury management; native fixed-two and a lethal Heart-plus-two Slash retain the actual Dodge.");
            Need(JsonSerializer.Serialize(Choose(10, prediction).Thought) ==
                JsonSerializer.Serialize(Choose(10, prediction with { Wushuang = null }).Thought),
                "Historical policy ten keeps identical response decisions and seeded thoughts when the optional native incoming summary is attached.");
            s.Cold();
            Need(s.Incoming() == prediction, "Cold restore preserves the declared receipt and the exact incoming native payment prediction.");
        }
    }

    public static void NullifiedTrickClaimsItsOnceFinishedPhysicalMaterialCold()
    {
        var s = Scene.Start(186, 2008, counterspellMaterials: true);
        s.EmptyOwn(); s.EmptyPeer(); s.Obtain("drawTwo"); s.Obtain("nullification");
        var trick = s.V(0).Hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
        var counterspell = s.V(0).Hand.Single(card => card.Kind == CardKind.Nullification).Id;
        var use = s.Play(s.Action(LegalActionKind.DrawTwo)); s.NullificationBoundary(); s.Cold();
        s.PayNullification(counterspell); s.PlayBoundary();
        Need(s.E<NullificationResolvedEvent>().Any(fact => fact.ResolutionId == use && fact.EffectNullified) &&
            s.E<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use) == 1 && s.V(0).Hand.Any(card => card.Id == trick),
            "The real physical counterspell cancels its native trick, whose finished original material is claimed once without querying it as a still-processing entity.");
        Need(s.G.CardMovements.Count(move => move.CardId == trick && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished) == 1 &&
            s.G.CardMovements.Count(move => move.CardId == trick && move.From == CardLocation.DiscardPile &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == "wushuang.counterspell-claim") == 1 &&
            !s.G.CreateCardZoneDiagnostics().Any(entity => entity.Location.Zone == CardZoneKind.Processing),
            "The claim follows exactly one actual UseFinished movement, never resurrects a destroyed material, and leaves no processing entity or duplicate finish payment.");
        s.Cold();
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        private PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(player => player.Seat == seat);
        public T[] E<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 2).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())), G.CardMovements,
            Events = G.Events.Select(envelope => JsonSerializer.Serialize(envelope.Payload, envelope.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands), Zones = G.CreateCardZoneDiagnostics() });
        private IReadOnlyList<LegalAction> CardActions() => G.GetHumanLegalActions().Where(action => action.Kind is
            LegalActionKind.Slash or LegalActionKind.Alcohol or LegalActionKind.EndPlay).ToArray();
        public (LegalAction Action, AiThoughtRecord Thought) Choose(int version) => new SimpleAiBrain(0, 917, version).ChoosePlay(
            G.CreateSnapshot(0), CardActions(), 1, (action, target) => G.PredictPublicActionDamage(0, action, target));
        public LegalAction Action(LegalActionKind kind) => G.GetHumanLegalActions().First(action => action.Kind == kind);
        public void AssertHistoricalAndOtherMode()
        {
            var actions = CardActions(); var view = G.CreateSnapshot(0);
            var ordinary = new SimpleAiBrain(0, 917, 10).ChoosePlay(view, actions, 1, (action, target) =>
                G.PredictPublicActionDamage(0, action, target) with { Wushuang = null });
            var enriched = Choose(10);
            Need(JsonSerializer.Serialize(ordinary.Thought.Candidates) == JsonSerializer.Serialize(enriched.Thought.Candidates),
                "Legacy policy ten must ignore the optional summary with identical candidate actions, scores, reasons and seeded tie resolution.");
            var other = view with { Wushuang = null };
            var old = new SimpleAiBrain(0, 917, 10).ChoosePlay(other, actions, 1);
            var current = new SimpleAiBrain(0, 917, 11).ChoosePlay(other, actions, 1);
            Need(JsonSerializer.Serialize(old.Thought.Candidates) == JsonSerializer.Serialize(current.Thought.Candidates),
                "The new strategy retains every non-Wushuang candidate and RNG draw.");
        }
        public void Obtain(string kind) { Use("obtain-" + kind); PlayBoundary(); }
        public void EmptyOwn()
        {
            Use("empty-own"); PlayBoundary();
            Need(V(0).Hand.Count == 0, "The real owner-zone discard must remove every random initial hand entity before preparing a precise resource budget.");
        }
        public void EmptyPeer() { Use("empty-peer", targets: [1]); PlayBoundary(); }
        public void Give(int id) { Use("give", cards: [id], targets: [1]); PlayBoundary(); }
        public void DodgeBoundary() => Until(() => G.CreateSnapshot(1).PendingDecision is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge });
        public void NullificationBoundary() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        public void PayNullification(int cardId)
        {
            var pending = P!;
            var choice = pending.Choices.Single(option => option.Cards.SequenceEqual([cardId]));
            Accept(new AnswerPromptCommand(0, pending.PromptId, choice.Id, G.Revision));
        }
        public PublicDamagePrediction Incoming()
        {
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            var attack = global::FixtureRuntimeAccess.Engine.GetProperty("ActiveCardAttack", flags)!.GetValue(G);
            Need(attack is not null && G.CreateSnapshot(1).PendingDecision?.Kind == DecisionKind.RespondDodge,
                "Incoming prediction is read only at a genuine native Dodge response boundary.");
            return (PublicDamagePrediction)global::FixtureRuntimeAccess.Engine.GetMethod("PredictPublicIncomingDamage", flags)!
                .Invoke(G, [attack, 1, null, null, null])!;
        }
        private void Use(string activation, int[]? cards = null, int[]? targets = null) => Accept(new UseProgramSkillCommand(0,
            Driver, activation, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public long Play(LegalAction action)
        {
            var before = E<CardUseDeclaredEvent>().Select(use => use.ResolutionId).ToHashSet();
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources ?? [] });
            return E<CardUseDeclaredEvent>().Last(use => !before.Contains(use.ResolutionId)).ResolutionId;
        }
        public void PlayBoundary() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Finish() => Until(() => G.State.Status == EngineStatus.Completed);
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 250; step++)
            {
                if (done()) return;
                if (G.CreateSnapshot(0).Wushuang is { CanShop: true, Phase: WushuangShopPhase.Shopping })
                    Accept(new FinishWushuangShopCommand(0, G.Revision));
                else if (P is { PlayerSeat: 0 } pending)
                {
                    var choice = pending.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip" ||
                        choice.Parameters.GetValueOrDefault("response") is "let-die" or "take-damage" or "pass");
                    Need(choice is not null, "Unexpected human boundary: " + SnapshotJson.Serialize(G.CreateSnapshot(0)));
                    Accept(new AnswerPromptCommand(0, pending.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded real Wushuang combat fixture missed its boundary: " + SnapshotJson.Serialize(G.CreateSnapshot(0)));
        }
        private void Accept(GameCommand command) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native Wushuang command."); }
        public void Cold()
        {
            var state = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(state == State(), "Cold restore retains both filtered views, public tactic purchases/allowances, native frames, physical entities and accepted journal.");
        }
        public static Scene Start(int seed, int tactic, int ownHp = 4, int enemyHp = 4, Suit slashSuit = Suit.Spade,
            bool enemyYiji = false, bool counterspellMaterials = false, int? legacyEnemyTactic = null)
        {
            var legacy = legacyEnemyTactic is not null || !WushuangCatalogue.CanOfferAsOpening(WushuangTactics.ById[tactic])
                ? tactic : (int?)null;
            var registry = ContentRegistry.Build(new Fixture(ownHp, enemyHp, slashSuit, enemyYiji, counterspellMaterials, legacy, legacyEnemyTactic));
            var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(seed) with
                { AiPolicyVersion = 10, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); scene.Until(() => scene.P?.Kind == DecisionKind.SelectGeneral);
            scene.Accept(new SelectGeneralCommand(0, Human, game.Revision, scene.P!.PromptId));
            if (legacy is not null) { scene.PlayBoundary(); return scene; }
            scene.Until(() => game.CreateSnapshot(0).Wushuang is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            var offer = game.CreateSnapshot(0).Wushuang!.Offers.Single(item => item.TacticId == tactic);
            scene.Accept(new BuyWushuangOfferCommand(0, offer.Id, null, game.Revision)); scene.PlayBoundary(); return scene;
        }
    }

    private sealed class Fixture(int ownHp, int enemyHp, Suit slashSuit, bool enemyYiji, bool counterspellMaterials,
        int? legacyOpeningTactic = null, int? legacyEnemyOpeningTactic = null) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:wushuang-ai-combat", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var source = Source.Value;
            WushuangLegacyOpeningFixture.CopyModeFactories(builder, source, legacyOpeningTactic, legacyEnemyOpeningTactic);
            foreach (var definition in source.Cards.Values) builder.AddCard(definition);
            foreach (var definition in source.Skills.Values) builder.AddSkill(definition);
            foreach (var definition in source.Generals.Values) builder.AddGeneral(definition);
            foreach (var definition in source.Decks.Values) builder.AddDeck(definition);
            foreach (var definition in source.Modes.Values.Where(mode => mode.Id != WushuangMode.ModeId)) builder.AddMode(definition);
            var activations = new List<object>();
            var obtains = new List<(string Id, string Kind)> { ("obtain-slash", "slash"), ("obtain-alcohol", "alcohol"), ("obtain-dodge", "dodge") };
            if (counterspellMaterials) obtains.AddRange([("obtain-drawTwo", "drawTwo"), ("obtain-nullification", "nullification")]);
            foreach (var (id, kind) in obtains)
                activations.Add(new { id, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "obtainWushuangDeckCards", target = "owner", amount = 1, outputKind = kind } } });
            activations.Add(new { id = "give", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, minTargets = 1, maxTargets = 1,
                targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "giveSelected", target = "selectedTarget" } } });
            activations.Add(new { id = "empty-peer", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                effects = new[] { new { op = "discardParticipantCards", target = "selectedTarget", amount = 64, zones = new[] { "hand" } } } });
            activations.Add(new { id = "empty-own", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                effects = new[] { new { op = "discardOwnedZoneCards", target = "owner", zones = new[] { "hand" } } } });
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Driver, revision = 1, activations, modifiers = new[] { new { id = "repeat", query = "slashLimit", operation = "add", value = 4, priority = 0 } } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "afterNormalDraw", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } } } });
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> {
                [Driver] = new { name = "真实战法准备", description = "通过原生实体取得、转交和额外杀额度准备当前合法动作。" },
                [Quiet] = new { name = "安静对手", description = "真实跳过出牌阶段。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, "Combat AI fixture") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Human, "无双决策本人", "", Driver, "qun", BaseHp: 4) { InitialHp = ownHp });
            builder.AddGeneral(new(Peer, "无双决策对手", "", Quiet, "qun", BaseHp: 4)
                { InitialHp = enemyHp, AdditionalSkillIds = enemyYiji ? ["classic:yiji"] : null });
            var cards = counterspellMaterials ? new[] { CardKind.Slash, CardKind.Alcohol, CardKind.Dodge, CardKind.DrawTwo, CardKind.Nullification } :
                new[] { CardKind.Slash, CardKind.Alcohol, CardKind.Dodge };
            builder.AddDeck(new("fixture:wushuang-ai-deck", "可追溯实体小牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 80)
                .SelectMany(_ => cards.Select(kind => new ContentDeckPhysicalCard(source.Cards.Values.First(card => card.LegacyKind == kind).Id,
                    kind == CardKind.Slash ? slashSuit : Suit.Spade, 7))).ToArray() });
            builder.AddMode(new(WushuangMode.ModeId, "真实战法单挑决策", 2, 2, new Dictionary<string, int>(), "fixture:wushuang-ai-deck", 2,
                [Human, Peer], ContentModeKind.Team, new Dictionary<string, int> { [WushuangMode.PlayerTeamId] = 1, [WushuangMode.AiTeamId] = 1 }));
        }
    }
}
