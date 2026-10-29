using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheRecastTacticChecks
{
    private const string One = StandardShanheRecastTacticsPackage.ZhudaoSkillId;
    private const string Two = StandardShanheRecastTacticsPackage.Zhudao2SkillId;
    private const string Rain = StandardShanheRecastTacticsPackage.JishiyuSkillId;
    private const string Owner = "fixture:recast-tactic-owner", Enemy = "fixture:recast-tactic-enemy";
    private const string Driver = "fixture:recast-tactic-driver", Quiet = "fixture:recast-tactic-quiet";
    private const string Deck = "fixture:recast-tactic-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheRecastTacticsPackage(), new Fixture()));

    public static void CompletedSlashRecastsHandAndEquipmentAfterNativeRecoveryChild()
    {
        var setup = new ShanheBattleSetup(83, 1, "recast-paid-child", Owner, 3, 0, 0, 0, 0, [],
            [new(CardKind.Slash), new(CardKind.Dodge), new(CardKind.SilverLion)], [],
            [new(Enemy) { StartingHand = [] }], [], new string('d', 64)) { TacticSkillIds = [Two] };
        var s = Scene.Start(setup);
        var lion = s.View().Hand.Single(card => card.Kind == CardKind.SilverLion).Id;
        s.Use(lion);
        s.Play();
        var slash = s.View().Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var hand = s.View().Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        Need(s.View().Equipment.Single().Id == lion, "The scenario's equipment recipe first enters hand and is genuinely equipped before Slash and the later recast cost.");
        s.Use(slash);
        s.Reach(() => s.At(Two, "activate"));
        Need(s.Events<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 1, Amount: 1 }] &&
            s.Game.CardMovements.Count(move => move.CardId == slash && move.To == CardLocation.DiscardPile) == 1 &&
            s.View() is { Hp: 3, HandCount: 1, Equipment.Count: 1 },
            "The owner's actual physical Slash finishes before the optional recast; one real hand card and a wounded owner's Silver Lion remain available.");
        s.Answer(choice => Action(choice) == "activate");
        s.Reach(() => s.At(Two, "select-owned-cards"));
        var paidFrame = s.Frame(Two)!.Id;
        s.Answer(choice => choice.Cards.SequenceEqual([hand]));
        Need(s.Frame(Two)?.OwnedCardSelection?.SelectedCardIds.SequenceEqual([hand]) == true &&
            s.Zone(hand) == CardLocation.Hand(0) && s.Zone(lion) == CardLocation.Equipment(0),
            "Selecting the first of up to two cards stages the original entity without paying either material early.");
        s.Answer(choice => choice.Cards.SequenceEqual([lion]));
        s.Reach(() => s.Prompt?.SkillPrompt?.SkillId == "ol:liangzhu");
        var paid = s.Frame(Two)!;
        var receipt = paid.BoundCardsRecast!;
        var costs = s.Game.CardMovements.Where(move => move.Reason == CardMoveReasons.RecastDiscard).ToArray();
        Need(paid.Id == paidFrame && receipt is { Stage: ProgramBoundCardsRecastStage.CostChildren, DrawCount: 0 } &&
            receipt.Source.SkillId == Two && receipt.Materials.Select(material => material.CardId).ToHashSet().SetEquals([hand, lion]) &&
            receipt.Materials.Single(material => material.CardId == hand).From == CardLocation.Hand(0) &&
            receipt.Materials.Single(material => material.CardId == lion).From == CardLocation.Equipment(0) &&
            receipt.CostBatchId > 0 && costs.Length == 2 && costs.All(move => move.To == CardLocation.DiscardPile) &&
            costs.Min(move => move.Sequence) > receipt.SequenceBefore && costs.Max(move => move.Sequence) == receipt.SequenceAfter &&
            s.View() is { Hp: 4, HandCount: 0, Equipment.Count: 0 } && s.RecastDraws().Length == 0 &&
            s.Events<RecoveryAppliedEvent>() is [{ TargetSeat: 0, Amount: 1 }] &&
            s.Events<SpSunShangxiangLiangzhuStartedEvent>() is [{ RecoveredSeat: 0 }] &&
            s.Events<CardRecastEvent>().Length == 0 && !s.Events<ProgramBindingResolvedEvent>().Any(item => item.FrameId == paidFrame),
            "Both original HE entities are paid in one native recast batch, then genuine Silver Lion recovery pauses for real Liangzhu on the same paid frame before any recast draw or completion.",
            new { paid.Id, receipt, costs, Owner = s.View(), Draws = s.RecastDraws(), Recoveries = s.Events<RecoveryAppliedEvent>() });
        s.Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "decline");
        s.Play();
        var recasts = s.Events<CardRecastEvent>();
        var draws = s.RecastDraws();
        Need(s.View() is { Hp: 4, HandCount: 2, Equipment.Count: 0 } &&
            draws.Length == 2 && draws[1].Sequence == draws[0].Sequence + 1 &&
            draws.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Sequence > receipt.SequenceAfter) &&
            recasts.Length == 2 && recasts.All(item => item.ActorSeat == 0 && item.DrawCount == 1) &&
            recasts.Select(item => item.CardId).ToHashSet().SetEquals([hand, lion]) &&
            s.Game.CardMovements.Count(move => move.Reason == CardMoveReasons.RecastDiscard) == 2 &&
            s.Events<ProgramBindingResolvedEvent>().Single(item => item.FrameId == paidFrame) is
                { SkillId: Two, Window: SkillProgramTriggerWindow.CardUseCompleted, Activated: true, Completed: true } &&
            s.Events<SpSunShangxiangLiangzhuCompletedEvent>().Length == 1 && s.Frame(Two) is null &&
            s.Game.CreateSnapshot(0).ProcessingCardCount == 0,
            "Declining the real recovery child returns to the original paid recast, draws exactly two physical cards in one batch, and completes each material and the parent binding once.",
            new { Draws = draws, Recasts = recasts, Bindings = s.Events<ProgramBindingResolvedEvent>() });
        var recoveryEnd = s.Sequence<SpSunShangxiangLiangzhuCompletedEvent>(_ => true);
        Need(recoveryEnd < s.Game.Events.Where(item => item.Payload is CardMovedEvent move && move.Reason == CardMoveReasons.RecastDraw).Min(item => item.Sequence) &&
            s.Sequence<ProgramBindingResolvedEvent>(item => item.FrameId == paidFrame) > recoveryEnd,
            "The recovery child's actual completion precedes the native recast draw and the original Slash-completed binding return.");
    }

    public static void PurchasedTiersRecastDodgedSlashAndRainDrawsAfterOutsideTurnResponse()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheRecastTacticsPackage(), new Fixture(attack: true));
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var skills = new[] { One, Two, Rain };
        var rewards = skills.Select(skill => current.Rewards.Single(reward => reward.TacticSkillId == skill)).ToArray();
        var loadout = new ShanheRewardDefinition("loadout", "真实随身牌", StartingCards:
            [new(CardKind.Slash), new(CardKind.Crossbow), new(CardKind.Dodge)]);
        var definition = new ShanheCampaignDefinition("fixture:recast-stock", "购买独立重铸战法", [Owner], ["loadout"],
        [
            new("loadout", "领取实体随身牌", ShanheNodeKind.Event, ["shop"], Choices: [new("take", loadout.Name, RewardId: loadout.Id)]),
            new("shop", "原价与同类加价的战法商店", ShanheNodeKind.Shop, ["battle"], Choices: rewards.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name,
                    StandardShanheCampaignCatalog.ShopMetadataByRewardId[reward.Id].Price, RewardId: reward.Id, LeavesNode: false)).ToArray()),
            new("battle", "闪避后重铸与真实回合外响应", ShanheNodeKind.Battle, [],
                [new("guard", "仅一张闪的原生敌军", [new(Enemy) { StartingHand = [new(CardKind.Dodge)] }])])
        ], rewards.Append(loadout).ToArray(), StartingGold: 1500, ExtraSkillSlots: 1) { Schools = current.Schools };
        var campaign = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UseCodexRewardPool = true, UnlockedRewardIds = rewards.Select(reward => reward.Id).ToArray() }, _ => registry);
        Campaign(new MoveShanheNodeCommand("loadout", campaign.Revision));
        Campaign(new ChooseShanheNodeOptionCommand("take", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        var prices = new[] { 300, 800, 400 };
        for (var index = 0; index < rewards.Length; index++)
        {
            var item = campaign.Snapshot.ShopItems.Single(item => item.Reward?.Id == rewards[index].Id);
            Need(item is { State: ShanheShopItemState.Available, RequiresSkillReplacement: false } && item.GoldCost == prices[index],
                "The exact purchased sources retain their own 300/700/200 base prices plus the real accumulated tactic-category surcharge.", new { index, item.GoldCost });
            Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, campaign.Revision));
        }
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.TacticIds.ToHashSet().SetEquals(rewards.Select(reward => reward.Id)) &&
            campaign.Snapshot.ExtraSkillIds.Count == 0 && campaign.Snapshot.ExtraSkillSlots == 1,
            "All three exact unique sources are genuinely paid for and coexist as distinct tactics without consuming the ordinary skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        Need(campaign.PendingBattleSetup!.TacticSkillIds.ToHashSet().SetEquals(skills), "The real encounter carries both independent recast tiers and Timely Rain into battle.");
        var s = Scene.Start(campaign.PendingBattleSetup!, registry);
        Need(s.View().GeneralId == Owner && skills.All(skill => s.View().Skills!.Any(item => item.ContentId == skill)) &&
            s.Game.CreateSnapshot(0).Players.Count(player => player.IsHuman) == 1,
            "The unrelated printed general receives all bought sources in a real two-seat encounter with one human.");
        var npcDodge = s.View(1).Hand.Single().Id;
        s.Use(s.View().Hand.Single(card => card.Kind == CardKind.Crossbow).Id);
        s.Play();
        var slash = s.View().Hand.Single(card => card.Kind == CardKind.Slash).Id;
        s.Use(slash);
        s.Reach(() => s.At(One, "activate") || s.At(Two, "activate"));
        Need(s.Events<DamageAppliedEvent>().Length == 0 && s.Events<CardRespondedEvent>().Any(item => item.CardId == npcDodge && item.ResponderSeat == 1 && item.SourceSeat == 0) &&
            s.Zone(slash) == CardLocation.DiscardPile && s.Zone(npcDodge) == CardLocation.DiscardPile && s.View(1).HandCount == 0,
            "The NPC genuinely pays its sole Dodge for the owner's physical Slash; the fully dodged use still opens the recast tactics after cleanup.");
        var seen = new HashSet<string>();
        for (var index = 0; index < 2; index++)
        {
            var skill = s.Prompt!.SkillPrompt!.SkillId;
            Need((skill == One || skill == Two) && seen.Add(skill), "Each separately purchased recast tier is offered once for the same completed Slash.");
            s.Answer(choice => Action(choice) == "activate");
            s.Reach(() => s.At(skill, "select-owned-cards"));
            var cost = s.View().Hand.Single().Id;
            s.Answer(choice => choice.Cards.SequenceEqual([cost]));
            if (skill == Two)
            {
                Need(s.At(Two, "finish-owned-cards") && s.Zone(cost) == CardLocation.Hand(0),
                    "The higher tier genuinely permits finishing at one card before its two-card maximum and has not yet paid the staged material.");
                s.Answer(choice => Action(choice) == "finish-owned-cards");
            }
            s.Reach(() => s.Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || s.At(One, "activate") || s.At(Two, "activate"));
            Need(s.View().HandCount == 1 && s.RecastDraws().Length == index + 1 && s.Zone(cost) == CardLocation.DiscardPile &&
                s.Game.CardMovements.Count(move => move.CardId == cost && move.Reason == CardMoveReasons.RecastDiscard) == 1 &&
                !s.Events<ProgramBindingResolvedEvent>().Any(item => item.SkillId == Rain),
                "Each tier independently pays one last hand entity and natively draws one; losing the last hand card during the owner's actual turn never activates Timely Rain.");
        }
        s.Play();
        Need(seen.SetEquals([One, Two]) && s.Events<CardRecastEvent>().Length == 2 &&
            new[] { One, Two }.All(skill => s.Events<ProgramBindingResolvedEvent>().Count(item => item.SkillId == skill && item.Activated && item.Completed) == 1),
            "The lower tier is not replaced by the higher tier, and both complete their one-card native recasts after a dodged Slash.");
        var response = s.View().Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        var ownTurn = s.Game.State.TurnNumber;
        s.Accept(new EndPlayPhaseCommand(0, s.Game.Revision, s.Prompt!.PromptId));
        s.Reach(() => s.Prompt is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        Need(s.Game.State.TurnNumber > ownTurn && s.Events<CardUseDeclaredEvent>().Any(item => item.SourceSeat == 1 && item.CardKind == CardKind.Slash) &&
            s.Prompt!.Choices.Any(choice => choice.Cards.SequenceEqual([response])),
            "The real NPC's next actual turn issues a native virtual Slash and asks the human owner to pay its sole physical Dodge.");
        s.Answer(choice => choice.Cards.SequenceEqual([response]));
        s.Play();
        var rainDraws = s.Game.CardMovements.Where(move => move.Reason.Value == "skill-program." + Rain + ".Draw").ToArray();
        Need(rainDraws.Length == 2 && rainDraws.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            s.View() is { Hp: 4, HandCount: 2 } && s.Events<DamageAppliedEvent>().Length == 0 &&
            s.Events<CardRespondedEvent>().Any(item => item.CardId == response && item.ResponderSeat == 0 && item.SourceSeat == 1) &&
            s.Game.CardMovements.Count(move => move.CardId == response && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
            s.Game.CardMovements.Count(move => move.CardId == response && move.To == CardLocation.DiscardPile) == 1 &&
            s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Rain) is
                { OwnerSeat: 0, Window: SkillProgramTriggerWindow.CardsMoved, Activated: true, Completed: true } &&
            s.RecastDraws().Length == 2 && s.Events<CardRecastEvent>().Length == 2 &&
            new[] { One, Two }.All(skill => s.Events<ProgramBindingResolvedEvent>().Count(item => item.SkillId == skill && item.Activated) == 1),
            "One genuine outside-turn final-hand Dodge response draws exactly two through bought Timely Rain; it pays once and creates no extra owner-use recast trigger.",
            new { rainDraws, Owner = s.View(), Responses = s.Events<CardRespondedEvent>(), Bindings = s.Events<ProgramBindingResolvedEvent>() });
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real recast-source purchase."); }
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Enumerable.Range(0, 2).Select(seat => Game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public ProgramSkillFrame? Frame(string skill) => Game.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.SkillId == skill);
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public CardMovementRecord[] RecastDraws() => Game.CardMovements.Where(move => move.Reason == CardMoveReasons.RecastDraw).ToArray();
        public CardLocation Zone(int id) => Game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location;
        public long Sequence<T>(Func<T, bool> predicate) => Game.Events.Single(item => item.Payload is T value && predicate(value)).Sequence;
        public bool At(string skill, string action) => Prompt?.SkillPrompt?.SkillId == skill && Prompt.Choices.Any(choice => Action(choice) == action);
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected recast-tactic command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = Prompt!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(predicate).Id, Game.Revision)); }
        public void Use(int card)
        {
            var action = Game.GetHumanLegalActions().Single(item => item.CardId == card);
            Accept(new PlayCardCommand(0, card, action.TargetSeats, Game.Revision, Prompt!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "Unexpected human recast-tactic input: " + JsonSerializer.Serialize(Prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded recast-tactic boundary: " + JsonSerializer.Serialize(Prompt));
        }
        public static Scene Start(ShanheBattleSetup setup, ContentRegistry? registry = null)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry ?? Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool attack = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-recast-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ["ol:sp-sun-shangxiang", "classic:sun-shangxiang"]);
            var lion = CardCatalog.Get(CardKind.SilverLion);
            builder.AddCard(new("fixture:recast-silver-lion", lion.DisplayName, lion.CategoryName, lion.Description, CardKind.SilverLion));
            var attackTrigger = attack ? """
                ,{"id":"native-next-turn-slash","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,
                  "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLivingVirtualSlashTarget"},
                    {"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}
                """ : "";
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"settle-hand","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]},{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[
                    {"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                     "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}{{attackTrigger}}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "实体手牌与真实回合", description = "小型重铸实战驱动" }) }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "独立战法持有者", "supporter", Driver, "shu", 4, ["ol:liangzhu"]));
            builder.AddGeneral(new(Enemy, "原生安静敌军", "supporter", Quiet, "wei", 8));
            builder.AddDeck(new(Deck, "零自然摸牌与固定实体牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(index => new ContentDeckPhysicalCard(attack ? "standard:dodge" : "standard:slash", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "实体重铸实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
