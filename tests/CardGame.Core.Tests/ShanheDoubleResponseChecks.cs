using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheDoubleResponseChecks
{
    private const string Owner = "fixture:double-response-owner", Peer = "fixture:double-response-peer";
    private const string Deck = "fixture:double-response-deck", Watch = "fixture:double-response-paid-child";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheDoubleResponsePackage(), new Fixture()));

    public static void ArrowsFreezeEachIssuedTargetAndResumeTwoPaidDodgesAfterTheirChild()
    {
        var s = Scene.Start(peerSlashes: 0, watch: true);
        var original = s.Use(0, CardKind.ArrowBarrage); var id = s.UseId(original);
        s.AtResponse(1, CardKind.Dodge);
        Need(s.Frame(id).CardAttack is { TargetSeat: 1, RequiredDodgeResponses: 1, SuccessfulDodgeResponses: 0 },
            "An unowned first target issues the original one-Dodge native attack.");
        s.Buy(StandardShanheCampaignCatalog.CurrentXulijianRewardId, 300);
        Need(s.Frame(id).CardAttack is { TargetSeat: 1, RequiredDodgeResponses: 1 } &&
            s.P is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge },
            "The real purchase preserves an already issued target attack and its pending human response.");
        var ordinary = s.Respond(1, CardKind.Dodge); s.AtResponse(2, CardKind.Dodge);
        Need(s.Frame(id).CardAttack is { TargetSeat: 2, RequiredDodgeResponses: 2, SuccessfulDodgeResponses: 0 },
            "The next target is issued after the purchase and immediately requires two Dodges.");
        var first = s.Respond(2, CardKind.Dodge); s.Until(s.IsWatch);
        Need(s.Frame(id).CardAttack is { TargetSeat: 2, RequiredDodgeResponses: 2, SuccessfulDodgeResponses: 0 } &&
            s.G.CardMovements.Count(m => m.CardId == first && m.From == CardLocation.Hand(2) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == first && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile &&
                m.Reason == CardMoveReasons.ResponseFinished) == 1,
            "The completed-response child retains the original target and finished physical payment before its response cursor advances. " +
                s.ResponseDiagnostic(id, first));
        s.ContinueWatch(); s.AtResponse(2, CardKind.Dodge);
        Need(s.Frame(id).CardAttack is { TargetSeat: 2, RequiredDodgeResponses: 2, SuccessfulDodgeResponses: 1 },
            "Returning the native child records one exact paid Dodge and reopens the same target for the second. " +
                s.ResponseDiagnostic(id, first));
        var second = s.Respond(2, CardKind.Dodge); s.Play(0);
        Need(!s.Events<DamageAppliedEvent>().Any(e => e.SourceCardUseResolutionId == id) &&
            s.Events<RequiredResponseProgressEvent>().Where(e => e.ResolutionId == id)
                .Select(e => (e.ResponderSeat, e.ResponseCount, e.RequiredResponseCount)).SequenceEqual([(2, 1, 2), (2, 2, 2)]),
            "Only the next issued target used the acquired two-Dodge quota, and both paid Dodges fully cancel its one native damage occurrence.");
        s.AssertPaidResponse(ordinary, 1); s.AssertPaidResponse(first, 2); s.AssertPaidResponse(second, 2); s.AssertPaidUse(original, 0);

        var next = s.Use(0, CardKind.ArrowBarrage); var nextId = s.UseId(next);
        s.AtResponse(1, CardKind.Dodge); var a = s.Respond(1, CardKind.Dodge);
        s.AtResponse(1, CardKind.Dodge); var b = s.Respond(1, CardKind.Dodge);
        s.AtResponse(2, CardKind.Dodge); var c = s.Respond(2, CardKind.Dodge); s.Play(0);
        Need(s.Events<RequiredResponseProgressEvent>().Where(e => e.ResolutionId == nextId)
                .Select(e => (e.ResponderSeat, e.ResponseCount, e.RequiredResponseCount)).SequenceEqual([(1, 1, 2), (1, 2, 2), (2, 1, 2)]) &&
            s.Events<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == nextId).ToArray() is [{ SourceSeat: 0, TargetSeat: 2, Amount: 1 }],
            "The later Arrow Barrage requires two per target; a single remaining real Dodge is paid once and still leaves its target taking native damage.");
        s.AssertPaidResponse(a, 1); s.AssertPaidResponse(b, 1); s.AssertPaidResponse(c, 2); s.AssertPaidUse(next, 0);
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 3).Select(seat => G.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(p => p is not null);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(seat).Players[seat];
        public long UseId(int card) => Events<CardUseDeclaredEvent>().Single(e => e.CardId == card).ResolutionId;
        public CardUseFrame Frame(long id) => G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == id);
        public string ResponseDiagnostic(long id, int card)
        {
            var attack = G.ResolutionStack.OfType<CardUseFrame>().FirstOrDefault(f => f.Id == id)?.CardAttack;
            return JsonSerializer.Serialize(new
            {
                useId = id, card, attack = attack is null ? null : new
                    { attack.TargetSeat, attack.RequiredDodgeResponses, attack.SuccessfulDodgeResponses },
                stack = G.ResolutionStack.Select(f => new { f.Id, f.Kind, f.Step }),
                prompt = P is null ? null : new { P.Kind, P.PlayerSeat, skillId = P.SkillPrompt?.SkillId },
                responses = Events<GroupResponseEvent>().Where(e => e.ResolutionId == id),
                progress = Events<RequiredResponseProgressEvent>().Where(e => e.ResolutionId == id),
                movements = G.CardMovements.Where(m => m.CardId == card)
            });
        }
        public bool IsWatch() => P?.SkillPrompt?.SkillId == Watch;
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real double-response command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!;
            var choice = p.Choices.FirstOrDefault(predicate) ?? throw new InvalidOperationException("Missing response choice: " + JsonSerializer.Serialize(p));
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
        }
        public void ContinueWatch() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The fixed response fixture ended before its native boundary.");
                if (P is null) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (P.Kind == DecisionKind.Nullification)
                    Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                else if (IsWatch()) ContinueWatch();
                else throw new InvalidOperationException("Unexpected native double-response boundary: " + JsonSerializer.Serialize(P));
            }
            throw new InvalidOperationException("Missing bounded double-response boundary: " + JsonSerializer.Serialize(P));
        }
        public void Play(int seat) => Until(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void AtResponse(int seat, CardKind kind) => Until(() => P is { } p && p.PlayerSeat == seat &&
            p.Kind == (kind == CardKind.Dodge ? DecisionKind.RespondDodge : DecisionKind.RespondSlash));
        public void EndPlay(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public int Use(int actor, CardKind kind, int? target = null)
        {
            var action = G.GetLegalActions(actor).First(a => a.CardId is { } card &&
                View(actor).Hand.Any(c => c.Id == card && c.Kind == kind) && a.ConversionSource is null &&
                (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            var id = action.CardId!.Value;
            Accept(new PlayCardCommand(actor, id, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            return id;
        }
        public int Respond(int seat, CardKind kind)
        {
            Need(P?.PlayerSeat == seat, "Only the published native responder can pay its next entity.");
            var card = View(seat).Hand.First(c => c.Kind == kind).Id;
            Answer(c => c.Cards.SequenceEqual([card]) && !c.Parameters.ContainsKey("conversion-skill-id"));
            return card;
        }
        public void Decline() => Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        public void Buy(string rewardId, int price)
        {
            var before = G.CreateShanheMarketSnapshot(0)!;
            var item = before.ShopItems.Single(i => i.Reward?.Id == rewardId);
            Need(item.GoldCost == price, "Use the actual quoted double-response price.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
            Need(G.CreateShanheMarketSnapshot(0)!.Gold == before.Gold - price &&
                Events<ShanheMarketPurchaseCommittedEvent>() is [{ Price: 300 }] &&
                Events<ShanheMarketPurchaseAppliedEvent>().Length == 1,
                "The tactic's real live-market acquisition commits and applies one exact 300-coin payment.");
        }
        public void AssertPaidResponse(int card, int actor) => Need(
            G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(actor) && m.To == CardLocation.Processing) == 1 &&
            G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "Every native response entity is paid and finished once: " + card);
        public void AssertPaidUse(int card, int actor) => Need(
            G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(actor) && m.To == CardLocation.Processing) == 1 &&
            G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
            Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == UseId(card)) == 1,
            "One physical trick owns its native use and every response round without a second payment: " + card);

        public static Scene Start(int peerSlashes, bool watch = false)
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentHanzhanRewardId, "酣战")
                    { TacticSkillId = StandardShanheDoubleResponsePackage.HanzhanSkillId, Rarity = 2 },
                new(StandardShanheCampaignCatalog.CurrentXulijianRewardId, "蓄力箭")
                    { TacticSkillId = StandardShanheDoubleResponsePackage.XulijianSkillId, Rarity = 2 },
                new("supplies", "实体试牌", StartingCards:
                    [new(CardKind.Duel, Suit.Heart, 2), new(CardKind.Duel, Suit.Club, 3),
                     new(CardKind.ArrowBarrage, Suit.Heart, 4), new(CardKind.ArrowBarrage, Suit.Club, 5),
                     new(CardKind.Slash, Suit.Spade, 6), new(CardKind.Slash, Suit.Club, 7),
                     new(CardKind.Slash, Suit.Spade, 8), new(CardKind.Slash, Suit.Club, 9)])
            ];
            var peerHand = Enumerable.Range(0, peerSlashes).Select(i => new ShanheStartingCard(CardKind.Slash, Suit.Spade, i + 1))
                .Concat([new(CardKind.Duel, Suit.Heart, 10), new(CardKind.Dodge, Suit.Heart, 11),
                    new(CardKind.Dodge, Suit.Club, 12), new(CardKind.Dodge, Suit.Heart, 13)]).ToArray();
            var definition = new ShanheCampaignDefinition("fixture:double-response", "真实双重响应", [Owner], ["supplies"],
                [new("supplies", "试牌", ShanheNodeKind.Event, ["shop", "battle"], Choices:
                    [new("take", "领取实体", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-hanzhan", "酣战", 300, RewardId: rewards[0].Id, LeavesNode: false),
                     new("buy-arrows", "蓄力箭", 300, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "固定守军", ShanheNodeKind.Battle, [],
                    [new("guard", "守军", [new(Peer) { StartingHand = peerHand },
                        new(Peer, ExtraSkillIds: watch ? [Watch] : [])
                            { StartingHand = [new(CardKind.Dodge, Suit.Heart, 1), new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Heart, 3)] }])])],
                rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { r => new MoveShanheNodeCommand("supplies", r), r => new ChooseShanheNodeOptionCommand("take", null, r),
                    r => new MoveShanheNodeCommand("battle", r) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter double-response fixture."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanSeats = [0, 1, 2], HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.Play(0); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:double-response", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddModeFeatureFactory(new HumanResponseSetupFactory());
            builder.AddModeFeatureFactory(new HumanResponseProfileFactory());
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Watch}}}","revision":1,"triggers":[
                    {"id":"paid-dodge-child","window":"cardResponseCompleted","ownerRelation":"actor","cardKinds":["dodge"],
                     "optional":false,"usageScope":"game","usageLimit":1,"effects":[
                       {"op":"chooseOption","target":"owner","resultBind":"paid-child","options":[{"id":"continue"}]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "已付响应子窗口", description = "原生响应付款后继续。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, "已付响应子窗口", "原生响应付款后继续。")
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Owner, "双重响应持有者", "supporter", "standard:none", "shu", 20));
            builder.AddGeneral(new(Peer, "真实响应对手", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "零自然摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "双重响应实战", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    // Keep production Shanhe roster, grants, deck and setup algorithms. The
    // fixture only assigns its three explicit external controllers to seats.
    private sealed class HumanResponseSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:double-response-human-setup";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase) =>
            new StandardModeSetupFactory().ValidateInitialOptions(options, phase);
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new HumanResponseSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(context));
    }

    private sealed class HumanResponseProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:double-response-human-profile";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        {
            // External seat control is the fixture's own setup contract. Keep
            // every production campaign/profile qualification after removing
            // only that unsupported UI-controller option from its profile view.
            var options = context.Options! with { HumanSeats = null };
            var hosts = new ModeHostRegistry()
                .Register<IModeSetupRandom>(() => context.GetHost<IModeSetupRandom>())
                .Register<IModeNationalRevealHost>(() => context.GetHost<IModeNationalRevealHost>())
                .Register<IModeSetupEquipmentHost>(() => context.GetHost<IModeSetupEquipmentHost>());
            return new StandardModeProfileFactory().Create(new ModeFeatureCreationContext(options,
                context.ModeDefinition!, context.ContentRegistry, hosts));
        }
    }

    private sealed class HumanResponseSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers()
        {
            var plan = inner.BuildPlayers();
            return new(plan.Seats.Select(seat => seat with { IsHuman = true }), plan.AvailableGeneralIds,
                plan.SelectionOrder, plan.ShenwuSharedGodIds, plan.ShenwuLeaderGeneralId, plan.GuanduConfiguration,
                plan.InitialActorSeat);
        }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request) => inner.GetGeneralOffers(request);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original) => inner.PrepareDeck(original);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original) => inner.ProjectDeckCardNames(original);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats) =>
            inner.PrepareSelectedEquipment(drawPile, seats);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId) =>
            inner.PrepareSelectedGeneral(seat, generalId);
    }
}
