using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheFenchengSkillStockChecks
{
    private const string Fencheng = "classic:fencheng", Jianxiong = "ol:jianxiong", Quanji = "classic:quanji";
    private const string Activation = "escalating-fire-walk";
    private const string Owner = "fixture:fencheng-stock-owner", First = "fixture:fencheng-stock-first", Second = "fixture:fencheng-stock-second";
    private const string Deck = "fixture:fencheng-stock-deck", DiscardReason = "skill-program." + Fencheng + ".EscalatingDiscardOrDamage";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

    public static void BoughtThreeSkillsEscalateShopPricesAndFenchengUsesNativeNpcDiscardAndFire()
    {
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var rewards = new[] { "build-skill-3205", "build-skill-3080", "build-skill-3184" }
            .Select(id => current.Rewards.Single(reward => reward.Id == id)).ToArray();
        Need(rewards.Select(reward => reward.SkillId).SequenceEqual([Fencheng, Jianxiong, Quanji]),
            "The three actual current-season sources grant their exact ordinary Fencheng, OL Jianxiong and ordinary Quanji identities.");
        var definition = new ShanheCampaignDefinition("fixture:shanhe-fencheng-stock", "真实三技能购买与焚城", [Owner], ["shop"],
        [
            new("shop", "技能集市", ShanheNodeKind.Shop, ["battle"], Choices: rewards.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                    GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[reward.Id].Price,
                    RewardId: reward.Id, LeavesNode: false)).ToArray()),
            new("battle", "焚城递增弃牌实战", ShanheNodeKind.Battle, [],
                [new("guard", "两名实际一牌敌军",
                    [new(First) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] },
                     new(Second) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 3)] }])])
        ], rewards, StartingGold: 1100, ExtraSkillSlots: 3);
        var campaign = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UseCodexRewardPool = true, UnlockedRewardIds = rewards.Select(reward => reward.Id).ToArray() }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        Need(campaign.Snapshot.ShopItems.Count == 3 && campaign.Snapshot.ShopItems.All(item => item.CanPurchase),
            "The real codex random merchant publishes all three available skill sources from its exact three-item pool.");
        foreach (var (reward, price) in rewards.Zip(new[] { 100, 700, 300 }))
        {
            var item = campaign.Snapshot.ShopItems.Single(item => item.Reward?.Id == reward.Id);
            var gold = campaign.Snapshot.Gold;
            var revision = campaign.Revision;
            Need(item is { State: ShanheShopItemState.Available, RequiresSkillReplacement: false } && item.GoldCost == price,
                "The actual purchase sequence applies the skill category surcharge: Fencheng 100, Jianxiong 700 and Quanji 300 copper.");
            Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, revision));
            Need(campaign.Revision == revision + 1 && campaign.Snapshot.Gold == gold - price,
                "Each real merchant command commits one purchase and deducts its currently displayed escalating price once.");
        }
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.ExtraSkillSlots == 3 &&
            campaign.Snapshot.ExtraSkillIds.SequenceEqual([Fencheng, Jianxiong, Quanji]) && campaign.Snapshot.TacticIds.Count == 0,
            "The eleven-hundred-copper wallet buys exactly three ordinary skills into three existing skill slots without adding capacity or tactics.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        var s = Scene.Start(campaign.PendingBattleSetup!);
        s.Play();
        var players = s.Game.CreateSnapshot(0).Players;
        Need(players.Count == 3 && players.Count(player => player.IsHuman) == 1 && players[0].GeneralId == Owner &&
            new[] { Fencheng, Jianxiong, Quanji }.All(id => players[0].Skills!.Any(skill => skill.ContentId == id)) &&
            players.Skip(1).All(player => !player.IsHuman && player.TeamId != players[0].TeamId && player.HandCount == 1 && player.Equipment.Count == 0),
            "The actual three-seat Shanhe battle grants all bought programs to the unrelated printed general and gives each opposing native AI exactly one real hand card with no equipment.");
        var firstCard = s.View(1).Hand.Single().Id;
        var secondCard = s.View(2).Hand.Single().Id;
        var initialOwnerHp = s.View().Hp;
        var firstHp = s.View(1).Hp;
        var secondHp = s.View(2).Hp;
        var action = s.Game.GetHumanLegalActions().Single(item => item.ProgramSkillId == Fencheng);
        Need(action.ProgramActivationId == Activation && action.TargetSeats.Count == 0,
            "The purchased ordinary limited skill exposes its native fixed-clockwise activation without a selected starting target.");
        s.Fire();
        s.Reach(() => s.AtFireNpc(1));
        var firstFrame = s.FireFrame ?? throw new InvalidOperationException("The native first NPC decision lost its purchased Fencheng frame.");
        var fireFrame = firstFrame.Id;
        Need(firstFrame.DiscardChallenge is { Mode: "escalating", Cursor: 0, PreviousCount: 0, SelectedIds.Count: 0 } &&
            s.Prompt!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "card" && choice.Cards.SequenceEqual([firstCard])),
            "The native walk first publishes seat one's original one-card payment with a one-card minimum, before any damage or manual NPC answer.");

        s.Reach(() => s.AtFireNpc(2));
        var paid = s.Game.CardMovements.Where(move => move.Reason.Value == DiscardReason).ToArray();
        var secondPrompt = s.Prompt!;
        Need(s.FireFrame is { Id: var continuedFrameId, DiscardChallenge: { Cursor: 1, PreviousCount: 1, SelectedIds.Count: 0 } } && continuedFrameId == fireFrame &&
            paid is [{ CardId: var paidId, From: var from, To: var to }] && paidId == firstCard && from == CardLocation.Hand(1) && to == CardLocation.DiscardPile &&
            s.View(1).HandCount == 0 && s.View(1).Hp == firstHp && s.View(2).HandCount == 1 && s.Events<DamageAppliedEvent>().Length == 0 &&
            !secondPrompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "finish") &&
            secondPrompt.Choices.Count(choice => choice.Parameters.GetValueOrDefault("branch") == "card") == 1,
            "The first real AI selects and discards its one existing entity; the same original walk then reaches seat two with PreviousCount=1, requiring at least two while that AI owns only one card.");
        s.Play();
        var damage = s.Events<DamageAppliedEvent>();
        var resolved = s.Events<ProgramSkillResolvedEvent>().Where(item => item.SkillId == Fencheng).ToArray();
        var discardEvent = s.Game.Events.Single(item => item.Payload is CardMovedEvent moved && moved.CardId == firstCard && moved.Reason.Value == DiscardReason);
        var damageEvent = s.Game.Events.Single(item => item.Payload is DamageAppliedEvent);
        Need(damage is [{ SourceSeat: 0, TargetSeat: 2, Amount: 2, Nature: DamageNature.Fire }] &&
            s.View().Hp == initialOwnerHp && s.View(1).Hp == firstHp && s.View(2).Hp == secondHp - 2 &&
            s.View(2).Hand.Single().Id == secondCard && s.View(2).Equipment.Count == 0 &&
            s.Game.CardMovements.Count(move => move.CardId == firstCard && move.From == CardLocation.Hand(1) && move.To == CardLocation.DiscardPile) == 1 &&
            !s.Game.CardMovements.Any(move => move.CardId == secondCard && move.From == CardLocation.Hand(2)) &&
            discardEvent.Sequence < damageEvent.Sequence && resolved is [{ OwnerSeat: 0, ActivationId: Activation, Completed: true }] &&
            resolved[0].FrameId == fireFrame && s.FireFrame is null && !s.Game.GetHumanLegalActions().Any(item => item.ProgramSkillId == Fencheng),
            "The second ordinary AI cannot exceed the previous one-card discard and therefore receives the owner's real two fire damage after seat one's unique payment, keeps its unpaid card, and completes the original limited activation exactly once.");

        var beforeRetry = (s.Game.Revision, Events: s.Game.Events.Count, Moves: s.Game.CardMovements.Count);
        var retry = s.Game.Submit(new UseProgramSkillCommand(0, Fencheng, Activation, [], [], s.Game.Revision, s.Prompt!.PromptId));
        Need(!retry.Accepted && (s.Game.Revision, s.Game.Events.Count, s.Game.CardMovements.Count) == beforeRetry &&
            s.Events<ProgramSkillResolvedEvent>().Count(item => item.SkillId == Fencheng) == 1 && s.Events<DamageAppliedEvent>().Length == 1,
            "A second real command cannot reactivate the consumed limited skill, repeat a discard, or issue another fire hit.");
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected actual Fencheng stock purchase."); }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Enumerable.Range(0, 3).Select(seat => Game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        public ProgramSkillFrame? FireFrame => Game.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.SkillId == Fencheng);
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public bool AtFireNpc(int seat) => Prompt is { Kind: DecisionKind.ProgramTrigger } prompt && prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == Fencheng;
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Fencheng stock command."); }
        public void Fire() => Accept(new UseProgramSkillCommand(0, Fencheng, Activation, [], [], Game.Revision, Prompt!.PromptId));
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "Unexpected human prompt in the bounded native Fencheng scene: " + JsonSerializer.Serialize(Prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded native Fencheng boundary: " + JsonSerializer.Serialize(new { Prompt, FireFrame, Damage = Events<DamageAppliedEvent>() }));
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-fencheng-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            // Cao Ying's registered Lingren also requires its native linked death skill, Xingshang.
            CharacterFixtureContent.Register(builder, "classic:li-ru", "ol:cao-ying", "classic:cao-pi", "classic:zhong-hui");
            builder.AddGeneral(new(Owner, "购入焚城的普通武将", "supporter", "standard:none", "shu", 12));
            builder.AddGeneral(new(First, "仅有一牌的前席敌军", "supporter", "standard:none", "wei", 6));
            builder.AddGeneral(new(Second, "仅有一牌的后席敌军", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定闪牌堆与零额外摸牌", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index => new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "三技能购买后焚城实战", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, First, Second], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
