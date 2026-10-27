using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheAlliedTrickChecks
{
    private const string Owner = "fixture:allied-trick-owner", Ally = "fixture:allied-trick-ally", Peer = "fixture:allied-trick-peer";
    private const string Bonus = "fixture:allied-trick-recovery-bonus", Deck = "fixture:allied-trick-deck";
    private const string Identity = "fixture:allied-trick-identity", IdentityOwner = "fixture:allied-trick-identity-owner";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheAlliedTricksPackage(), new Fixture()));

    public static void PaidFengshounianSkipsIssuedEnemyPickerWithoutReducingReveal()
    {
        var s = Scene.Start();
        var baseline = s.Use(CardKind.FiveGrains);
        var baseId = s.UseId(baseline);
        Need(s.Events<HarvestCardSelectedEvent>().Where(e => e.ResolutionId == baseId)
                .Select(e => e.PlayerSeat).SequenceEqual([0, 1, 2, 3]),
            "Registering the future product leaves the unowned real Five Grains available to every original target.");
        s.AssertPaid(baseline);

        var card = s.Begin(CardKind.FiveGrains);
        s.Until(() => s.P is { Kind: DecisionKind.SelectHarvestCard, PlayerSeat: 2 });
        var prompt = s.P!.PromptId;
        var id = s.UseId(card);
        Need(s.Events<HarvestCardSelectedEvent>().Where(e => e.ResolutionId == id)
                .Select(e => e.PlayerSeat).SequenceEqual([0, 1]),
            "The unowned native draft really reaches the first enemy after both friendly players have chosen.");
        s.Buy(StandardShanheCampaignCatalog.CurrentFengshounianRewardId);
        s.Play();
        var revealed = s.Events<CardsRevealedEvent>().Single(e => e.ResolutionId == id).Cards.Select(c => c.Id).ToArray();
        var selected = s.Events<HarvestCardSelectedEvent>().Where(e => e.ResolutionId == id).ToArray();
        Need(revealed.Length == 4 && s.Events<GroupCardUsedEvent>().Single(e => e.ResolutionId == id)
                .TargetSeats.SequenceEqual([0, 1, 2, 3]) &&
            selected.Select(e => e.PlayerSeat).SequenceEqual([0, 1]) && s.P!.PromptId != prompt &&
            revealed.Except(selected.Select(e => e.CardId)).All(leftover => s.G.CardMovements.Count(move =>
                move.CardId == leftover && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile &&
                move.Reason == CardMoveReasons.HarvestDiscard) == 1),
            "The live paid grant retires the enemy's issued choice on the original frame, retains four targets and reveals, and discards the two remaining real cards once.");
        s.AssertPaid(card);
    }

    public static void PaidJiemengDoublesAlliedRequestBeforeNativeCapsAndRecoveryBonuses()
    {
        var s = Scene.Start(wounded: true);
        var first = s.Use(CardKind.PeachGarden);
        Need(s.Events<RecoveryAppliedEvent>().Select(e => (e.TargetSeat, e.Amount))
                .SequenceEqual([(0, 1), (1, 2), (2, 1), (3, 1)]) &&
            s.G.CreateSnapshot(0).Players.Select(p => p.Hp).SequenceEqual([2, 3, 2, 2]),
            "Before purchase ordinary Peach Garden heals one; the ally's already owned recovery bonus remains its separate native layer.");
        s.AssertPaid(first);
        s.Buy(StandardShanheCampaignCatalog.CurrentJiemengRewardId);
        var before = s.Events<RecoveryAppliedEvent>().Length;
        var second = s.Use(CardKind.PeachGarden);
        Need(s.Events<RecoveryAppliedEvent>().Skip(before).Select(e => (e.TargetSeat, e.Amount))
                .SequenceEqual([(0, 2), (1, 3), (2, 1), (3, 1)]) &&
            s.G.CreateSnapshot(0).Players.Select(p => p.Hp).SequenceEqual([4, 6, 3, 3]),
            "The real paid tactic doubles only self and the public teammate's base request; enemy healing stays one and the teammate's native plus-one still applies once.");
        s.AssertPaid(second);
        before = s.Events<RecoveryAppliedEvent>().Length;
        var third = s.Use(CardKind.PeachGarden);
        Need(s.Events<RecoveryAppliedEvent>().Skip(before).Select(e => (e.TargetSeat, e.Amount))
                .SequenceEqual([(0, 1), (1, 2), (2, 1), (3, 1)]) &&
            s.G.CreateSnapshot(0).Players.Select(p => p.Hp).SequenceEqual([5, 8, 4, 4]),
            "Both doubled requests and the ally's extra recovery obey each actual maximum HP without inflating the applied recovery facts.");
        s.AssertPaid(third);
    }

    public static void OrdinaryIdentityDoesNotInferAlliesFromHiddenRolesOrSharedFaction()
    {
        var s = Scene.StartIdentity();
        var visible = s.G.CreateSnapshot(0).Players;
        var currentPool = Registry.Value.Modes[Identity].GeneralPoolIds!;
        var publicDetails = "Allied identity public views: " + string.Join("; ", visible.Select(player =>
            $"seat={player.Seat}, general={player.GeneralId}, role={player.Role?.ToString() ?? "hidden"}, " +
            $"FactionId={player.FactionId ?? "null"}, IsTeamRevealed={player.IsTeamRevealed}, TeamId={player.TeamId ?? "null"}, " +
            $"printedFaction={Registry.Value.Generals[player.GeneralId].FactionId ?? "null"}")) +
            "; CurrentPool=" + string.Join(",", currentPool);
        var loyalist = Enumerable.Range(1, visible.Count - 1)
            .Single(seat => s.G.CreateSnapshot(seat).Players[seat].Role == Role.Loyalist);
        Need(visible.Skip(1).All(player => !player.IsTeamRevealed && player.TeamId is null &&
                player.Role is null && !player.IsRoleRevealed) &&
            visible[0].Role == Role.Lord && visible[loyalist].Role is null &&
            visible.All(player => player.FactionId is null && currentPool.Contains(player.GeneralId, StringComparer.Ordinal) &&
                Registry.Value.Generals[player.GeneralId].FactionId == "shu"),
            "The real hidden Loyalist and every other non-Lord role stay private, and no public team or faction declaration is inferred from the generals' matching printed Shu faction. " + publicDetails);
        var card = s.Use(CardKind.FiveGrains);
        var id = s.UseId(card);
        Need(s.Events<CardsRevealedEvent>().Single(e => e.ResolutionId == id).Cards.Count == 4 &&
            s.Events<HarvestCardSelectedEvent>().Where(e => e.ResolutionId == id).Select(e => e.PlayerSeat).SequenceEqual([0]) &&
            s.G.CardMovements.Count(move => move.Reason == CardMoveReasons.HarvestDiscard) == 3,
            "A printed owned Fengshounian in ordinary identity play recognizes self only; the hidden Loyalist and matching factions do not become public allies.");
        s.AssertPaid(card);
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public long UseId(int card) => Events<CardUseDeclaredEvent>().Single(e => e.CardId == card).ResolutionId;
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native allied-trick command rejected."); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0, Kind: DecisionKind.SelectHarvestCard } harvest)
                    Accept(new AnswerPromptCommand(0, harvest.PromptId, harvest.Choices[0].Id, G.Revision));
                else
                {
                    Need(P is not { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "Unexpected earlier allied-trick Play boundary.");
                    Accept(new AdvanceOneStepCommand(G.Revision));
                }
            }
            throw new InvalidOperationException($"Native allied-trick flow did not reach its boundary; pending={P?.Kind}, seat={P?.PlayerSeat}.");
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && G.ResolutionStack.Count == 0);
        public int Begin(CardKind kind)
        {
            var card = G.CreateSnapshot(0).Players[0].Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().Single(a => a.CardId == card.Id && a.ConversionSource is null);
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId)); return card.Id;
        }
        public int Use(CardKind kind) { var card = Begin(kind); Play(); return card; }
        public void Buy(string reward)
        {
            var market = G.CreateShanheMarketSnapshot(0)!;
            var item = market.ShopItems.Single(i => i.Reward?.Id == reward);
            Need(item.GoldCost == 200, "The published normal tactic really costs 200 coins before any purchase surcharge.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision));
            Until(() => !G.HasPendingShanheMarketPurchase);
            Need(G.CreateShanheMarketSnapshot(0)!.Gold == market.Gold - 200 &&
                Events<ShanheMarketPurchaseCommittedEvent>().Single() is { Price: 200 } &&
                Events<ShanheMarketPurchaseAppliedEvent>().Single().RewardId == reward,
                "A real live purchase pays exactly once and completes its original grant before the suspended card resumes.");
        }
        public void AssertPaid(int card)
        {
            var id = UseId(card);
            Need(G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == id) == 1,
                "The one original group trick pays its real material and finishes exactly once across all recipients.");
        }
        public static Scene Start(bool wounded = false)
        {
            ShanheRewardDefinition[] rewards =
            [new(StandardShanheCampaignCatalog.CurrentFengshounianRewardId, "丰收年")
                { TacticSkillId = StandardShanheAlliedTricksPackage.FengshounianSkillId, Rarity = 1 },
             new(StandardShanheCampaignCatalog.CurrentJiemengRewardId, "结盟")
                { TacticSkillId = StandardShanheAlliedTricksPackage.JiemengSkillId, Rarity = 1 },
             new("cards", "真实群体锦囊", StartingCards:
                [new(CardKind.FiveGrains, Suit.Heart, 1), new(CardKind.FiveGrains, Suit.Club, 2),
                 new(CardKind.PeachGarden, Suit.Heart, 3), new(CardKind.PeachGarden, Suit.Heart, 4), new(CardKind.PeachGarden, Suit.Heart, 5)])];
            var definition = new ShanheCampaignDefinition("fixture:allied-tricks", "公开敌友群体锦囊", [Owner], ["cards"],
                [new("cards", "真实补给", ShanheNodeKind.Event, ["shop", "battle"], Choices: [new("take", "领取", RewardId: "cards")]),
                 new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("harvest", "丰收年", 200, RewardId: rewards[0].Id, LeavesNode: false),
                     new("recover", "结盟", 200, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "公开敌友", ShanheNodeKind.Battle, [],
                    [new("peers", "公开敌友", [new(Peer, Hp: wounded ? 1 : 5) { StartingHand = [] },
                        new(Peer, Hp: wounded ? 1 : 5) { StartingHand = [] }],
                        Allies: [new(Ally, Hp: wounded ? 1 : 8) { StartingHand = [] }])])], rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(31, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true,
                    UnlockedRewardIds = [rewards[0].Id, rewards[1].Id] }, _ => Registry.Value);
            foreach (var command in new Func<long, ShanheCampaignCommand>[]
                { r => new MoveShanheNodeCommand("cards", r), r => new ChooseShanheNodeOptionCommand("take", null, r),
                  r => new MoveShanheNodeCommand("battle", r) })
            { var result = session.Submit(command(session.Revision)); Need(result.Accepted, result.Error ?? "Cannot enter allied-trick fixture."); }
            var setup = session.PendingBattleSetup! with { Hp = wounded ? 1 : 5 };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
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
            Need(game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }, "The identity table must offer its native general selection.");
            s.Accept(new SelectGeneralCommand(0, IdentityOwner, game.Revision, game.PendingDecision!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:allied-tricks", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var bonus = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Bonus}}","revision":1,"cardPolicies":[
                    {"id":"real-recovery-bonus","kind":"ownRecoveryAmountBonus","value":1}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{Bonus}}":{"name":"回复量检查","description":"每次真实回复量加一。"} } }
                """);
            builder.AddSkill(new(Bonus, "回复量检查", "每次真实回复量加一。")
                { Program = bonus.Programs[Bonus], ProgramPresentation = bonus.Presentations[Bonus] });
            builder.AddGeneral(new(Owner, "锦囊使用者", "supporter", "standard:none", "shu", 5));
            builder.AddGeneral(new(Ally, "公开同行", "supporter", Bonus, "shu", 8));
            builder.AddGeneral(new(Peer, "公开敌军", "supporter", "standard:none", "wei", 5));
            builder.AddGeneral(new(IdentityOwner, "身份场持有者", "supporter", StandardShanheAlliedTricksPackage.FengshounianSkillId,
                "shu", 5, [StandardShanheAlliedTricksPackage.JiemengSkillId]));
            for (var index = 1; index <= 3; index++) builder.AddGeneral(new(Peer + index, "同势力隐藏身份", "supporter", "standard:none", "shu", 5));
            builder.AddDeck(new(Deck, "无自然摸牌", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddDeck(new(Deck + "-identity", "真实五谷实体", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:five_grains", Suit.Heart, 3)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "公开敌友锦囊", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
            builder.AddMode(new(Identity, "普通隐藏身份", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck + "-identity",
                GeneralCandidateCount: 4, GeneralPoolIds: [IdentityOwner, Peer + "1", Peer + "2", Peer + "3"]));
        }
    }
}
