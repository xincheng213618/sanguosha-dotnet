using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHeartBladeChecks
{
    private const string Owner = "fixture:heart-blade-owner", Peer = "fixture:heart-blade-peer";
    private const string Driver = "fixture:heart-blade-driver", Deck = "fixture:heart-blade-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheHeartBladePackage(), new Fixture()));

    public static void LivePurchaseUsesFrozenNativeSuitAndChainInheritsOnce()
    {
        var s = Scene.Start();
        s.Card(CardKind.Slash, Suit.Heart, 8, [1], false, 1);
        Need(s.Events<CardUseAppearanceCapturedEvent>().Any(e => e.Action.ActorSeat == 0 &&
            e.Action.EffectiveKind == CardKind.Slash && e.Action.EffectiveSuit == Suit.Heart),
            "Registering future stock captures exact original suit before ownership, without adding damage.");
        s.Buy();
        Need(s.Market().Gold == 300 && s.Events<ShanheMarketPurchaseCommittedEvent>().Length == 1 &&
            s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 1 &&
            s.View().Skills!.Count(skill => skill.ContentId == StandardShanheHeartBladePackage.SkillId) == 1,
            "The genuine 700-coin purchase immediately grants one Heart Blade to this running battle.");
        s.Card(CardKind.Slash, Suit.Heart, 13, [1], false, 3);
        s.Card(CardKind.Slash, Suit.Spade, 7, [1], false, 3);
        s.Card(CardKind.Slash, Suit.Diamond, 9, [1], false, 1);
        s.Card(CardKind.Peach, Suit.Heart, 3, [1], true, 3);
        s.Card(CardKind.Peach, Suit.Diamond, 4, [1], true, 1);
        s.Card(CardKind.Duel, Suit.Heart, 5, [1], false, 1);
        s.Card(CardKind.ThunderSlash, Suit.Heart, 11, [1], false, 3);
        s.Card(CardKind.ColdSlash, Suit.Heart, 12, [1], false, 3);
        s.Card(CardKind.IronChain, Suit.Spade, 6, [1, 2], false, null);
        s.Card(CardKind.FireSlash, Suit.Heart, 10, [1], false, 3, [1, 2]);
    }

    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        private GameEngine G { get; } = game;
        public PlayerSnapshot View(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ShanheCampaignSnapshot Market() => G.CreateShanheMarketSnapshot(0)!;
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Heart Blade command."); }
        private void ReachPlay()
        {
            for (var step = 0; step < 180; step++)
            {
                if (G.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(G.State.Winner == Winner.None, "The bounded Heart Blade scene ended before its expected boundary.");
                if (G.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var cold = prompt.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("program-action") ==
                        "native-cold-damage" && c.Parameters.GetValueOrDefault("action") == "cold-damage");
                    Need(cold is not null, "Unexpected human choice in the bounded Heart Blade scene.");
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, cold!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded Heart Blade native Play boundary.");
        }
        public void Buy()
        {
            var item = Market().ShopItems.Single(i => i.Reward?.Id == StandardShanheCampaignCatalog.CurrentHeartBladeRewardId);
            Accept(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision)); ReachPlay();
        }
        public void Card(CardKind materialKind, Suit suit, int rank, int[] targets, bool converted,
            int? expectedDamage, int[]? damageTargets = null)
        {
            var card = View().Hand.Single(c => c.Kind == materialKind && c.Suit == suit && c.Rank == rank);
            var label = $"{materialKind}/{suit}/{rank} cardId={card.Id}";
            var action = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual(targets) &&
                (converted ? a.ConversionSource?.SkillId == Driver : a.ConversionSource is null)) ??
                throw new InvalidOperationException("Missing native Heart Blade action: " + label);
            if (expectedDamage is { } expected)
            {
                var revision = G.Revision; var eventCount = G.Events.Count; var movements = G.CardMovements.Count;
                for (var query = 0; query < 3; query++)
                    Need(G.PredictPublicActionDamage(0, action, targets[0]).DamageAmount == expected,
                        "Public proposed damage must match the real effective-suit bonus without consuming anything.");
                Need(G.Revision == revision && G.Events.Count == eventCount && G.CardMovements.Count == movements,
                    "Repeated Heart Blade prediction must leave the native command state and entities unchanged.");
            }
            var before = Events<DamageAppliedEvent>().Length;
            Accept(new PlayCardCommand(0, card.Id, targets, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
            ReachPlay();
            var damage = Events<DamageAppliedEvent>().Skip(before).ToArray();
            var declarations = Events<CardUseDeclaredEvent>().Where(e => e.CardId == card.Id).ToArray();
            // NativeActionAppearance records every native use. Accepted events
            // depend on finalized-window observers and need not exist for tricks.
            var appearances = Events<CardUseAppearanceCapturedEvent>().Select(e => e.Action).Where(a =>
                a.Type == CardActionType.Use && a.ActorSeat == 0 && a.PhysicalCards.Any(c => c.CardId == card.Id)).ToArray();
            Need(declarations.Length == 1 && appearances.Length == 1,
                $"Expected one native declaration/appearance for {label}; actual {declarations.Length}/{appearances.Length}.");
            var declared = declarations[0];
            var appearance = appearances[0];
            var effectiveSuit = suit == Suit.Spade ? Suit.Heart : suit;
            Need(appearance.ActorSeat == declared.SourceSeat && appearance.EffectiveKind == declared.CardKind &&
                appearance.EffectiveSuit == effectiveSuit && appearance.ConversionChain.Count == (converted ? 1 : 0) &&
                G.CardMovements.Count(m => m.CardId == card.Id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == card.Id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId) == 1,
                "The real card preserves its original effective suit and conversion, moves and finishes exactly once: " + label);
            if (expectedDamage is { } amount)
            {
                var recipients = damageTargets ?? targets;
                Need(damage.Select(e => e.TargetSeat).Order().SequenceEqual(recipients.Order()) &&
                    damage.All(e => e.SourceSeat == 0 && e.Amount == amount) &&
                    damage.Single(e => e.TargetSeat == targets[0]).SourceCardUseResolutionId == declared.ResolutionId,
                    "Only Heart Slash gains two card damage; other suits and Heart Duel keep one, and chain inherits three once. Actual: " +
                    JsonSerializer.Serialize(damage));
            }
            else Need(damage.Length == 0, "Iron Chain must prepare the real propagation without dealing damage itself.");
        }
        public static Scene Start()
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentHeartBladeRewardId, "关公刃")
                    { TacticSkillId = StandardShanheHeartBladePackage.SkillId, Rarity = 4 },
                new("supplies", "实体花色试牌", StartingCards:
                    [new(CardKind.Slash, Suit.Heart, 8), new(CardKind.Slash, Suit.Heart, 13), new(CardKind.Slash, Suit.Spade, 7),
                     new(CardKind.Slash, Suit.Diamond, 9),
                     new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Peach, Suit.Diamond, 4), new(CardKind.Duel, Suit.Heart, 5),
                     new(CardKind.FireSlash, Suit.Heart, 10), new(CardKind.ThunderSlash, Suit.Heart, 11),
                     new(CardKind.ColdSlash, Suit.Heart, 12), new(CardKind.IronChain, Suit.Spade, 6)])
            ];
            var definition = new ShanheCampaignDefinition("fixture:heart-blade", "红桃杀实战", [Owner], ["supplies"],
                [new("supplies", "实体补给", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take", "领取实体补给", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy", "关公刃", 700, RewardId: rewards[0].Id, LeavesNode: false)]),
                 new("battle", "花色小局", ShanheNodeKind.Battle, [],
                    [new("guard", "固定守军", Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Peer)
                        { StartingHand = [] }).ToArray())])], rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(23, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var create in new Func<long, ShanheCampaignCommand>[]
                     { revision => new MoveShanheNodeCommand("supplies", revision),
                       revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                       revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = session.Submit(create(session.Revision)); Need(result.Accepted, result.Error ?? "Heart Blade scene failed to enter its real battle."); }
            var setup = session.PendingBattleSetup!;
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.ReachPlay(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:heart-blade", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"quota","query":"slashLimit","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "cardPolicies":[{"id":"spade-is-heart","kind":"rewriteSuit","inputSuit":"spade","outputSuit":"heart"}],
                   "viewAs":[{"id":"peach-slash","inputKinds":["peach"],"inputSuits":[],
                     "sourceZones":["hand"],"outputKind":"slash","forPlay":true,"forResponse":true,"useEffectiveInputSuit":true}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实体转杀驱动", description = "花色实战试牌。" } } }));
            builder.AddSkill(new(Driver, "实体转杀驱动", "花色实战试牌。")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "红桃杀使用者", "supporter", Driver, "shu", 30));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", "standard:none", "wei", 30));
            builder.AddCard(new("fixture:heart-blade-ice-slash", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            builder.AddDeck(new(Deck, "无普通摸牌实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "红桃杀实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
