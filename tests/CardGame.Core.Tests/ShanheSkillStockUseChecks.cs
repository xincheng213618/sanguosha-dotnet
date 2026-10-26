using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSkillStockUseChecks
{
    private const string Qixi = "classic:qixi", Wushuang = "classic:wushuang";
    private const string Owner = "fixture:skill-stock-user", Enemy = "fixture:skill-stock-defender", Deck = "fixture:skill-stock-use-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

    public static void PurchasedQixiAndWushuangUsePhysicalConversionAndTwoDodges()
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var rewards = new[] { "build-skill-3021", "build-skill-3029" }
            .Select(id => current.Rewards.Single(reward => reward.Id == id)).ToArray();
        var kit = new ShanheRewardDefinition("fixture:skill-stock-kit", "原生实体材料", StartingCards:
            [new(CardKind.Dodge, Suit.Club, 7), new(CardKind.Slash, Suit.Heart, 8)]);
        var definition = new ShanheCampaignDefinition("fixture:skill-stock-use", "真实技能购入与使用", [Owner], ["shop"],
        [
            new("shop", "构筑技能商店", ShanheNodeKind.Shop, ["battle"], Choices:
                rewards.Select(reward => new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                    GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[reward.Id].Price,
                    RewardId: reward.Id, LeavesNode: false))
                    .Append(new("kit", "领用实体材料", RewardId: kit.Id, LeavesNode: false)).ToArray()),
            new("battle", "真实双人交锋", ShanheNodeKind.Battle, [],
                [new("defender", "需要防住致命杀的守军", [new(Enemy, Hp: 1)
                    { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Diamond, 3), new(CardKind.Dodge, Suit.Heart, 4)] }])])
        ], [.. rewards, kit], StartingGold: 900, ExtraSkillSlots: 2);
        var session = ShanheCampaignSession.Create(definition,
            new(113, Owner) { UnlockedRewardIds = [.. rewards.Select(reward => reward.Id), kit.Id] }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", session.Revision));
        Need(session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.ShopItems.Where(item => item.Reward?.SkillId is not null)
                .All(item => item is { State: ShanheShopItemState.Available, RequiresSkillReplacement: false }),
            "Both actual source skills are initially unearned and available for purchase in two real free skill slots.");
        foreach (var (reward, price) in rewards.Zip(new[] { 300, 600 }))
        {
            var item = session.Snapshot.ShopItems.Single(item => item.Reward?.Id == reward.Id);
            Need(item.GoldCost == price, "The actual source stock retains Qixi's three hundred and Wushuang's six hundred copper price.");
            Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
        }
        Campaign(new ChooseShanheNodeOptionCommand("kit", null, session.Revision));
        Need(session.Snapshot.Gold == 0 && session.Snapshot.ExtraSkillIds.SequenceEqual([Qixi, Wushuang]) &&
            session.Snapshot.ExtraSkillSlots == 2 && session.Snapshot.TacticIds.Count == 0,
            "Real purchase commands pay nine hundred copper and grant the exact ordinary versions in two skill slots, without turning either skill into a tactic.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Campaign(new MoveShanheNodeCommand("battle", session.Revision));
        Need(session.PendingBattleSetup!.ExtraSkillIds.SequenceEqual([Qixi, Wushuang]),
            "Only the two genuinely purchased ordinary skills enter this battle setup.");
        var s = Scene.Start(session.PendingBattleSetup!);
        var material = s.View().Hand.Single(card => card.Kind == CardKind.Dodge);
        var slash = s.View().Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var enemyCards = s.View(1).Hand.Select(card => card.Id).ToHashSet();
        var conversion = s.Game.GetHumanLegalActions().First(action => action.CardId == material.Id &&
            action.Kind == LegalActionKind.Dismantlement && action.ConversionSource?.SkillId == Qixi && action.TargetSeats.SequenceEqual([1]));
        var converted = s.BeginTargetSelection(conversion);
        Need(converted is { Type: CardActionType.Use, ActorSeat: 0, ProviderSeat: 0, EffectiveKind: CardKind.Dismantlement } &&
            converted.PhysicalCards is [{ CardKind: CardKind.Dodge, From: var origin }] && origin == CardLocation.Hand(0) &&
            converted.PhysicalCards.Single().CardId == material.Id && converted.ConversionChain.Any(source => source.SkillId == Qixi),
            "The live native Dismantlement retains the purchased Qixi source and the exact paid black Dodge while its real target-card choice is pending.");
        s.CompletePendingUse();
        var discarded = s.Game.CardMovements.Single(move => enemyCards.Contains(move.CardId) &&
            move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Dismantlement).CardId;
        Need(material.Suit == Suit.Club && converted.PhysicalCards.Single().CardId == material.Id &&
            converted.ConversionChain.Any(source => source.SkillId == Qixi) &&
            s.Events<TargetCardDiscardedEvent>() is [{ SourceSeat: 0, TargetSeat: 1, FromZone: CardZoneKind.Hand }] &&
            s.View(1).HandCount == 2 && s.View(1).Hand.All(card => card.Id != discarded) &&
            s.PaidOnce(material.Id, 0, CardMoveReasons.Use, CardMoveReasons.UseFinished) &&
            s.Game.CardMovements.Count(move => move.CardId == discarded && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.DismantlementFinished) == 1,
            "The purchased Qixi converts the original black Dodge into one actual Dismantlement, pays that exact entity once and truly discards one of the target's physical hand cards.");
        var remainingDodges = s.View(1).Hand.Select(card => card.Id).ToHashSet();
        s.Play(s.Game.GetHumanLegalActions().First(action => action.CardId == slash && action.ConversionSource is null &&
            action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1])));
        var progress = s.Events<RequiredResponseProgressEvent>();
        var responses = s.Events<CardRespondedEvent>().Where(fact => fact.ResponderSeat == 1 && fact.EffectiveCardKind == CardKind.Dodge).ToArray();
        Need(progress.Select(fact => (fact.ResponseCount, fact.RequiredResponseCount)).SequenceEqual([(1, 2), (2, 2)]) &&
            progress.All(fact => fact.SkillOwnerSeat == 0 && fact.ResponderSeat == 1 && fact.RequiredCardKind == CardKind.Dodge) &&
            responses.Select(fact => fact.CardId).ToHashSet().SetEquals(remainingDodges) && responses.Length == 2 &&
            responses.All(fact => s.PaidOnce(fact.CardId, 1, CardMoveReasons.Respond, CardMoveReasons.ResponseFinished)) &&
            s.PaidOnce(slash, 0, CardMoveReasons.Use, CardMoveReasons.UseFinished) &&
            s.View(1) is { Hp: 1, HandCount: 0 } && s.Events<DamageAppliedEvent>().Length == 0,
            "The actually purchased Wushuang makes the following real Slash require two distinct paid Dodges; the first payment advances only to one of two, and the second prevents the single attack without damage.");
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected actual skill-stock purchase command."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native purchased-skill use."); }
        public void Play(LegalAction action)
        {
            SubmitUse(action, action.TargetCardId);
            ReachPlay();
        }
        public CardActionContext BeginTargetSelection(LegalAction action)
        {
            SubmitUse(action, null);
            Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.SelectTargetCard });
            return Game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.SourceSeat == 0 &&
                frame.CardKind == CardKind.Dismantlement).Action ??
                throw new InvalidOperationException("The real converted target-card selection lost its native action.");
        }
        public void CompletePendingUse() => ReachPlay();
        private void SubmitUse(LegalAction action, int? targetCardId) =>
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, Game.Revision, Prompt!.PromptId,
                action.PlayedCardKind, targetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        public bool PaidOnce(int id, int seat, CardMoveReason payment, CardMoveReason finish) =>
            Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(seat) &&
                move.To == CardLocation.Processing && move.Reason == payment) == 1 &&
            Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == finish) == 1;
        private void ReachPlay() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        private void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                if (Prompt is { Kind: DecisionKind.SelectTargetCard } target)
                    Accept(new AnswerPromptCommand(0, target.PromptId, target.Choices[0].Id, Game.Revision));
                else if (Prompt is { Kind: DecisionKind.Nullification } counter)
                    Accept(new AnswerPromptCommand(0, counter.PromptId,
                        counter.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "pass").Id, Game.Revision));
                else
                {
                    Need(Prompt?.PlayerSeat != 0, "The bounded purchased-skill scene reached an unexpected human prompt.");
                    Accept(new AdvanceOneStepCommand(Game.Revision));
                }
            }
            throw new InvalidOperationException("Missing bounded purchased-skill play boundary.");
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-skill-stock-use", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:gan-ning", "classic:lu-bu");
            builder.AddGeneral(new(Owner, "技能购买者", "supporter", "standard:none", "shu", 12));
            builder.AddGeneral(new(Enemy, "实际响应守军", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "零自然摸牌实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index =>
                new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "购入技能真实用牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
