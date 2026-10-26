using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGuandaoChecks
{
    private const string Owner = "fixture:guandao-owner", Convert = "fixture:guandao-convert", Deck = "fixture:guandao-deck";
    private static string Peer(int seat) => "fixture:guandao-peer-" + seat;
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheMarketPackage(), new StandardShanheGuandaoPackage(), new Fixture()));

    public static void PaidDiamondSlashRangeIncludesNativeElementsAndConvertedCards()
    {
        foreach (var (kind, converted) in new[]
                 { (CardKind.Slash, false), (CardKind.FireSlash, false), (CardKind.ThunderSlash, false),
                   (CardKind.ColdSlash, false), (CardKind.Slash, true) })
        {
            var s = Scene.Start(kind);
            var hand = s.View().Hand;
            var physical = hand.Single(c => c.Kind == kind && c.Suit == Suit.Diamond && c.Rank == 8).Id;
            var otherSuit = hand.Single(c => c.Kind == kind && c.Suit == Suit.Heart).Id;
            var peach = hand.Single(c => c.Kind == CardKind.Peach).Id;
            var snatch = hand.Single(c => c.Kind == CardKind.Snatch).Id;
            Need(!s.Actions().Any(a => a.TargetSeats.Contains(2) && (a.CardId == physical || a.CardId == peach)) &&
                s.Actions().Any(a => a.CardId == physical && a.TargetSeats.SequenceEqual([1])),
                "Registering unowned future stock leaves the real four-seat distance-two opponent outside ordinary Slash range.");
            s.Buy();
            var actions = s.Actions();
            Need(s.Market().Gold == 800 && s.Events<ShanheMarketPurchaseCommittedEvent>().Length == 1 &&
                s.Events<ShanheMarketPurchaseAppliedEvent>().Length == 1 &&
                s.View().Skills!.Count(skill => skill.ContentId == StandardShanheGuandaoPackage.SkillId) == 1 &&
                actions.Any(a => a.CardId == physical && a.ConversionSource is null && a.TargetSeats.SequenceEqual([2])) &&
                actions.Any(a => a.CardId == peach && a.ConversionSource?.SkillId == Convert && a.TargetSeats.SequenceEqual([2])) &&
                !actions.Any(a => (a.CardId == otherSuit || a.CardId == snatch) && a.TargetSeats.Contains(2)),
                "A genuine 200-coin purchase immediately extends only Diamond Slash, including the actual Diamond Peach conversion; Heart Slash and Diamond Snatch keep native range.");
            var paid = converted ? peach : physical;
            var action = actions.First(a => a.CardId == paid && a.TargetSeats.SequenceEqual([2]) &&
                (converted ? a.ConversionSource?.SkillId == Convert : a.ConversionSource is null));
            s.Play(action);
            var declared = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == paid);
            var accepted = s.Events<CardActionAcceptedEvent>().Select(e => e.Action).Single(a =>
                a.Type == CardActionType.Use && a.ActorSeat == 0 && a.PhysicalCards.Any(c => c.CardId == paid));
            var material = accepted.PhysicalCards.Single();
            var printedSuit = hand.Single(c => c.Id == material.CardId).Suit;
            // Native actions omit an appearance override when no appearance-tracking observer needs it.
            var effectiveSuit = accepted.EffectiveSuit ?? printedSuit;
            Need(s.View(2).Hp == 19 && declared.CardKind == kind &&
                material.CardId == paid && material.CardKind == (converted ? CardKind.Peach : kind) &&
                effectiveSuit == Suit.Diamond && accepted.EffectiveKind == kind &&
                accepted.ConversionChain.Count == (converted ? 1 : 0) &&
                (!converted || accepted.ConversionChain.Single().SkillId == Convert) &&
                s.Events<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 2, Amount: 1, SourceCardUseResolutionId: var original }] &&
                original == declared.ResolutionId &&
                s.G.CardMovements.Count(m => m.CardId == paid && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == paid && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId) == 1 &&
                !s.Actions().Any(a => a.Kind == LegalActionKind.Slash),
                "The distant real card pays once and deals native damage with its exact effective suit and conversion; range exemption never adds another Slash use. Actual: " +
                JsonSerializer.Serialize(new
                {
                    Kind = kind, Converted = converted, PaidCardId = paid, ActualHp = s.View(2).Hp,
                    PrintedSuit = printedSuit, ResolvedEffectiveSuit = effectiveSuit,
                    Declared = new { declared.ResolutionId, declared.CardId, declared.CardKind, declared.SourceSeat },
                    Accepted = new { accepted.ActionId, accepted.EffectiveKind, accepted.EffectiveSuit,
                        ConversionChain = accepted.ConversionChain.Select(c => new { c.SkillId, c.BindingId }).ToArray() },
                    Damage = s.Events<DamageAppliedEvent>().Select(e => new
                        { e.SourceSeat, e.TargetSeat, e.Amount, e.SourceCardUseResolutionId }).ToArray(),
                    IntoProcessing = s.G.CardMovements.Count(m => m.CardId == paid &&
                        m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing),
                    IntoDiscard = s.G.CardMovements.Count(m => m.CardId == paid &&
                        m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile),
                    FinishedUses = s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId),
                    RemainingSlashes = s.Actions().Where(a => a.Kind == LegalActionKind.Slash).Select(a => new
                        { a.CardId, a.PlayedCardKind, a.TargetSeats, Source = a.ConversionSource?.SkillId }).ToArray()
                }));
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PlayerSnapshot View(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public IReadOnlyList<LegalAction> Actions() => G.GetHumanLegalActions();
        public ShanheCampaignSnapshot Market() => G.CreateShanheMarketSnapshot(0)!;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real Guandao command."); }
        private void ReachPlay()
        {
            for (var step = 0; step < 120; step++)
            {
                if (G.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                if (G.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    var damage = prompt.Choices.SingleOrDefault(c =>
                        c.Parameters.GetValueOrDefault("program-action") == "native-cold-damage" &&
                        c.Parameters.GetValueOrDefault("action") == "cold-damage");
                    Need(damage is not null, "Unexpected human choice in the bounded Diamond Slash scene.");
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, damage!.Id, G.Revision));
                }
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Diamond Slash failed to return to the native Play boundary.");
        }
        public void Buy()
        {
            var item = Market().ShopItems.Single(i => i.Reward?.Id == StandardShanheCampaignCatalog.CurrentGuandaoRewardId);
            Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, G.Revision)); ReachPlay();
        }
        public void Play(LegalAction action)
        {
            Submit(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
            ReachPlay();
        }
        public static Scene Start(CardKind kind)
        {
            ShanheRewardDefinition[] rewards =
            [
                new(StandardShanheCampaignCatalog.CurrentGuandaoRewardId, "关刀之脊")
                    { TacticSkillId = StandardShanheGuandaoPackage.SkillId, Rarity = 1 },
                new("supplies", "实体距离试牌", StartingCards:
                    [new(kind, Suit.Diamond, 8), new(kind, Suit.Heart, 9), new(CardKind.Slash, Suit.Diamond, 12),
                     new(CardKind.Peach, Suit.Diamond, 3), new(CardKind.Snatch, Suit.Diamond, 4)])
            ];
            var definition = new ShanheCampaignDefinition("fixture:guandao", "方片杀实战", [Owner], ["supplies"],
                [new("supplies", "实体补给", ShanheNodeKind.Event, ["shop", "battle"],
                    Choices: [new("take", "领取实体补给", RewardId: "supplies")]),
                 new("shop", "常驻集市", ShanheNodeKind.Shop, ["battle"], Choices:
                    [new("buy", "关刀之脊", 200, RewardId: rewards[0].Id, LeavesNode: false)]),
                 new("battle", "四人距离小局", ShanheNodeKind.Battle, [],
                    [new("guard", "固定守军", Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition(Peer(seat))
                        { StartingHand = seat == 2 ? [new(CardKind.Peach, Suit.Heart, 5)] : [] }).ToArray())])],
                rewards, StartingGold: 1000);
            var session = ShanheCampaignSession.Create(definition, new(23, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => Registry.Value);
            foreach (var create in new Func<long, ShanheCampaignCommand>[]
                     { revision => new MoveShanheNodeCommand("supplies", revision),
                       revision => new ChooseShanheNodeOptionCommand("take", null, revision),
                       revision => new MoveShanheNodeCommand("battle", revision) })
            { var result = session.Submit(create(session.Revision)); Need(result.Accepted, result.Error ?? "Guandao scene failed to enter its real battle."); }
            var setup = session.PendingBattleSetup!;
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Submit(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-guandao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Convert}}","revision":1,
                  "viewAs":[{"id":"diamond-peach-slash","inputKinds":["peach"],"inputSuits":["diamond"],
                    "sourceZones":["hand"],"outputKind":"slash","forPlay":true,"forResponse":true,"useEffectiveInputSuit":true}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Convert] = new { name = "方片转杀", description = "实体转化试牌。" } } }));
            builder.AddSkill(new(Convert, "方片转杀", "实体转化试牌。")
                { Program = catalog.Programs[Convert], ProgramPresentation = catalog.Presentations[Convert] });
            builder.AddGeneral(new(Owner, "方片杀使用者", "supporter", Convert, "shu", 20));
            foreach (var seat in Enumerable.Range(1, 3))
                builder.AddGeneral(new(Peer(seat), "固定守军", "supporter", "standard:none", "wei", 20));
            builder.AddCard(new("fixture:guandao-ice-slash", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            builder.AddDeck(new(Deck, "无普通摸牌实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "关刀距离实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
