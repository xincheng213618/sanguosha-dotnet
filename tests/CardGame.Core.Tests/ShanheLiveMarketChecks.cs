using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLiveMarketChecks
{
    private const string Owner = "fixture:live-market-owner", Peer = "fixture:live-market-peer";
    private const string Driver = "fixture:live-market-driver", Extra = "fixture:live-market-extra", Quiet = "fixture:live-market-quiet";
    private const string Deck = "fixture:live-market-deck";

    public static void EntryWealthQualificationSurvivesPurchasesAndRefreshes()
    {
        var scene = Scene.Start();
        scene.Buy("wealth");
        Need(scene.Market().Gold == 2000 && scene.Player().Skills!.Count(skill =>
                skill.ContentId == StandardShanheWealthPackage.WealthHandLimitSkillId) == 1,
            "The new entry-wealth tactic uses this battle's frozen 3000 entry coins after its actual price lowers the wallet to the 2000 threshold.");
        scene.Submit(new CardGame.Core.RefreshShanheMarketCommand(0, scene.Game.Revision));
        scene.Buy("slash");
        Need(scene.Market().Gold == 1800 && scene.Player().Skills!.Count(skill =>
                skill.ContentId == StandardShanheWealthPackage.WealthHandLimitSkillId) == 1 &&
            scene.Facts<ShanheMarketPurchaseCommittedEvent>().Length == 2,
            "A paid refresh and another purchase preserve entry qualification without treating the current wallet as entry money.");
    }

    public static void PaidSkillTacticAndCardsApplyNowAndKeepTheOriginalPrompt()
    {
        var scene = Scene.Start();
        var before = scene.Market(); var revision = scene.Game.Revision;
        Need(!scene.Game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, "forged-offer", null, revision)).Accepted &&
            scene.Game.Revision == revision && scene.Market().Gold == before.Gold && scene.Market().TacticIds.Count == 0,
            "An unknown live offer cannot change the game journal or spend its detached market wallet.");
        scene.Buy("blood");
        Need(scene.Player() is { MaxHp: 2, Hp: 2 } &&
            scene.Facts<MaximumHpChangedEvent>() is [{ PlayerSeat: 0, Delta: -2, MaximumHp: 2 }] &&
            scene.Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
            "Buying the real blood tactic immediately runs only its new native opening cost and returns to this same battle's Play decision.");
        scene.Buy("rage"); scene.Buy("skill"); scene.Buy("slash");
        Need(scene.Market().Gold == 2500 && scene.Market().ExtraSkillIds.SequenceEqual([Extra]) &&
            scene.Facts<ShanheMarketPurchaseCommittedEvent>().Length == 4 &&
            scene.Facts<ShanheMarketPurchaseAppliedEvent>().Length == 4,
            "The live ledger pays four real offers and one same-tactic surcharge, granting one ordinary skill and the two independent battle tactics.");
        var hit = scene.Facts<DamageAppliedEvent>().Length;
        scene.Submit(new UseProgramSkillCommand(0, Extra, "ping", [], [1], scene.Game.Revision, scene.Game.PendingDecision!.PromptId));
        scene.Play();
        Need(scene.Facts<DamageAppliedEvent>().Skip(hit).ToArray() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 2 }],
            "The just-bought real ordinary skill is immediately usable and the just-bought low-HP tactic modifies its real damage.");
        var created = scene.Facts<ShanheMarketCardCreatedEvent>().Single(e => e.RewardId == "slash");
        var card = scene.Player().Hand.Single(c => c.Id == created.CardId);
        var action = scene.Game.GetHumanLegalActions().First(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual([1]));
        scene.Submit(new PlayCardCommand(0, card.Id, [1], scene.Game.Revision, scene.Game.PendingDecision!.PromptId, action.PlayedCardKind));
        scene.Play();
        Need(scene.Game.CardMovements.Any(m => m.CardId == card.Id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) &&
            scene.Game.CardMovements.Last(m => m.CardId == card.Id).To == CardLocation.OutsideGame,
            "A purchased physical card enters the current hand, pays a native actual use and retains the campaign resource post-use removal contract.");
        var last = scene.Market().Gold;
        Need(!scene.Game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0,
                scene.Facts<ShanheMarketPurchaseCommittedEvent>().Last().ChoiceId, null, scene.Game.Revision)).Accepted && scene.Market().Gold == last,
            "Reusing an already paid live offer cannot pay or grant it a second time.");
    }

    public static void ACardBoughtDuringSlashResponseJoinsThatExactResponse()
    {
        var scene = Scene.Start();
        scene.Submit(new EndPlayPhaseCommand(0, scene.Game.Revision, scene.Game.PendingDecision!.PromptId));
        for (var step = 0; step < 120 && scene.Game.PendingDecision?.Kind != DecisionKind.RespondDodge; step++)
            scene.Submit(new AdvanceOneStepCommand(scene.Game.Revision));
        Need(scene.Game.PendingDecision is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 },
            "The deterministic real enemy Slash reaches the human's original response prompt.");
        var original = scene.Game.PendingDecision!;
        var frames = scene.Game.ResolutionStack.Select(f => f.Id).ToArray();
        scene.Buy("dodge", returnToPlay: false);
        var created = scene.Facts<ShanheMarketCardCreatedEvent>().Single(e => e.RewardId == "dodge");
        var response = scene.Game.PendingDecision!;
        Need(response.Kind == original.Kind && response.SourceSeat == original.SourceSeat && response.PromptId != original.PromptId &&
            scene.Game.ResolutionStack.Select(f => f.Id).SequenceEqual(frames) && response.ValidCardIds.Contains(created.CardId),
            "Purchasing during a pending Slash restores the exact unchanged native frame stack and rebuilds that response with the new physical Dodge.");
        var choice = response.Choices.First(c => c.Cards.Contains(created.CardId));
        scene.Submit(new AnswerPromptCommand(0, response.PromptId, choice.Id, scene.Game.Revision));
        scene.Play();
        Need(scene.Game.CardMovements.Any(m => m.CardId == created.CardId && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) &&
            scene.Player().Hp == 4 && scene.Facts<ShanheMarketPurchaseCommittedEvent>().Length == 1,
            "The newly bought card actually answers the already pending Slash, without replaying its payment or letting the attack hit.");
    }

    public static void DyingPurchasesUseRealNegativeHealthAndReturnTheOriginalRescue()
    {
        var scene = Scene.Start();
        scene.Buy("peach-initial");
        scene.Submit(new UseProgramSkillCommand(0, Driver, "hurt-five", [], [], scene.Game.Revision, scene.Game.PendingDecision!.PromptId));
        Need(scene.Player().Hp == -1 && scene.Game.PendingDecision is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 },
            "A genuine five-damage segment leaves the living human at minus one with its real original rescue prompt.");
        var dyingId = scene.Game.ResolutionStack.OfType<DyingFrame>().Single().Id;
        var revision = scene.Game.Revision; var gold = scene.Market().Gold;
        var paidHealth = scene.Market().ShopItems.Single(i => i.Reward?.Id == "health-cost");
        Need(scene.Market().Hp == -1 && !paidHealth.CanAfford &&
            !scene.Game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, paidHealth.ChoiceId, null, revision)).Accepted &&
            scene.Game.Revision == revision && scene.Market().Gold == gold,
            "Negative-health quotes remain readable, but a positive-health cost cannot spend health or money from a dying owner.");
        scene.Buy("peach-rescue", returnToPlay: false);
        var peach = scene.Facts<ShanheMarketCardCreatedEvent>().Single(e => e.RewardId == "peach-rescue");
        var prompt = scene.Game.PendingDecision!;
        Need(scene.Player().Hp == -1 && prompt.Kind == DecisionKind.RescueDying && prompt.ValidCardIds.Contains(peach.CardId) &&
            scene.Game.ResolutionStack.OfType<DyingFrame>().Single().Id == dyingId,
            "Buying a zero-health-cost Peach at negative HP immediately adds it to the same original rescue response.");
        var choice = prompt.Choices.First(c => c.Cards.Contains(peach.CardId));
        scene.Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, scene.Game.Revision));
        for (var step = 0; step < 70 && scene.Game.PendingDecision?.Kind != DecisionKind.RescueDying; step++)
            scene.Submit(new AdvanceOneStepCommand(scene.Game.Revision));
        Need(scene.Player().Hp == 0 && scene.Game.PendingDecision is { Kind: DecisionKind.RescueDying } &&
            scene.Game.ResolutionStack.OfType<DyingFrame>().Single().Id == dyingId,
            "The purchased Peach actually recovers minus one to zero and resumes that same still-unfinished dying resolution.");
        revision = scene.Game.Revision; gold = scene.Market().Gold;
        Need(!scene.Game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0,
                scene.Market().ShopItems.Single(i => i.Reward?.Id == "health-cost").ChoiceId, null, revision)).Accepted &&
            scene.Game.Revision == revision && scene.Market().Gold == gold,
            "A positive-health cost remains unaffordable at exactly zero HP as well.");
        scene.Buy("medicine", returnToPlay: false);
        scene.Play();
        Need(scene.Player().Hp == 1 && scene.Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            scene.Facts<DamageAppliedEvent>() is [{ TargetSeat: 0, Amount: 5 }] &&
            scene.Facts<DyingResolvedEvent>().Where(e => e.ResolutionId == dyingId).ToArray() is [{ VictimSeat: 0, Survived: true }] &&
            scene.Facts<RecoveryAppliedEvent>().Select(e => e.RemainingHp).SequenceEqual([0, 1]) &&
            scene.Facts<ShanheMarketPurchaseCommittedEvent>().Length == 3 && scene.Market().Gold == 2600,
            "A zero-health-cost real recovery purchase then saves the original dying owner, exits its native dying window once, pays once and returns without replaying the five-point attack.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot Player() => Game.CreateSnapshot(0).Players[0];
        public ShanheCampaignSnapshot Market() => Game.CreateShanheMarketSnapshot(0)!;
        public T[] Facts<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var r = Game.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected live market native command."); }
        public void Play()
        {
            for (var step = 0; step < 140; step++)
            {
                if (Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                if (Game.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ?? prompt.Choices[0];
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded live market fixture lost its original Play boundary.");
        }
        public void Buy(string reward, bool returnToPlay = true)
        {
            var item = Market().ShopItems.Single(i => i.Reward?.Id == reward);
            Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, Game.Revision));
            if (returnToPlay) Play();
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheMarketPackage(),
                new StandardShanheRoundRecoveryPackage(), new StandardShanheLowHpDamagePackage(), new StandardShanheWealthPackage(), new Fixture());
            ShanheRewardDefinition[] rewards =
            [
                new("blood", "血战Ⅱ") { TacticSkillId = StandardShanheRoundRecoveryPackage.Xuezhan2SkillId },
                new("rage", "狂暴Ⅱ") { TacticSkillId = StandardShanheLowHpDamagePackage.Kuangbao2SkillId },
                new("wealth", "入场财富") { CampaignEconomy = new(GoldAbove: 2000,
                    BattleSkillId: StandardShanheWealthPackage.WealthHandLimitSkillId) },
                new("skill", "即时习得") { SkillId = Extra },
                new("slash", "杀", StartingCards: [new(CardKind.Slash, Suit.Spade, 7)]),
                new("dodge", "闪", StartingCards: [new(CardKind.Dodge, Suit.Diamond, 2)]),
                new("peach-initial", "备用桃", StartingCards: [new(CardKind.Peach, Suit.Heart, 3)]),
                new("peach-rescue", "救援桃", StartingCards: [new(CardKind.Peach, Suit.Heart, 4)]),
                new("health-cost", "需付体力的牌", StartingCards: [new(CardKind.Dodge, Suit.Heart, 6)]),
                new("medicine", "即时军医", Heal: 1)
            ];
            var choices = rewards.Select(r => new ShanheNodeChoiceDefinition("buy-" + r.Id, r.Name, r.Id == "wealth" ? 1000 : 100,
                HpCost: r.Id == "health-cost" ? 1 : 0, RewardId: r.Id, LeavesNode: false)).ToArray();
            var definition = new ShanheCampaignDefinition("fixture:live-market", "即时集市", [Owner], ["shop"],
                [new("shop", "常驻商店", ShanheNodeKind.Shop, ["battle"], Choices: choices),
                 new("battle", "固定小战斗", ShanheNodeKind.Battle, [], [new("guard", "固定守军", [new(Peer)
                    { StartingHand = [new(CardKind.Slash, Suit.Spade, 8)] }])])], rewards, StartingGold: 3000)
                { StartingResourcesExpireOnUse = true };
            var session = ShanheCampaignSession.Create(definition, new(83, Owner)
                { UseCodexRewardPool = true, PersistentMarket = true }, _ => registry);
            var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
            Need(entered.Accepted, entered.Error ?? "The live market fixture must enter a real battle.");
            var setup = session.PendingBattleSetup!;
            Need(setup.MarketSetup is not null, "Persistent campaign battles carry the frozen paid-market seed.");
            var engine = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var scene = new Scene(engine); scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-live-market", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var skills = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                 {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"add","value":5,"priority":0}],
                  "activations":[{"id":"hurt-five","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":5}]}]},
                 {"id":"{{Quiet}}","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"add","value":5,"priority":0}]},
                 {"id":"{{Extra}}","revision":1,"activations":[{"id":"ping","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, Extra }.ToDictionary(id => id, _ => new { name = "即时集市小型原生驱动", description = "真实购买后立即使用" }) }));
            foreach (var (id, p) in skills.Programs) builder.AddSkill(new(id, id, id) { Program = p, ProgramPresentation = skills.Presentations[id] });
            builder.AddGeneral(new(Owner, "集市使用者", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "固定闪牌堆", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "即时行军商店", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
