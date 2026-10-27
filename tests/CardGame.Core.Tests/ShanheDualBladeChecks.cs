using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDualBladeChecks
{
    private const string Owner = "fixture:dual-blade-owner", Peer = "fixture:dual-blade-peer";
    private const string Driver = "fixture:dual-blade-driver", Deck = "fixture:dual-blade-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheDualBladePackage(), new Fixture()));

    public static void DodgedUnownedSlashConsumesRoundBeforeLivePurchase()
    {
        var s = Scene.Start(firstDefenderDodge: true);
        var dodge = s.G.CreateSnapshot(1).Players[1].Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        s.Maximum(CardKind.Slash, 3, 1);
        s.Card(CardKind.Slash, 3, [1], false, 0, []);
        Need(s.G.CardMovements.Count(m => m.CardId == dodge && m.From == CardLocation.Hand(1) &&
                m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == dodge && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile) == 1,
            "A real defender pays its Dodge; a missed Slash is still this round's first actual Use.");
        s.Buy(StandardShanheCampaignCatalog.CurrentDualBlade2RewardId, 500);
        s.Maximum(CardKind.Peach, 2, 1);
        s.RejectExtraTargets(CardKind.Peach, 2, [1, 2, 3]);
        s.NextRound();
        s.Maximum(CardKind.Peach, 2, 3);
        s.Card(CardKind.Peach, 2, [1, 2, 3], true, 1, [StandardShanheDualBladePackage.UpgradedSkillId]);
        s.Maximum(CardKind.Slash, 4, 1);
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PlayerSnapshot View() => G.CreateSnapshot(0).Players[0];
        private ShanheCampaignSnapshot Market() => G.CreateShanheMarketSnapshot(0)!;
        private T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private CardSnapshot CardEntity(CardKind kind, int rank) => View().Hand.Single(c => c.Kind == kind && c.Rank == rank);
        private LegalAction[] Slashes(CardKind kind, int rank) => G.GetHumanLegalActions().Where(a =>
            a.Kind == LegalActionKind.Slash && a.CardId == CardEntity(kind, rank).Id).ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Dual Blade command."); }
        private void Play()
        {
            for (var step = 0; step < 220; step++)
            {
                if (G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(G.State.Winner == Winner.None, "The bounded Dual Blade scene ended before its expected Play boundary.");
                Need(G.PendingDecision is not { PlayerSeat: 0, Choices.Count: > 0 },
                    "Unexpected human choice in the deterministic Dual Blade scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded Dual Blade scene did not return to Play.");
        }
        public void Buy(string reward, int price)
        {
            var item = Market().ShopItems.Single(i => i.Reward?.Id == reward);
            var balance = Market().Gold;
            Need(item.GoldCost == price, "The real Dual Blade quote preserves its authored price and current category surcharge.");
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision)); Play();
            Need(Market().Gold == balance - price && Events<ShanheMarketPurchaseCommittedEvent>().Last().Price == price &&
                Events<ShanheMarketPurchaseAppliedEvent>().Length == Events<ShanheMarketPurchaseCommittedEvent>().Length,
                "The real market purchase pays once and immediately completes its original native grant.");
        }
        public void Maximum(CardKind kind, int rank, int expected)
        {
            var revision = G.Revision; var events = G.Events.Count; var moves = G.CardMovements.Count;
            for (var query = 0; query < 3; query++)
                Need(Slashes(kind, rank).Max(action => action.TargetSeats.Count) == expected,
                    $"{kind}/{rank} has exactly {expected} possible targets under its actual first-Slash quota.");
            Need(G.Revision == revision && G.Events.Count == events && G.CardMovements.Count == moves,
                "Repeated legal queries do not consume a Slash declaration or change its real entity.");
        }
        public void RejectExtraTargets(CardKind kind, int rank, int[] targets)
        {
            var material = CardEntity(kind, rank);
            var allowed = Slashes(kind, rank).First();
            var before = G.CardMovements.Count;
            var result = G.Submit(new PlayCardCommand(0, material.Id, targets, G.Revision, G.PendingDecision!.PromptId,
                allowed.PlayedCardKind, allowed.TargetCardId) { ConversionSource = allowed.ConversionSource,
                AdditionalConversionSources = allowed.AdditionalConversionSources });
            Need(!result.Accepted && G.CardMovements.Count == before && View().Hand.Any(c => c.Id == material.Id),
                "Buying after a real missed Slash never authorizes forged extra targets or pays the rejected entity.");
        }
        public void Card(CardKind kind, int rank, int[] targets, bool converted, int amount, string[] sourceIds)
        {
            var material = CardEntity(kind, rank);
            var action = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == material.Id &&
                a.TargetSeats.SequenceEqual(targets) && (converted ? a.ConversionSource?.SkillId == Driver : a.ConversionSource is null)) ??
                throw new InvalidOperationException($"Missing genuine Dual Blade {kind}/{rank} to {string.Join(',', targets)}.");
            var before = Events<DamageAppliedEvent>().Length;
            Accept(new PlayCardCommand(0, material.Id, targets, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource,
                AdditionalConversionSources = action.AdditionalConversionSources }); Play();
            var declared = Events<CardUseDeclaredEvent>().Single(e => e.CardId == material.Id);
            var damage = Events<DamageAppliedEvent>().Skip(before).ToArray();
            Need(amount == 0 ? damage.Length == 0 : damage.Select(d => d.TargetSeat).Order().SequenceEqual(targets.Order()) &&
                damage.All(d => d.Amount == amount && d.SourceSeat == 0 && d.SourceCardUseResolutionId == declared.ResolutionId),
                "One original native declaration resolves every chosen target exactly once: " + JsonSerializer.Serialize(damage));
            Need(G.CardMovements.Count(m => m.CardId == material.Id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == material.Id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId) == 1,
                "Multiple native targets preserve one actual material payment, discard and root completion.");
            if (sourceIds.Length > 0)
            {
                var receipt = Events<ProgramCardTargetCountAppliedEvent>().Single(e => e.ResolutionId == declared.ResolutionId);
                Need(receipt.TargetSeats.SequenceEqual(targets) && sourceIds.All(id => receipt.ContributionSourceIds.Any(source => source.Contains(id, StringComparison.Ordinal))),
                    "The original post-Begin native target receipt keeps each first-Slash source instead of recalculating a spent quota.");
            }
        }
        public void NextRound()
        {
            var round = Events<RoundStartedEvent>().Last().RoundNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, G.PendingDecision!.PromptId)); Play();
            Need(Events<RoundStartedEvent>().Last().RoundNumber == round + 1,
                "First-Slash targets refresh across the actual global round, not an owner-only turn counter.");
        }
        public static Scene Start(bool firstDefenderDodge = false)
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentDualBladeRewardId, "双刃")
                    { TacticSkillId = StandardShanheDualBladePackage.SkillId, Rarity = 2 },
                new(StandardShanheCampaignCatalog.CurrentDualBlade2RewardId, "双刃Ⅱ")
                    { TacticSkillId = StandardShanheDualBladePackage.UpgradedSkillId, Rarity = 3 },
                new("supplies", "实体试牌", StartingCards:
                    [new(CardKind.Duel, Suit.Heart, 1), new(CardKind.Peach, Suit.Heart, 2),
                     new(CardKind.Slash, Suit.Club, 3), new(CardKind.Slash, Suit.Spade, 4),
                     new(CardKind.FireSlash, Suit.Diamond, 5)])
            ];
            var definition = new ShanheCampaignDefinition("fixture:dual-blade", "双刃实战", [Owner], ["supplies"],
                [new("supplies", "实体补给", ShanheNodeKind.Event, ["shop", "battle"], Choices: [new("take", "领取", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy-one", "双刃", 300, RewardId: rewards[0].Id, LeavesNode: false),
                     new("buy-two", "双刃Ⅱ", 500, RewardId: rewards[1].Id, LeavesNode: false)]),
                 new("battle", "固定守军", ShanheNodeKind.Battle, [],
                    [new("guard", "守军", Enumerable.Range(1, 4).Select(seat => new ShanheCombatantDefinition(Peer)
                        { StartingHand = firstDefenderDodge && seat == 1 ? [new(CardKind.Dodge, Suit.Club, 7)] : [] }).ToArray())])],
                rewards, StartingGold: 1200);
            var session = ShanheCampaignSession.Create(definition, new(27, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var create in new Func<long, ShanheCampaignCommand>[]
                     { revision => new MoveShanheNodeCommand("supplies", revision),
                       revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                       revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = session.Submit(create(session.Revision)); Need(result.Accepted, result.Error ?? "Dual Blade failed to enter its genuine battle."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 16, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:dual-blade", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"quota","query":"slashLimit","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "viewAs":[{"id":"peach-slash","inputKinds":["peach"],"inputSuits":[],"sourceZones":["hand"],
                     "outputKind":"slash","forPlay":true,"forResponse":true,"useEffectiveInputSuit":true}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实体转杀", description = "双刃试牌。" } } }));
            builder.AddSkill(new(Driver, "实体转杀", "双刃试牌。")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "双刃使用者", "supporter", Driver, "shu", 30));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", "standard:none", "wei", 30));
            builder.AddDeck(new(Deck, "零普通摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "双刃实战", 5, 5, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 4 }));
        }
    }
}
