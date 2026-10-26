using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBeigeSkillChecks
{
    private const string Beige = StandardShanheBeigeSkillsPackage.SkillId;
    private const string Owner = "fixture:beige-stock-owner", Enemy = "fixture:beige-stock-enemy";
    private const string Driver = "fixture:beige-stock-driver", Deck = "fixture:beige-stock-deck";
    private const string JudgmentReason = "skill.shanhe.beige-3140";
    private const string PaymentReason = "skill-program." + Beige + ".SelectAndMoveOwnedCard";
    private const string MoveReason = "skill-program." + Beige + ".MoveBoundCards";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheBeigeSkillsPackage(), new Fixture()));

    public static void PurchasedBeigePaysOnlyEquipmentBeforeClubJudgmentAndSourceNpcDiscardsTwo()
    {
        const string rewardId = "build-skill-3140";
        var current = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var reward = current.Rewards.Single(item => item.Id == rewardId);
        var definition = new ShanheCampaignDefinition("fixture:shanhe-beige-stock", "真实悲歌购买与梅花判定", [Owner], ["shop"],
        [
            new("shop", "技能集市", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("buy", reward.Name, GoldCost: StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId].Price,
                    RewardId: rewardId, LeavesNode: false)]),
            new("battle", "悲歌梅花实战", ShanheNodeKind.Battle, [],
                [new("guard", "持杀与两张真实弃牌的敌军", [new(Enemy)
                    { StartingHand = [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Dodge, Suit.Heart, 8), new(CardKind.Dodge, Suit.Diamond, 9)] }])])
        ], [reward], StartingGold: 300, ExtraSkillSlots: 1);
        var campaign = ShanheCampaignSession.Create(definition,
            new(83, Owner) { UseCodexRewardPool = true, UnlockedRewardIds = [rewardId] }, _ => Registry.Value);
        Campaign(new MoveShanheNodeCommand("shop", campaign.Revision));
        var item = campaign.Snapshot.ShopItems.Single();
        Need(item is { State: ShanheShopItemState.Available, GoldCost: 300, RequiresSkillReplacement: false } &&
            item.Reward?.Id == rewardId && reward.SkillId == Beige,
            "The actual codex merchant sells the exact fixed-outcome S5 Beige source for three hundred copper into one ordinary skill slot.");
        Campaign(new ChooseShanheNodeOptionCommand(item.ChoiceId, null, campaign.Revision));
        Need(campaign.Snapshot.Gold == 0 && campaign.Snapshot.ExtraSkillSlots == 1 &&
            campaign.Snapshot.ExtraSkillIds.SequenceEqual([Beige]) && campaign.Snapshot.TacticIds.Count == 0,
            "The real purchase pays once and grants only the intended ordinary-slot Beige identity, without gifting a printed character or another version.");
        Campaign(new ChooseShanheNodeOptionCommand("$leave", null, campaign.Revision));
        Campaign(new MoveShanheNodeCommand("battle", campaign.Revision));
        var s = Scene.Start(campaign.PendingBattleSetup!);
        s.Play();
        var players = s.Game.CreateSnapshot(0).Players;
        Need(players.Count == 2 && players.Count(player => player.IsHuman) == 1 && players[0].GeneralId == Owner &&
            players[0].Skills!.Any(skill => skill.ContentId == Beige) && players[0].Skills!.All(skill => skill.ContentId is Driver or Beige) &&
            players[1].GeneralId == Enemy && !players[1].IsHuman && players[1].TeamId != players[0].TeamId && players[1].HandCount == 3,
            "The real two-seat Shanhe encounter grants bought Beige to an unrelated owner and keeps the opponent a native AI with its authored three physical cards.");
        s.EquipCrossbow();
        var cost = s.View().Equipment.Single().Id;
        var slash = s.View(1).Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var sourceCards = s.View(1).Hand.Where(card => card.Id != slash).Select(card => card.Id).ToArray();
        Need(s.View() is { HandCount: 0, Equipment.Count: 1, Hp: 4 } && s.View().Equipment.Single().Kind == CardKind.Crossbow,
            "The owner really equips its only starting card and therefore has zero hand cards and exactly one equipment entity available as Beige's cost.");
        s.RequestSlash();
        s.Reach(() => s.AtBeige("activate") || s.Prompt is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        if (s.Prompt is { Kind: DecisionKind.RespondDodge })
        {
            s.Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
            s.Reach(() => s.AtBeige("activate"));
        }
        Need(s.Events<CardUseDeclaredEvent>().Where(item => item.SourceSeat == 1).ToArray() is [{ CardId: var actualSlash, CardKind: CardKind.Slash }] && actualSlash == slash &&
            s.Events<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 1 }] && s.View().Hp == 3 &&
            s.Game.CardMovements.Count(move => move.CardId == slash && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) == 1 &&
            s.View(1).Hand.Select(card => card.Id).ToHashSet().SetEquals(sourceCards) &&
            !s.Game.CardMovements.Any(move => move.Reason.Value == PaymentReason) && s.Judgments().Length == 0 &&
            s.View().Equipment.Single().Id == cost,
            "The native requested physical Slash really leaves the NPC's hand and injures the owner before Beige is offered; its sole equipment cost remains unpaid and no judgment has begun.");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.Prompt?.Choices.Any(choice => choice.Cards.SequenceEqual([cost])) == true);
        var paidFrame = s.BeigeFrame!.Id;
        Need(s.Prompt!.PlayerSeat == 0 && s.Prompt.Choices.Where(choice => choice.Cards.Count > 0).All(choice => choice.Cards.SequenceEqual([cost])) &&
            s.Zone(cost) == CardLocation.Equipment(0) && s.Judgments().Length == 0,
            "The bought skill publishes the owner's equipment as its only physical payment choice, still without judging before that choice is accepted.");
        s.Answer(choice => choice.Cards.SequenceEqual([cost]));
        s.Reach(() => s.AtBeige("select-target"));
        Need(s.BeigeFrame?.Id == paidFrame && s.Zone(cost) == CardLocation.DiscardPile && s.View() is { HandCount: 0, Equipment.Count: 0 } &&
            s.Game.CardMovements.Count(move => move.CardId == cost && move.From == CardLocation.Equipment(0) &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == PaymentReason) == 1 && s.Judgments().Length == 0 &&
            s.Prompt!.Choices.All(choice => choice.Targets.SequenceEqual([0])),
            "The original after-damage frame has already paid exactly one equipment card before its natural injured-target input; no judgment can precede that committed payment and only the hurt owner is the target.");
        s.Answer(choice => choice.Targets.SequenceEqual([0]));
        s.Reach(() => s.Prompt?.PlayerSeat == 1 && s.AtBeige("select-owned-cards"));
        var judgment = s.Events<JudgmentResolvedEvent>().Single(item => item.Reason == JudgmentReason);
        Need(s.Judgments() is [{ TargetSeat: 0 }] && judgment is { TargetSeat: 0, Suit: Suit.Club, CardId: not null } &&
            s.BeigeFrame is { OwnedCardSelection: { CardOwnerSeat: 1, RequiredCount: 2, SelectedCardIds.Count: 0 } } frame && frame.Id == paidFrame &&
            s.Prompt!.Choices.All(choice => choice.Cards.Count == 1 && sourceCards.Contains(choice.Cards.Single())) &&
            s.View(1).Hand.Select(card => card.Id).ToHashSet().SetEquals(sourceCards) && s.View().HandCount == 0 && s.View().Equipment.Count == 0,
            "The injured owner makes the genuine Club judgment, then the same paid frame asks the original NPC damage source to choose two of its own entities; the empty-handed owner is not substituted as the payer.");
        s.Step();
        Need(s.BeigeFrame?.OwnedCardSelection is { CardOwnerSeat: 1, SelectedCardIds.Count: 1 } &&
            sourceCards.All(id => s.Zone(id) == CardLocation.Hand(1)) &&
            !s.Game.CardMovements.Any(move => move.From == CardLocation.Hand(1) && move.Reason.Value == MoveReason),
            "The real source AI stages the first required card on the original frame without prematurely moving one card or repeating the owner's paid cost.");
        s.Play();

        var sourceMoves = s.Game.CardMovements.Where(move => move.From == CardLocation.Hand(1) && move.Reason.Value == MoveReason).ToArray();
        var judgmentCard = judgment.CardId!.Value;
        var resolved = s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Beige);
        Need(sourceMoves.Length == 2 && sourceMoves.All(move => move.To == CardLocation.DiscardPile) &&
            sourceMoves.Select(move => move.CardId).ToHashSet().SetEquals(sourceCards) &&
            sourceCards.All(id => s.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(1) && move.To == CardLocation.DiscardPile) == 1) &&
            s.View(1).HandCount == 0 && s.View() is { Hp: 3, HandCount: 0, Equipment.Count: 0 } &&
            s.Game.CardMovements.Count(move => move.Reason.Value == PaymentReason) == 1 &&
            s.Game.CardMovements.Count(move => move.CardId == judgmentCard && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Judgment(0) && move.Reason == CardMoveReasons.JudgmentReveal) == 1 &&
            s.Game.CardMovements.Count(move => move.CardId == judgmentCard && move.From == CardLocation.Judgment(0) &&
                move.To == CardLocation.Processing && move.Reason.Value == "skill-program.judgment.result") == 1 &&
            s.Game.CardMovements.Count(move => move.CardId == judgmentCard && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == MoveReason) == 1 && s.Zone(judgmentCard) == CardLocation.DiscardPile &&
            s.Game.CardMovements.Count(move => move.CardId == slash && move.To == CardLocation.DiscardPile) == 1 &&
            s.Events<DamageAppliedEvent>().Length == 1 && s.Events<JudgmentResolvedEvent>().Count(item => item.Reason == JudgmentReason) == 1 &&
            resolved is { OwnerSeat: 0, Window: SkillProgramTriggerWindow.AfterDamageApplied, Activated: true, Completed: true } && resolved.FrameId == paidFrame &&
            s.Events<ProgramSkillResolvedEvent>().Single(item => item.SkillId == Driver) is { ActivationId: "request", Completed: true } &&
            s.BeigeFrame is null && s.Game.CreateSnapshot(0).ProcessingCardCount == 0,
            "The actual NPC's two original entities are discarded once for Club, the judged card and real Slash both clean up normally, and the one original paid Beige binding completes and returns to its request activation without charging the owner again.",
            new
            {
                sourceCards, sourceMoves,
                Owner = new { s.View().Hp, s.View().HandCount, EquipmentCount = s.View().Equipment.Count },
                SourceHandCount = s.View(1).HandCount,
                PaymentMoves = s.Game.CardMovements.Where(move => move.Reason.Value == PaymentReason).ToArray(),
                JudgmentMoves = s.Game.CardMovements.Where(move => move.CardId == judgmentCard).ToArray(),
                SlashMoves = s.Game.CardMovements.Where(move => move.CardId == slash).ToArray(),
                Damage = s.Events<DamageAppliedEvent>(), Judgments = s.Events<JudgmentResolvedEvent>(), resolved,
                DriverResolved = s.Events<ProgramSkillResolvedEvent>().Where(item => item.SkillId == Driver).ToArray(),
                RemainingBeigeFrame = s.BeigeFrame?.Id, s.Game.CreateSnapshot(0).ProcessingCardCount
            });

        var sourcePaymentEnd = s.Game.Events.Where(item => item.Payload is CardMovedEvent move && move.From == CardLocation.Hand(1) &&
            move.Reason.Value == MoveReason).Max(item => item.Sequence);
        var ordered = new[]
        {
            s.Sequence<DamageAppliedEvent>(_ => true),
            s.Sequence<CardMovedEvent>(item => item.CardId == cost && item.Reason.Value == PaymentReason),
            s.Sequence<JudgmentRequestedEvent>(item => item.Reason == JudgmentReason),
            s.Sequence<JudgmentResolvedEvent>(item => item.Reason == JudgmentReason),
            sourcePaymentEnd,
            s.Sequence<CardMovedEvent>(item => item.CardId == judgmentCard && item.To == CardLocation.DiscardPile),
            s.Sequence<ProgramBindingResolvedEvent>(item => item.SkillId == Beige),
            s.Sequence<ProgramSkillResolvedEvent>(item => item.SkillId == Driver)
        };
        Need(ordered.Zip(ordered.Skip(1)).All(pair => pair.First < pair.Second),
            "The real commit order is Slash injury, owner equipment payment, injured-player judgment, source's two-card payment, judgment cleanup, original Beige trigger completion and original request activation return.", new { ordered });
        return;

        void Campaign(ShanheCampaignCommand command)
        { var result = campaign.Submit(command); Need(result.Accepted, result.Error ?? "Rejected actual Beige stock purchase."); }
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Enumerable.Range(0, 2).Select(seat => Game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        public ProgramSkillFrame? BeigeFrame => Game.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.SkillId == Beige);
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public JudgmentRequestedEvent[] Judgments() => Events<JudgmentRequestedEvent>().Where(item => item.Reason == JudgmentReason).ToArray();
        public CardLocation Zone(int id) => Game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location;
        public long Sequence<T>(Func<T, bool> predicate) => Game.Events.Single(item => item.Payload is T fact && predicate(fact)).Sequence;
        public bool AtBeige(string action) => Prompt?.SkillPrompt?.SkillId == Beige &&
            Prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == action);
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native bought-Beige command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = Prompt!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, Game.Revision)); }
        public void EquipCrossbow()
        {
            var action = Game.GetHumanLegalActions().Single(item => item.Kind == LegalActionKind.Equip && item.CardId is not null);
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, Game.Revision, Prompt!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void RequestSlash() => Accept(new UseProgramSkillCommand(0, Driver, "request", [], [1], Game.Revision, Prompt!.PromptId));
        public void Step() => Accept(new AdvanceOneStepCommand(Game.Revision));
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 180; step++)
            {
                if (condition()) return;
                Need(Prompt?.PlayerSeat != 0, "Unexpected human prompt in the bounded bought-Beige scene: " + JsonSerializer.Serialize(Prompt));
                Step();
            }
            throw new InvalidOperationException("Missing bounded bought-Beige boundary: " + JsonSerializer.Serialize(Prompt));
        }
        public static Scene Start(ShanheBattleSetup setup)
        {
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-beige-stock", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "activations":[{"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"requested"}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "请求真实敌军杀", description = "令敌军使用其真实手牌杀攻击本人" } } }));
            builder.AddSkill(new(Driver, "请求真实敌军杀", "由原生请求使用真实实体杀")
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "购入悲歌的普通武将", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Enemy, "真实持杀与两张弃牌的敌军", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "全梅花判定与唯一真实连弩", 1, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "购买悲歌后真实杀伤与梅花判定", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
