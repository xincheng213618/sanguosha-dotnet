using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheMultiTargetTrickChecks
{
    private const string Conversion = "fixture:multi-trick-peach-duel";
    private const string Owner = "fixture:multi-trick-owner", Peer = "fixture:multi-trick-peer", Deck = "fixture:multi-trick-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheMultiTargetTricksPackage(), new StandardShanheXuyanPackage(), new Fixture()));

    public static void PaidXuyanRevealsSameSuitPaymentAndReturnsFromNativeRevealChildren()
    {
        var s = Scene.Start(xuyan: true);
        var originalCost = s.Hand(0).Single(card => card.Kind == CardKind.Slash).Id;
        var retainedCost = s.Hand(0).Single(card => card.Kind == CardKind.IronChain && card.Suit == Suit.Spade).Id;
        var first = s.BeginFirePayment();
        Need(s.P!.Prompt.Contains("弃置", StringComparison.Ordinal), "Unowned stock leaves ordinary Fire Attack's real discard cost.");
        s.PayFire(originalCost);
        Need(!s.Hand(0).Any(card => card.Id == originalCost) && s.G.CardMovements.Count(move => move.CardId == originalCost &&
            move.Reason == CardMoveReasons.FireAttackDiscardFinished) == 1 &&
            s.Events<DamageAppliedEvent>().Single(fact => fact.SourceCardUseResolutionId == first).Amount == 1,
            "Before buying the tactic the same native Fire Attack really discards its same-suit payment and applies one damage.");

        var second = s.BeginFirePayment();
        s.Buy(StandardShanheCampaignCatalog.CurrentXuyanRewardId, 300, resumePlay: false);
        Need(s.P is { Kind: DecisionKind.FireAttackDiscard, PlayerSeat: 0 } &&
            s.P.Prompt.Contains("展示", StringComparison.Ordinal) && s.P.Choices.Any(choice => choice.Cards.SequenceEqual([retainedCost])),
            "Buying Xuyan while the native payment is pending refreshes that same owning use to a real reveal choice.");
        s.PayFire(retainedCost);
        var history = s.G.Events.Select(item => item.Payload).ToArray();
        var revealedIndex = Array.FindIndex(history, fact => fact is CardsRevealedEvent reveal &&
            reveal.ResolutionId == second && reveal.Cards.Any(card => card.Id == retainedCost));
        var benefitIndex = Array.FindIndex(history, fact => fact is MouXiaoQiaoOlDrawReturnedEvent { Requested: 1, Actual: 1 });
        var damageIndex = Array.FindIndex(history, fact => fact is DamageAppliedEvent damage && damage.SourceCardUseResolutionId == second);
        Need(revealedIndex >= 0 && benefitIndex > revealedIndex && damageIndex > benefitIndex &&
            s.Hand(0).Any(card => card.Id == retainedCost) && !s.G.CardMovements.Any(move => move.CardId == retainedCost &&
                move.From == CardLocation.Hand(0)) && s.G.CreateSnapshot(0).Players[2].Hp == 28 &&
            s.Events<FireAttackResolvedEvent>().Single(fact => fact.ResolutionId == second).MatchingDiscardCardId == retainedCost,
            "The exact retained hand card is publicly revealed; its genuine Jueyan draw child returns before Fire Attack damage, with no discarded substitute.");

        var third = s.BeginFirePayment();
        s.PayFire(retainedCost);
        Need(s.Hand(0).Any(card => card.Id == retainedCost) && s.Events<CardsRevealedEvent>()
                .Count(fact => fact.Cards.Any(card => card.Id == retainedCost)) == 2 &&
            s.Events<MouXiaoQiaoOlDrawReturnedEvent>().Length == 1 && s.G.CreateSnapshot(0).Players[2].Hp == 27,
            "A later original Fire Attack can reveal the same retained payment again; its first-suit reward cannot be paid twice.");
        foreach (var use in new[] { first, second, third })
        {
            var declared = s.Events<CardUseDeclaredEvent>().Single(fact => fact.ResolutionId == use);
            Need(s.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use) == 1 &&
                s.G.CardMovements.Count(move => move.CardId == declared.CardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(move => move.CardId == declared.CardId && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
                "Every actual Fire Attack still pays and finishes its original entity exactly once.");
        }
    }

    public static void PaidHengjiangUsesOneChainWithIndependentCountersAndKeepsRecast()
    {
        var s = Scene.Start();
        s.Maximum(CardKind.IronChain, 2);
        s.Reject(CardKind.IronChain, [0, 1, 2, 3]);
        s.Buy(StandardShanheCampaignCatalog.CurrentHengjiangRewardId, 500);
        s.Maximum(CardKind.IronChain, 4);
        Need(s.G.GetCombatDistance(0, 2) == 2 && !s.G.GetHumanLegalActions().Any(action =>
                action.Kind == LegalActionKind.Slash && action.TargetSeats.Contains(2)),
            "Extra trick targets do not extend the holder's native Slash range.");
        var card = s.Use(CardKind.IronChain, [0, 1, 2, 3], counterTarget: 1);
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == card);
        Need(s.G.CreateSnapshot(0).Players.Select(p => p.IsChained).SequenceEqual([true, false, true, true]) &&
            s.Events<IronChainStateChangedEvent>().Where(e => e.ResolutionId == use.ResolutionId)
                .Select(e => e.TargetSeat).Order().SequenceEqual([0, 2, 3]) &&
            s.Events<NullificationResolvedEvent>().Where(e => e.ResolutionId == use.ResolutionId)
                .Select(e => (e.TargetSeat, e.EffectNullified)).SequenceEqual([(0, false), (1, true), (2, false), (3, false)]),
            "The same paid Iron Chain resolves self, ally and distant enemies independently; the ally's counterspell cannot cancel later targets.");
        s.AssertPaid(card);
        var recast = s.Hand(0).Single(c => c.Kind == CardKind.IronChain).Id;
        var hand = s.Hand(0).Count;
        Need(s.G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Recast && a.CardId == recast && a.TargetSeats.Count == 0),
            "The expanded Iron Chain retains its published zero-target native recast action.");
        s.Accept(new RecastCardCommand(0, recast, s.G.Revision, s.P!.PromptId)); s.Play();
        Need(s.Events<CardRecastEvent>().Single(e => e.CardId == recast).DrawCount == 1 && s.Hand(0).Count == hand &&
            !s.Events<CardUseDeclaredEvent>().Any(e => e.CardId == recast) &&
            s.G.CardMovements.Count(m => m.CardId == recast && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1,
            "Recasting pays the second actual card once and draws one card without fabricating another trick use.");
    }

    public static void PaidDantiaowangKeepsOneDuelAndEachTargetsPhysicalSlashResponse() => VerifyPaidDuel(converted: false);

    public static void PaidDantiaowangKeepsConvertedMaterialAndNativeDuelResponses() => VerifyPaidDuel(converted: true);

    private static void VerifyPaidDuel(bool converted)
    {
        var s = Scene.Start(converted);
        var materialKind = converted ? CardKind.Peach : CardKind.Duel;
        s.Maximum(materialKind, 1);
        s.Reject(materialKind, [1, 2, 3]);
        s.Buy(StandardShanheCampaignCatalog.CurrentDantiaowangRewardId, 300);
        s.Maximum(materialKind, 3);
        Need(s.G.GetHumanLegalActions().Where(a => a.Kind == LegalActionKind.Duel).All(a => !a.TargetSeats.Contains(0)),
            "The target expansion never makes the original Duel user a legal target.");
        var paidSlashes = s.Hand(2).Where(c => c.Kind == CardKind.Slash).Select(c => c.Id)
            .Concat(s.Hand(0).Where(c => c.Kind == CardKind.Slash).Select(c => c.Id)).ToArray();
        var card = s.Use(materialKind, [1, 2, 3], counterTarget: 1);
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == card);
        if (converted)
            Need(use.CardKind == CardKind.Duel && s.Events<ProgramViewAsConvertedEvent>().Single(e => e.FrameId == use.ResolutionId) is
                    { SkillId: Conversion, OwnerSeat: 0, OutputKind: CardKind.Duel, IsUse: true } conversion &&
                conversion.PhysicalCardIds.SequenceEqual([card]) && conversion.TargetSeats.SequenceEqual([1, 2, 3]),
                "The opt-in single-card conversion declares one actual Duel with the original Peach entity and all three native targets.");
        var responses = s.Events<DuelResponseEvent>().Where(e => e.ResolutionId == use.ResolutionId).ToArray();
        Need(!responses.Any(e => e.ResponderSeat == 1) &&
            responses.Where(e => e.UsedSlash).Select(e => e.SlashCardId!.Value).Order().SequenceEqual(paidSlashes.Order()) &&
            s.Events<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == use.ResolutionId)
                .Select(e => (e.SourceSeat, e.TargetSeat, e.Amount)).SequenceEqual([(2, 0, 1), (0, 3, 1)]) &&
            s.G.CreateSnapshot(0).Players.Select(p => p.Hp).SequenceEqual([29, 30, 30, 29]),
            "One countered friendly target does not stop the later real Duel: both directions pay their Slashes, then the last opponent resolves separately.");
        foreach (var slash in paidSlashes)
            Need(s.G.CardMovements.Count(m => m.CardId == slash && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
                "Every responding player pays and discards its own original physical Slash exactly once.");
        s.AssertPaid(card);
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(decision => decision is not null);
        public IReadOnlyList<CardSnapshot> Hand(int seat) => G.CreateSnapshot(seat).Players[seat].Hand;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native multi-target trick command rejected."); }
        public void Play(int? counterTarget = null)
        {
            for (var step = 0; step < 300; step++)
            {
                if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && G.ResolutionStack.Count == 0) return;
                if (P is { } prompt)
                {
                    PromptChoice? choice = null;
                    if (prompt.Kind == DecisionKind.Nullification)
                    {
                        var counter = Hand(prompt.PlayerSeat).FirstOrDefault(c => c.Kind == CardKind.Nullification);
                        if (prompt.PlayerSeat == 0 && counter is not null && counterTarget is { } wanted &&
                            Events<NullificationRequestedEvent>().Last().TargetSeat == wanted)
                            choice = prompt.Choices.FirstOrDefault(c => c.Cards.SequenceEqual([counter.Id]));
                        choice ??= prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") == "pass");
                    }
                    else if (prompt.Kind == DecisionKind.RespondSlash)
                    {
                        var slashes = Hand(prompt.PlayerSeat).Where(c => c.Kind == CardKind.Slash).Select(c => c.Id).ToHashSet();
                        choice = prompt.Choices.FirstOrDefault(c => c.Cards.Count == 1 && slashes.Contains(c.Cards[0])) ??
                            prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    }
                    Need(choice is not null, $"Unexpected native trick decision {prompt.Kind} for seat {prompt.PlayerSeat}.");
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The paid native trick did not return to its original Play boundary: " +
                JsonSerializer.Serialize(new
                {
                    pending = P is { } pending ? new { pending.Kind, pending.PlayerSeat, pending.SourceSeat,
                        pending.TargetSeat, pending.RequiredCardKind, pending.PromptId } : null,
                    state = G.State,
                    frames = G.ResolutionStack.Select(frame => new
                    {
                        type = frame.GetType().Name, frame.Id, frame.Kind, frame.Step,
                        use = frame is CardUseFrame use ? new { use.SourceSeat, use.CardId, use.CardKind,
                            use.TargetSeats, use.TargetIndex, use.SequentialTrick, use.CardAttack, use.Continuations } : null,
                        counter = frame is NullificationWindowFrame window ? new { window.SourceSeat, window.TargetSeats,
                            window.CandidateIndex, window.ChainDepth, window.EffectNullified } : null,
                        program = frame is ProgramSkillFrame program ? new { program.OwnerSeat, program.SkillId,
                            program.InstructionIndex, program.TriggerId } : null
                    }),
                    recentEvents = G.Events.TakeLast(12).Select(envelope => new
                    {
                        type = envelope.Payload.GetType().Name,
                        payload = JsonSerializer.Serialize(envelope.Payload, envelope.Payload.GetType())
                    })
                }));
        }
        public void Maximum(CardKind kind, int expected)
        {
            var card = Hand(0).First(c => c.Kind == kind);
            var revision = G.Revision; var moves = G.CardMovements.Count;
            for (var repeat = 0; repeat < 3; repeat++)
                Need(G.GetHumanLegalActions().Where(a => a.CardId == card.Id && a.Kind != LegalActionKind.Recast)
                        .Max(a => a.TargetSeats.Count) == expected,
                    "Only the actually owned tactic changes this real card's legal target count.");
            Need(G.Revision == revision && G.CardMovements.Count == moves, "Target queries must not pay or declare the card.");
        }
        public void Reject(CardKind kind, int[] targets)
        {
            var card = Hand(0).First(c => c.Kind == kind); var revision = G.Revision; var moves = G.CardMovements.Count;
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.Kind != LegalActionKind.Recast);
            var result = G.Submit(new PlayCardCommand(0, card.Id, targets, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            Need(!result.Accepted && G.Revision == revision && G.CardMovements.Count == moves && Hand(0).Any(c => c.Id == card.Id),
                "Registering the future product cannot authorize forged extra targets or consume its real card.");
        }
        public void Buy(string reward, int price, bool resumePlay = true)
        {
            var market = G.CreateShanheMarketSnapshot(0)!;
            var item = market.ShopItems.Single(i => i.Reward?.Id == reward);
            Need(item.GoldCost == price, "The actual product retains its source price.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            if (resumePlay) Play();
            Need(G.CreateShanheMarketSnapshot(0)!.Gold == market.Gold - price &&
                Events<ShanheMarketPurchaseCommittedEvent>().Single().Price == price &&
                Events<ShanheMarketPurchaseAppliedEvent>().Single().RewardId == reward,
                "A real paid market grant takes effect in the current battle and charges exactly once.");
        }
        public int Use(CardKind kind, int[] targets, int counterTarget)
        {
            var card = Hand(0).First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().Single(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource }); Play(counterTarget);
            return card.Id;
        }
        public long BeginFirePayment()
        {
            var card = Hand(0).First(item => item.Kind == CardKind.FireAttack);
            var action = G.GetHumanLegalActions().Single(item => item.CardId == card.Id && item.TargetSeats.SequenceEqual([2]));
            Accept(new PlayCardCommand(0, card.Id, [2], G.Revision, P!.PromptId, action.PlayedCardKind));
            for (var step = 0; step < 100; step++)
            {
                if (P is { Kind: DecisionKind.FireAttackDiscard, PlayerSeat: 0 })
                    return Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == card.Id).ResolutionId;
                if (P is { } prompt)
                {
                    var choice = prompt.Kind == DecisionKind.FireAttackReveal
                        ? prompt.Choices.Single(item => item.Cards.Count == 1 && Hand(2).Any(revealed => revealed.Id == item.Cards[0] && revealed.Suit == Suit.Spade))
                        : prompt.Choices.Single(item => item.Parameters.GetValueOrDefault("response") == "pass");
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native Fire Attack did not reach its original payment window.");
        }
        public void PayFire(int cardId)
        {
            var prompt = P!;
            var choice = prompt.Choices.Single(item => item.Cards.SequenceEqual([cardId]));
            Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision)); Play();
        }
        public void AssertPaid(int card)
        {
            var use = Events<CardUseDeclaredEvent>().Single(e => e.CardId == card);
            Need(G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1 &&
                Events<NullificationRespondedEvent>().Single(e => e.ResolutionId == use.ResolutionId) is { TargetSeat: 1, EffectNullified: true },
                "All target windows retain one original declared use, one material payment and one final completion.");
        }
        public static Scene Start(bool converted = false, bool xuyan = false)
        {
            ShanheRewardDefinition[] rewards =
            [new(StandardShanheCampaignCatalog.CurrentHengjiangRewardId, "横江锁")
                { TacticSkillId = StandardShanheMultiTargetTricksPackage.HengjiangSkillId, Rarity = 3 },
             new(StandardShanheCampaignCatalog.CurrentDantiaowangRewardId, "单挑王")
                { TacticSkillId = StandardShanheMultiTargetTricksPackage.DantiaowangSkillId, Rarity = 2 },
             new(StandardShanheCampaignCatalog.CurrentXuyanRewardId, "虚焰")
                { TacticSkillId = StandardShanheXuyanPackage.SkillId, Rarity = 2 },
             new("cards", "真实锦囊", SkillId: xuyan ? "mou-ol:jueyan" : null, StartingCards:
                [new(CardKind.IronChain, Suit.Club, 1), new(CardKind.IronChain, Suit.Spade, 2),
                 new(CardKind.Duel, Suit.Heart, 3), new(CardKind.Nullification, Suit.Club, 4), new(CardKind.Slash, Suit.Spade, 5),
                 .. (converted ? new[] { new ShanheStartingCard(CardKind.Peach, Suit.Heart, 10) } : []),
                 .. (xuyan ? Enumerable.Range(10, 3).Select(rank => new ShanheStartingCard(CardKind.FireAttack, Suit.Heart, rank)) : [])])];
            var definition = new ShanheCampaignDefinition("fixture:multi-tricks", "多目标原生锦囊", [Owner], ["cards"],
                [new("cards", "实体补给", ShanheNodeKind.Event, ["shop", "battle"], Choices: [new("take", "领取", RewardId: "cards")]),
                 new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("chain", "横江锁", 500, RewardId: rewards[0].Id, LeavesNode: false),
                     new("duel", "单挑王", 300, RewardId: rewards[1].Id, LeavesNode: false),
                     new("xuyan", "虚焰", 300, RewardId: rewards[2].Id, LeavesNode: false)]),
                 new("battle", "敌友", ShanheNodeKind.Battle, [],
                    [new("peers", "实际敌友", [new(Peer) { StartingHand = [new(CardKind.Slash, Suit.Club, 8), new(CardKind.Slash, Suit.Spade, 9)] },
                        new(Peer) { StartingHand = [] }], Allies: [new(Peer) { StartingHand = [] }])])], rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { r => new MoveShanheNodeCommand("cards", r), r => new ChooseShanheNodeOptionCommand("take", null, r),
                  r => new MoveShanheNodeCommand("battle", r) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter native trick fixture."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:multi-target-tricks", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mou:xiao-qiao-ol");
            builder.AddModeFeatureFactory(new HumanSetupFactory());
            builder.AddModeFeatureFactory(new HumanProfileFactory());
            var conversion = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Conversion}}","revision":1,"viewAs":[
                    {"id":"peach-duel","inputKinds":["peach"],"inputSuits":[],"sourceZones":["hand"],
                     "outputKind":"duel","forPlay":true,"forResponse":false,"singleCardTrickUse":true}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{Conversion}}":{"name":"实体桃转决斗","description":"单牌普通锦囊原生转化。"} } }
                """);
            builder.AddSkill(new(Conversion, "实体桃转决斗", "单牌普通锦囊原生转化。")
                { Program = conversion.Programs[Conversion], ProgramPresentation = conversion.Presentations[Conversion] });
            builder.AddGeneral(new(Owner, "真实锦囊使用者", "supporter", Conversion, "shu", 30));
            builder.AddGeneral(new(Peer, "独立响应者", "supporter", "standard:none", "wei", 30));
            builder.AddDeck(new(Deck, "固定零自然摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "原生多目标锦囊", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    // Delegate the production roster, grants, deck and profile; only expose its four response controllers.
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:multi-trick-human-setup";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase) =>
            new StandardModeSetupFactory().ValidateInitialOptions(options, phase);
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(context));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:multi-trick-human-profile";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext context) => new StandardModeProfileFactory().Create(
            new ModeFeatureCreationContext(context.Options! with { HumanSeats = null }, context.ModeDefinition!, context.ContentRegistry,
                new ModeHostRegistry().Register<IModeSetupRandom>(() => context.GetHost<IModeSetupRandom>())
                    .Register<IModeNationalRevealHost>(() => context.GetHost<IModeNationalRevealHost>())
                    .Register<IModeSetupEquipmentHost>(() => context.GetHost<IModeSetupEquipmentHost>())));
    }
    private sealed class HumanSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers()
        {
            var plan = inner.BuildPlayers();
            return new(plan.Seats.Select(seat => seat with { IsHuman = true }), plan.AvailableGeneralIds,
                plan.SelectionOrder, plan.ShenwuSharedGodIds, plan.ShenwuLeaderGeneralId, plan.GuanduConfiguration, plan.InitialActorSeat);
        }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request) => inner.GetGeneralOffers(request);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original) => inner.PrepareDeck(original);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original) => inner.ProjectDeckCardNames(original);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats) =>
            inner.PrepareSelectedEquipment(drawPile, seats);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId) => inner.PrepareSelectedGeneral(seat, generalId);
    }
}
