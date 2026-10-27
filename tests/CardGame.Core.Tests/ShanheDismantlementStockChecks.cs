using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDismantlementStockChecks
{
    private const string Owner = "fixture:dismantlement-owner", Peer = "fixture:dismantlement-peer";
    private const string Identity = "fixture:dismantlement-identity", IdentityOwner = Owner + "-identity";
    private const string Watch = "fixture:dismantlement-loss-child", Deck = "fixture:dismantlement-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheDismantlementStockPackage(), new Fixture()));

    public static void PaidTiersKeepAbsoluteLimitsAndAllowStoppingAtOneOrTwo()
    {
        var s = Scene.Start(10);
        s.Dismantle(1, 1);
        s.Buy(StandardShanheCampaignCatalog.CurrentChenqibubeiRewardId, 500);
        s.Dismantle(2, 1, stop: true);
        s.Dismantle(3, 2);
        s.Buy(StandardShanheCampaignCatalog.CurrentChenqibubei2RewardId, 800);
        s.Dismantle(4, 2, stop: true);
        s.Dismantle(5, 3);
        s.Dismantle(6, 1);
        Need(s.View(1).HandCount == 0 && s.Events<TargetCardDiscardedEvent>().Length == 10 &&
            s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Watch && e.Completed) == 10,
            "The two held absolute tiers stop at three; each of ten actual losses runs its native movement child once, and the final one-card target ends naturally.");

        var ordinary = Scene.StartIdentity();
        var skills = ordinary.View(0).Skills!.Select(skill => skill.ContentId).ToArray();
        Need(skills.Contains(StandardShanheDismantlementStockPackage.SkillId) &&
            skills.Contains(StandardShanheDismantlementStockPackage.UpgradedSkillId),
            "The real ordinary identity player must actually hold both complete stock programs before testing their mode isolation.");
        var before = ordinary.View(1).HandCount;
        var material = ordinary.View(0).Hand.First(card => card.Kind == CardKind.Dismantlement);
        var action = ordinary.G.GetHumanLegalActions().First(candidate => candidate.CardId == material.Id &&
            candidate.Kind == LegalActionKind.Dismantlement && candidate.TargetSeats.SequenceEqual([1]) && candidate.TargetCardId is null);
        ordinary.Accept(new PlayCardCommand(0, material.Id, action.TargetSeats, ordinary.G.Revision, ordinary.P!.PromptId));
        // Default driving takes every offered card rather than voluntarily stopping at one.
        ordinary.Play();
        Need(ordinary.View(1).HandCount == before - 1,
            "Holding both Shanhe programs in an actual ordinary identity match must still discard exactly one target card.");
        ordinary.AssertRoot(material.Id, 1);
    }

    public static void NativeCounterspellCancelsTheWholeOriginalDiscardWithoutExtraChoices()
    {
        var s = Scene.Start(3);
        s.Buy(StandardShanheCampaignCatalog.CurrentChenqibubei2RewardId, 700);
        var original = s.View(1).Hand.Select(c => c.Id).ToArray();
        var card = s.Begin(CardKind.Dismantlement, 1);
        s.Until(() => s.P is { Kind: DecisionKind.Nullification });
        var counter = s.View(0).Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        s.Answer(choice => choice.Cards.SequenceEqual([counter])); s.Play();
        var id = s.UseId(card);
        Need(s.Events<NullificationResolvedEvent>().Single(e => e.ResolutionId == id).EffectNullified &&
            !s.Events<TargetCardDiscardedEvent>().Any(e => e.ResolutionId == id) &&
            s.View(1).Hand.Select(c => c.Id).SequenceEqual(original) &&
            !s.Events<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Watch) &&
            s.G.CardMovements.Count(m => m.CardId == counter && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "A genuinely paid counterspell cancels the original use before any discard or appended selection, without touching the target's actual cards.");
        s.AssertRoot(card, 0);
    }

    public static void PublicArmorLossRunsNativeRecoveryAndSnatchKeepsOneSelection()
    {
        var s = Scene.Start(2, armor: true);
        s.Buy(StandardShanheCampaignCatalog.CurrentChenqibubei2RewardId, 700);
        var snatch = s.Begin(CardKind.Snatch, 7); s.Play(picks: 1);
        Need(s.Events<TargetCardTakenEvent>().Count(e => e.ResolutionId == s.UseId(snatch)) == 1 && s.View(1).HandCount == 1,
            "The Dismantlement-only policy does not append another Snatch selection or take the target's second card.");
        s.AssertRoot(snatch, 0);
        var lion = s.View(1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        s.Dismantle(1, 2, publicCard: lion);
        Need(s.View(1) is { Hp: 2, HandCount: 0, Equipment.Count: 0 } &&
            s.Events<SilverLionRemovedRecoveryEvent>() is [{ PlayerSeat: 1, RecoveredAmount: 1 }] &&
            s.Events<TargetCardDiscardedEvent>().Any(e => e.PublicCardId == lion && e.FromZone == CardZoneKind.Equipment),
            "The actual public armor entity is discarded through its native loss-recovery trigger before the remaining hand card is selected; empty zones then finish the same use.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.CreateSnapshot(0).Players.Count)
            .Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public long UseId(int card) => Events<CardUseDeclaredEvent>().Single(e => e.CardId == card).ResolutionId;
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native Dismantlement command rejected."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P!; var choice = prompt.Choices.FirstOrDefault(predicate);
            Need(choice is not null, "The exact current native discard choice is unavailable.");
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, G.Revision));
        }
        public void Until(Func<bool> done, int? activeUseCard = null, int picks = int.MaxValue, bool stop = false)
        {
            var selectedStop = false;
            for (var step = 0; step < 240; step++)
            {
                if (done())
                {
                    Need(!stop || selectedStop, "A below-limit use must actually choose the published finish option.");
                    return;
                }
                if (P is { PlayerSeat: 0, Kind: DecisionKind.Nullification })
                    Answer(choice => choice.Parameters.GetValueOrDefault("response") == "pass");
                else if (P is { PlayerSeat: 0, Kind: DecisionKind.SelectTargetCard } prompt)
                {
                    var discarded = activeUseCard is { } card ? Events<TargetCardDiscardedEvent>().Count(e => e.ResolutionId == UseId(card)) : 0;
                    if (discarded >= picks)
                    {
                        Answer(choice => choice.Label == "结束额外弃置"); selectedStop = true;
                    }
                    else
                    {
                        Need(prompt.ValidCardIds.Count == 0 && prompt.Choices.Where(choice => choice.Label != "结束额外弃置")
                                .All(choice => choice.Cards.Count == 0 && choice.Label.Contains("暗牌", StringComparison.Ordinal)),
                            "Without Eyeline every current hand slot remains opaque, including all appended selections.");
                        Answer(choice => choice.Label != "结束额外弃置");
                    }
                }
                else
                {
                    Need(P is not { PlayerSeat: 0 }, $"Unexpected discard owner prompt {P?.Kind}.");
                    Accept(new AdvanceOneStepCommand(G.Revision));
                }
            }
            throw new InvalidOperationException($"The bounded native discard did not return; pending={P?.Kind}, actor={P?.PlayerSeat}.");
        }
        public void Play(int? card = null, int picks = int.MaxValue, bool stop = false) =>
            Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && G.ResolutionStack.Count == 0, card, picks, stop);
        public void Buy(string reward, int price)
        {
            var market = G.CreateShanheMarketSnapshot(0)!;
            var item = market.ShopItems.Single(i => i.Reward?.Id == reward);
            Need(item.GoldCost == price, "The actual source price and already-earned category surcharge must be shown before payment.");
            var paid = Events<ShanheMarketPurchaseCommittedEvent>().Length;
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision)); Play();
            Need(G.CreateShanheMarketSnapshot(0)!.Gold == market.Gold - price &&
                Events<ShanheMarketPurchaseCommittedEvent>().Length == paid + 1 &&
                Events<ShanheMarketPurchaseCommittedEvent>().Last().Price == price &&
                Events<ShanheMarketPurchaseAppliedEvent>().Last().RewardId == reward,
                "Each real live purchase charges its exact quote once and applies its tactic in this battle.");
        }
        public int Begin(CardKind kind, int rank, int? publicCard = null)
        {
            var card = View(0).Hand.Single(c => c.Kind == kind && c.Rank == rank);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual([1]) &&
                a.TargetCardId == publicCard && a.ConversionSource is null);
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId)); return card.Id;
        }
        public void Dismantle(int rank, int count, bool stop = false, int? publicCard = null)
        {
            var card = Begin(CardKind.Dismantlement, rank, publicCard); Play(card, count, stop);
            AssertRoot(card, count);
        }
        public void AssertRoot(int card, int discarded)
        {
            var id = UseId(card);
            var removed = Events<TargetCardDiscardedEvent>().Where(e => e.ResolutionId == id).ToArray();
            Need(removed.Length == discarded && removed.All(e => e.SourceSeat == 0 && e.TargetSeat == 1) &&
                G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == id) == 1 &&
                !G.ResolutionStack.OfType<CardUseFrame>().Any(use => use.Id == id),
                "Every actual discard shares one root declaration, one real material payment and one finish, including voluntary stopping and a depleted target.");
        }
        public static Scene Start(int cards, bool armor = false)
        {
            ShanheRewardDefinition[] rewards =
            [new(StandardShanheCampaignCatalog.CurrentChenqibubeiRewardId, "趁其不备")
                { TacticSkillId = StandardShanheDismantlementStockPackage.SkillId, Rarity = 3 },
             new(StandardShanheCampaignCatalog.CurrentChenqibubei2RewardId, "趁其不备Ⅱ")
                { TacticSkillId = StandardShanheDismantlementStockPackage.UpgradedSkillId, Rarity = 4 },
             new("cards", "真实拆顺", StartingCards: Enumerable.Range(1, 6)
                .Select(rank => new ShanheStartingCard(CardKind.Dismantlement, Suit.Spade, rank))
                .Concat([new(CardKind.Snatch, Suit.Diamond, 7), new(CardKind.Nullification, Suit.Spade, 11)]).ToArray())];
            var definition = new ShanheCampaignDefinition("fixture:dismantlement-stock", "原生追加拆牌", [Owner], ["cards"],
                [new("cards", "实体补给", ShanheNodeKind.Event, ["shop", "battle"], Choices: [new("take", "领取", RewardId: "cards")]),
                 new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("base", "趁其不备", 500, RewardId: rewards[0].Id, LeavesNode: false),
                     new("upgraded", "趁其不备Ⅱ", 700, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "固定守军", ShanheNodeKind.Battle, [], [new("guard", "守军",
                    [new(Peer, Hp: armor ? 1 : 20)
                    {
                        StartingHand = Enumerable.Range(1, cards).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, rank)).ToArray(),
                        StartingEquipment = armor ? [new(CardKind.SilverLion, Suit.Club, 1)] : []
                    }])])], rewards, StartingGold: 3000);
            var session = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true, UnlockedRewardIds = [rewards[0].Id, rewards[1].Id] }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { r => new MoveShanheNodeCommand("cards", r), r => new ChooseShanheNodeOptionCommand("take", null, r), r => new MoveShanheNodeCommand("battle", r) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter discard fixture."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
        public static Scene StartIdentity()
        {
            var game = GameEngine.CreateStandard(new()
            {
                Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Identity,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand());
            for (var step = 0; step < 80 && game.PendingDecision is not { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }; step++)
                s.Accept(new AdvanceOneStepCommand(game.Revision));
            Need(game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }, "The ordinary identity table must expose its real general selection.");
            s.Accept(new SelectGeneralCommand(0, IdentityOwner, game.Revision, game.PendingDecision!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:dismantlement-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var watch = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-card-loss","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["card.effect.dismantlement"],
                    "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-child","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实失牌子结算", description = "原生拆牌后返回原选择。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, "真实失牌子结算", "原生拆牌后返回原选择。")
                { Program = watch.Programs[Watch], ProgramPresentation = watch.Presentations[Watch] });
            builder.AddGeneral(new(Owner, "拆牌使用者", "supporter", "standard:none", "shu", 30));
            builder.AddGeneral(new(Peer, "实际失牌角色", "supporter", Watch, "wei", 20));
            builder.AddGeneral(new(IdentityOwner, "普通身份双档持有者", "supporter", StandardShanheDismantlementStockPackage.SkillId,
                "shu", 30, [StandardShanheDismantlementStockPackage.UpgradedSkillId]));
            for (var index = 1; index <= 3; index++) builder.AddGeneral(new(Peer + index, "普通身份失牌者", "supporter", Watch, "wei", 20));
            builder.AddCard(new("fixture:dismantlement-silver-lion", "白银狮子", "装备牌", "原生失去防具后回复。", CardKind.SilverLion));
            builder.AddDeck(new(Deck, "零自然摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "原生拆牌小局", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
            builder.AddDeck(new(Deck + "-identity", "普通身份真实过河", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dismantlement", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Identity, "普通身份场", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck + "-identity",
                GeneralCandidateCount: 4, GeneralPoolIds: [IdentityOwner, Peer + "1", Peer + "2", Peer + "3"]));
        }
    }
}
